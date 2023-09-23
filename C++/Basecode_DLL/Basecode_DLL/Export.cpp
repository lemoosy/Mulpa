#include "Settings.h"
#include "Variables.h"
#include "Graph.h"
#include "Utils.h"
#include "Functions.h"

// ------------------------------ DLL ------------------------------

GameMakerDLL double DLL_Init()
{
	srand(time(nullptr));

	g_nn = new NN * [NN_CAPACITY]();

	return (double)EXIT_SUCCESS;
}

GameMakerDLL double DLL_Free()
{
	for (int i = 0; i < NN_CAPACITY; i++)
	{
		if (g_nn[i])
		{
			delete g_nn[i];
		}
	}

	delete[] g_nn;

	return (double)EXIT_SUCCESS;
}

// ------------------------------ NN ------------------------------

/// @return -1 si erreur, sinon ID.
int NN_GetEmptyID(void)
{
	for (int i = 0; i < NN_CAPACITY; i++)
	{
		if (g_nn[i] == nullptr)
		{
			return i;
		}
	}

	return -1;
}

/// @return -1 si erreur, sinon ID.
GameMakerDLL double NN_Create()
{
	int nnID = NN_GetEmptyID();

	if (nnID == -1)
	{
		return -1;
	}

	NN* nn = new NN(NN_NB_NEURAL_INPUT);

	for (int i = 0; i < NN_NB_LAYER_HIDDEN; i++)
	{
		nn->AddLayer(NN_NB_NEURAL_HIDDEN, &sigmoid);
	}

	nn->AddLayer(NN_NB_NEURAL_OUTPUT, &sigmoid);

	g_nn[nnID] = nn;

	return (double)nnID;
}

/// @return EXIT_FAILURE si erreur, sinon EXIT_SUCCESS.
GameMakerDLL double NN_Destroy(double p_nnID)
{
	int nnID = (int)p_nnID;

	if ((nnID < 0) || (nnID >= NN_CAPACITY)/* || (g_nn[nnID] == nullptr)*/)
	{
		return (double)EXIT_FAILURE;
	}

	delete g_nn[nnID];

	return (double)EXIT_SUCCESS;
}

/// @return EXIT_FAILURE si erreur, sinon EXIT_SUCCESS.
GameMakerDLL double NN_Forward(double p_nnID, char* world)
{
	int nnID = (int)p_nnID;

	assert(nnID != -1);

	if ((nnID < 0) || (nnID >= NN_CAPACITY) || (g_nn[nnID] == nullptr) || !world)
	{
		return (double)EXIT_FAILURE;
	}

	Matrix* X = World_To_NN(world);

	g_nn[nnID]->Forward(X);

	delete X;

	return (double)EXIT_SUCCESS;
}

/// @return -1 si erreur, sinon une valeur dans [0, 1, 2].
GameMakerDLL double NN_GetOutput(double p_nnID)
{
	int nnID = (int)p_nnID;

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

	return (double)minimum;
}

/// @return -1 si erreur.
GameMakerDLL double NN_GetScore(double p_nnID)
{
	int nnID = (int)p_nnID;

	if (g_nn[nnID] == nullptr)
	{
		return -1.0;
	}

	return (double)g_nn[nnID]->m_score;
}

GameMakerDLL double NN_Crossover(double p_nnID_1, double p_nnID_2)
{
	int nnID_1 = (int)p_nnID_1;
	int nnID_2 = (int)p_nnID_2;

	NN* nn_1 = g_nn[nnID_1];
	NN* nn_2 = g_nn[nnID_2];

	assert(nn_1);
	assert(nn_2);

	int nnID = NN_GetEmptyID();
	assert(nnID != -1);

	g_nn[nnID] = nn_1->Crossover(nn_2);

	return nnID;
}

// ------------------------------ ShortestPath ------------------------------

int Coord_ToID(int i, int j, int w)
{
	return (j * w + i);
}

GameMakerDLL double ShortestPath_Get(char* world)
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
					Coord_ToID(i, j, w),
					Coord_ToID(i + 1, j, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i - 1, j) == false && matrix->Get(i - 1, j) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_ToID(i, j, w),
					Coord_ToID(i - 1, j, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i, j + 1) == false && matrix->Get(i, j + 1) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_ToID(i, j, w),
					Coord_ToID(i, j + 1, w),
					1.0f
				);
			}

			if (matrix->OutOfDimension(i, j - 1) == false && matrix->Get(i, j - 1) != CASE_WALL)
			{
				graph->SetWeight(
					Coord_ToID(i, j, w),
					Coord_ToID(i, j - 1, w),
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
				startID = Coord_ToID(i, j, w);
			}

			if (matrix->Get(i, j) == CASE_END)
			{
				endID = Coord_ToID(i, j, w);
			}
		}
	}

	assert(endID != -1);

	if (startID == -1)
	{
		delete graph;
		delete matrix;

		return 1000.0;
	}

	/// Calcule du plus court chemin.

	float distance = 0.0f;
	List<int>* tab = graph->Dijkstra(startID, endID, &distance);

	delete tab;
	delete graph;
	delete matrix;

	return distance;
}
