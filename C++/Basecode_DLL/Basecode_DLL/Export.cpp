#include "Settings.h"
#include "Variables.h"
#include "Graph.h"
#include "Utils.h"

// DLL

GameMakerDLL int DLL_Init()
{
	g_nn = new NN * [NN_CAPACITY];
	g_size = 0;

	return EXIT_SUCCESS;
}

GameMakerDLL int DLL_Free()
{
	for (int i = 0; i < g_size; i++)
	{
		delete g_nn[i];
	}

	delete[] g_nn;

	return EXIT_SUCCESS;
}

// NN

GameMakerDLL int NN_Create()
{
	if (g_size >= NN_CAPACITY)
	{
		return -1;
	}

	int res = g_size;

	g_nn[g_size] = new NN(NN_SIZE_INPUT);
	g_size++;

	return g_size;
}

GameMakerDLL double NN_Destroy()
{
	return EXIT_SUCCESS;
}

GameMakerDLL int NN_Forward(int nnID, char* world)
{
	if ((nnID < 0) || (nnID >= NN_CAPACITY) || !world)
	{
		return EXIT_FAILURE;
	}

	NN* nn = g_nn[nnID];

	if (!nn)
	{
		return EXIT_FAILURE;
	}

	Matrix* X = World_To_Matrix(world);

	nn->Forward(X);

	delete X;

	return EXIT_SUCCESS;
}

GameMakerDLL int NN_GetOutput(int nnID)
{
	if ((nnID < 0) || (nnID >= NN_CAPACITY))
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

	return minimum;
}

// ShortestPath

int Coord_To_ID(int i, int j, int w)
{
	return j * w + i;
}

GameMakerDLL double ShortestPath_Get(int nnID, char* world)
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
			if (matrix->OutOfDimension(i + 1, j) == false)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i + 1, j, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i - 1, j) == false)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i - 1, j, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i, j + 1) == false)
			{
				graph->SetWeight(
					Coord_To_ID(i, j, w),
					Coord_To_ID(i, j + 1, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i, j - 1) == false)
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
