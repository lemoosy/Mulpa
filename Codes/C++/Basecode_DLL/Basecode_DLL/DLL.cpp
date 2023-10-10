#include "DLL.h"

int g_populationSize = 0;
int g_selectionSize = 0;
int g_childrenSize = 0;
int g_mutationRate = 0;

NN** g_population = nullptr;

// PG

UnityDLL void DLL_PG_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate)
{
	srand(time(nullptr));

	g_populationSize = p_populationSize;
	g_selectionSize = p_selectionSize;
	g_childrenSize = p_childrenSize;
	g_mutationRate = p_mutationRate;

	g_population = new NN*[p_populationSize]();

	for (int i = 0; i < p_populationSize; i++)
	{
		g_population[i] = NN_Create();
	}
}

UnityDLL void DLL_PG_Quit(void)
{
	Population_Clear();

	delete[] g_population;
}

UnityDLL void DLL_PG_SetScore(int p_populationIndex, float p_score)
{
	g_population[p_populationIndex]->SetScore(p_score);
}

UnityDLL float DLL_PG_GetScore(int p_populationIndex)
{
	return g_population[p_populationIndex]->GetScore();
}

UnityDLL void DLL_PG_Update(void)
{
	// Création de la sélection.

	NN** selection = new NN*[g_selectionSize]();

	for (int i = 0; i < g_selectionSize; i++)
	{
		selection[i] = Population_RemoveMinimum();
	}

	// Nettoyage de la population.

	Population_Clear();

	// Création des enfants.

	NN** children = new NN*[g_childrenSize]();

	int childrenSize = 0; // Nombre d'enfants créés.

	for (int i = 0; i < g_childrenSize; i++)
	{
		NN* nn1 = selection[rand() % g_selectionSize];
		NN* nn2 = selection[rand() % g_selectionSize];

		if (nn1 == nn2) continue;

		children[childrenSize] = nn1->Crossover(nn2);
		
		for (int j = 0; j < g_mutationRate; j++)
		{
			children[childrenSize]->Mutation();
		}

		childrenSize++;
	}

	// On transfère la sélection et les enfants dans la population.

	for (int i = 0; i < g_selectionSize; i++)
	{
		g_population[i] = selection[i];
	}

	delete selection;

	for (int i = 0; i < childrenSize; i++)
	{
		g_population[g_selectionSize + i] = children[i];
	}

	delete children;

	// Remplissage de la population.

	int size = g_selectionSize + childrenSize;

	for (int i = size; i < g_populationSize; i++)
	{
		g_population[i] = NN_Create();
	}
}

UnityDLL void DLL_PG_Forward(int p_populationIndex, int* p_world, int p_w, int p_h)
{
	NN* nn = g_population[p_populationIndex];
	
	Matrix* X = World_ToInput(p_world, p_w, p_h);

	nn->Forward(X);

	delete X;
}

UnityDLL int DLL_PG_GetOutput(int p_populationIndex)
{
	NN* nn = g_population[p_populationIndex];

	Layer* output = nn->GetLayer(-1);

	Matrix* Y = output->m_Y;

	int index = 0;

	for (int i = 1; i < Y->GetWidth(); i++)
	{
		if (Y->GetValue(i, 0) > Y->GetValue(index, 0))
		{
			index = i;
		}
	}

	return index;
}

// PCC

UnityDLL float DLL_PCC(int* p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross)
{
	if (Coord_OutOfDimension(p_i1, p_j1, p_w, p_h) ||
		Coord_OutOfDimension(p_i2, p_j2, p_w, p_h) ||
		(!p_cross && p_world[Coord_ToIndex(p_i1, p_j1, p_w)] == CASE_WALL)
		)
	{
		return -1.0f;
	}

	// Création du graphe.

	Graph* graph = new Graph(p_w * p_h);

	// Création des arcs.

	for (int j = 0; j < p_h; j++)
	{
		for (int i = 0; i < p_w; i++)
		{
			int u = Coord_ToIndex(i, j, p_w);

			if (!Coord_OutOfDimension(i + 1, j, p_w, p_h))
			{
				int v = Coord_ToIndex(i + 1, j, p_w);

				if (p_world[v] == CASE_WALL)
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, 10.0f);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}

			if (!Coord_OutOfDimension(i - 1, j, p_w, p_h))
			{
				int v = Coord_ToIndex(i - 1, j, p_w);

				if (p_world[v] == CASE_WALL)
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, 10.0f);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}

			if (!Coord_OutOfDimension(i, j + 1, p_w, p_h))
			{
				int v = Coord_ToIndex(i, j + 1, p_w);

				if (p_world[v] == CASE_WALL)
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, 10.0f);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}

			if (!Coord_OutOfDimension(i, j - 1, p_w, p_h))
			{
				int v = Coord_ToIndex(i, j - 1, p_w);

				if (p_world[v] == CASE_WALL)
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, 10.0f);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}
		}
	}

	// Calcule du PCC.

	int u = Coord_ToIndex(p_i1, p_j1, p_w);
	int v = Coord_ToIndex(p_i2, p_j2, p_w);

	float distance = 0.0f;
	DList<int>* PCC = graph->Dijkstra(u, v, &distance);

	if (PCC)
	{
		delete PCC;
	}

	delete graph;

	return distance;
}
