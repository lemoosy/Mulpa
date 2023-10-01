#include "DLL.h"

int g_populationSize = 0;
int g_selectionSize = 0;
int g_childrenSize = 0;
int g_mutationRate = 0;

NN** g_population = nullptr;

// Fonctions pour le script scr_playerAI.cs

UnityDLL void DLL_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate)
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

UnityDLL void DLL_Quit(void)
{
	Population_Clear();

	delete[] g_population;
}

UnityDLL void DLL_NN_SetScore(int p_nnIndex, float score)
{
	g_population[p_nnIndex]->SetScore(score);
}

UnityDLL float DLL_NN_GetScore(int p_nnIndex)
{
	return g_population[p_nnIndex]->GetScore();
}

UnityDLL bool DLL_Population_Update(void)
{
	// On vérifie si tous les NN ont été évalués.

	for (int i = 0; i < g_populationSize; i++)
	{
		NN* nn = g_population[i];

		if ((nn == nullptr) || (nn->GetScore() == (float)INT_MAX))
		{
			return true;
		}
	}

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

		children[i] = nn1->Crossover(nn2);
		
		for (int j = 0; j < g_mutationRate; j++)
		{
			children[i]->Mutation();
		}

		childrenSize++;
	}

	// On transfère la sélection et les enfants dans la population.

	for (int i = 0; i < g_selectionSize; i++)
	{
		g_population[i] = selection[i];
	}

	int size = g_selectionSize + childrenSize;

	for (int i = g_selectionSize; i < size; i++)
	{
		g_population[i] = children[i];
	}

	// Remplissage de la population.

	for (int i = size; i < g_populationSize; i++)
	{
		g_population[i] = NN_Create();
	}

	// Libération de la mémoire.

	delete children;
	delete selection;

	return false;
}

// Fonctions pour le script scr_player.cs

UnityDLL bool DLL_NN_Forward(int p_nnIndex, int* p_world, int p_w, int p_h)
{
	NN* nn = g_population[p_nnIndex];

	if (!nn)
	{
		return true;
	}

	Matrix* X = World_ToInput(p_world, p_w, p_h);

	if (!X)
	{
		return true;
	}

	nn->Forward(X);

	delete X;

	return false;
}

UnityDLL int DLL_NN_GetOutput(int p_nnIndex)
{
	NN* nn = g_population[p_nnIndex];

	if (!nn)
	{
		return -1;
	}

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

UnityDLL float DLL_World_GetShortestPath(int* p_world, int p_w, int p_h)
{
	if ((p_w != WORLD_MATRIX_W) || (p_h != WORLD_MATRIX_H))
	{
		return -1.0f;
	}

	// Création du graphe.

	Graph* graph = new Graph(p_w * p_h);

	// Création des arcs + Recherche du joueur et de la sortie.

	int playerID = -1;
	int exitID = -1;

	for (int j = 0; j < p_h; j++)
	{
		for (int i = 0; i < p_w; i++)
		{
			int index = Coord_ToIndex(i, j, p_w);
			int value = p_world[index];

			if (value == CASE_PLAYER)
			{
				playerID = index;
			}

			if (value == CASE_EXIT)
			{
				exitID = index;
			}

			if (
				!Coord_OutOfDimension(i + 1, j, p_w, p_h)
				&&
				(p_world[Coord_ToIndex(i + 1, j, p_w)] != CASE_WALL)
				)
			{
				graph->SetWeight(
					index,
					Coord_ToIndex(i + 1, j, p_w),
					1.0f
				);
			}

			if (
				!Coord_OutOfDimension(i - 1, j, p_w, p_h)
				&&
				(p_world[Coord_ToIndex(i - 1, j, p_w)] != CASE_WALL)
				)
			{
				graph->SetWeight(
					index,
					Coord_ToIndex(i - 1, j, p_w),
					1.0f
				);
			}

			if (
				!Coord_OutOfDimension(i, j + 1, p_w, p_h)
				&&
				(p_world[Coord_ToIndex(i, j + 1, p_w)] != CASE_WALL)
				)
			{
				graph->SetWeight(
					index,
					Coord_ToIndex(i, j + 1, p_w),
					1.0f
				);
			}

			if (
				!Coord_OutOfDimension(i, j - 1, p_w, p_h)
				&&
				(p_world[Coord_ToIndex(i, j - 1, p_w)] != CASE_WALL)
				)
			{
				graph->SetWeight(
					index,
					Coord_ToIndex(i, j - 1, p_w),
					1.0f
				);
			}
		}
	}

	if ((playerID == -1) || (exitID == -1))
	{
		delete graph;
		return -1.0f;
	}

	// Calcule du PCC.

	float distance = 0.0f;
	DList<int>* PCC = graph->Dijkstra(playerID, exitID, &distance);

	if (PCC)
	{
		delete PCC;
	}

	delete graph;

	return distance;
}
