#include "Settings.h"
#include "Variables.h"
#include "Graph.h"
#include "Utils.h"
#include "Functions.h"

// DLL

GameMakerDLL double DLL_Init()
{
	srand(time(nullptr));

	g_nn = new NN * [NN_CAPACITY];
	g_size = 0;

	return (double)EXIT_SUCCESS;
}

GameMakerDLL double DLL_Free()
{
	for (int i = 0; i < g_size; i++)
	{
		delete g_nn[i];
	}

	delete[] g_nn;

	return (double)EXIT_SUCCESS;
}

// NN

GameMakerDLL double NN_Create()
{
	if (g_size >= NN_CAPACITY)
	{
		return -1;
	}

	int res = g_size;

	NN* nn = new NN(NN_NB_NEURAL_INPUT);

	for (int i = 0; i < NN_NB_LAYER_HIDDEN; i++)
	{
		nn->AddLayer(NN_NB_NEURAL_HIDDEN, &sigmoid);
	}

	nn->AddLayer(NN_NB_NEURAL_OUTPUT, &sigmoid);

	g_nn[g_size] = nn;

	g_size++;

	return (double)res;
}

GameMakerDLL double NN_Destroy()
{
	return EXIT_SUCCESS;
}

GameMakerDLL double NN_Forward(double p_nnID, char* world)
{
	int nnID = (int)p_nnID;

	if ((nnID < 0) || (nnID >= g_size) || !world)
	{
		return (double)EXIT_FAILURE;
	}

	NN* nn = g_nn[nnID];

	if (!nn)
	{
		return EXIT_FAILURE;
	}

	Matrix* X = World_To_Matrix(world);

	nn->Forward(X);

	delete X;

	return (double)EXIT_SUCCESS;
}

GameMakerDLL double NN_GetOutput(double p_nnID)
{
	int nnID = (int)p_nnID;

	if ((nnID < 0) || (nnID >= g_size))
	{
		return -1;
	}

	NN* nn = g_nn[nnID];

	if (!nn)
	{
		return -1;
	}

	Layer* output = nn->GetLayer(-1);

	Matrix* Y = output->m_Y;

	int minimum = 0;

	for (int i = 1; i < Y->GetWidth(); i++)
	{
		if (Y->Get(i, 0) > Y->Get(minimum, 0))
		{
			minimum = i;
		}
	}

	return (double)minimum;
}

// ShortestPath

int Coord_To_ID(int i, int j, int w)
{
	return j * w + i;
}

GameMakerDLL double ShortestPath_Get(double nnID, char* world)
{
	Matrix* matrix = World_To_Matrix(world);

	int w = matrix->GetWidth();
	int h = matrix->GetHeight();

	/// Création du graphe.

	Graph* graph = new Graph(w * h);

	/// Création des chemins.

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			if (matrix->OutOfDimension(i + 1, j) == false && matrix->Get(i + 1, j) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i + 1, j, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i - 1, j) == false && matrix->Get(i - 1, j) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i - 1, j, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i, j + 1) == false && matrix->Get(i, j + 1) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i, j + 1, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i, j - 1) == false && matrix->Get(i, j - 1) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i, j - 1, w),
					1.0f
				);
			}
		}
	}

	/// Recherche du joueur et de la sortie.

	int startID = -1;
	int endID = -1;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			if (matrix->Get(i, j) == CASE_PLAYER)
			{
				startID = Coord_To_ID(i, j, w);
			}

			if (matrix->Get(i, j) == CASE_END)
			{
				endID = Coord_To_ID(i, j, w);
			}
		}
	}

	/// Calcule du plus court chemin.

	float distance = 0.0f;
	List<int>* tab = graph->Dijkstra(startID, endID, &distance);

	delete tab;
	delete graph;
	delete matrix;

	return distance;
}
