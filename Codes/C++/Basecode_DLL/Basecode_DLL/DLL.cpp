#include "DLL.h"

UnityDLL void DLL_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate)
{
	srand(time(nullptr));

	g_populationSize = p_populationSize;
	g_selectionSize = p_selectionSize;
	g_childrenSize = p_childrenSize;

	g_population = new NN*[p_populationSize]();

	for (int i = 0; i < p_populationSize; i++)
	{
		g_population[i] = NN_Create();
	}
}

UnityDLL void DLL_Quit(void)
{
	for (int i = 0; i < g_populationSize; i++)
	{
		delete g_population[i];
	}

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

UnityDLL int DLL_Population_Update()
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

UnityDLL void NN_Forward(int p_id, int* p_world, int w, int h)
{
	NN* nn = gNN_GetNN(p_id);

	Matrix* world = World_LoadMatrix(p_world);

	Matrix* X = World_ToInputsNN(world);

	nn->Forward(X);

	delete X;
	
	delete world;
}

UnityDLL int NN_GetOutput(int p_id)
{
	NN* nn = gNN_GetNN(p_id);

	Layer* output = nn->GetLayer(-1);

	Matrix* Y = output->m_Y;

	int minimum = 0;

	for (int i = 1; i < Y->GetWidth(); i++)
	{
		if (Y->GetValue(i, 0) > Y->GetValue(minimum, 0))
		{
			minimum = i;
		}
	}

	return minimum;
}
