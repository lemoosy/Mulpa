#include "Settings.h"
#include "Variables.h"
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

GameMakerDLL double ShortestPath_Get(int nnID, char* world)
{

}