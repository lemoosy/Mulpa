#include "DLL.h"

/// @brief Fenêtre d'affichage.
//Window* g_window;

UnityDLL int Add(int a, int b)
{
	return a + b;
}

UnityDLL double DLL_Init(double p_window)
{
	srand(time(nullptr));

	g_nn = new NN * [NN_CAPACITY]();

	if (p_window == 1.0)
	{
		//g_window = new Window("DLL", WINDOW_WIDTH, WINDOW_HEIGHT);
	}

	return 40.0;
}

UnityDLL double DLL_Free()
{
	for (int i = 0; i < NN_CAPACITY; i++)
	{
		if (g_nn[i])
		{
			delete g_nn[i];
		}
	}

	delete[] g_nn;

	//if (g_window)
	//{

	//}

	return 0.0;
}

UnityDLL double NN_Create()
{
	int id = gNN_GetEmptyID();

	assert(id != -1);

	NN* nn = new NN(512);
	nn->AddLayer(256, &ReLU);
	nn->AddLayer(128, &ReLU);
	nn->AddLayer(3, &sigmoid);

	gNN_SetNN(id, nn);

	return (double)id;
}

UnityDLL double NN_Destroy(double p_id)
{
	gNN_DestroyNN(p_id);

	return 0.0;
}

UnityDLL double NN_Forward(double p_id, char* p_world)
{
	NN* nn = gNN_GetNN(p_id);

	Matrix* world = World_Load(p_world);

	Matrix* X = World_ToNN(world);

	nn->Forward(X);

	delete X;
	
	delete world;

	return 0.0;
}

UnityDLL double NN_GetOutput(double p_id)
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

	return (double)minimum;
}

UnityDLL double NN_GetScore(double p_id)
{
	NN* nn = gNN_GetNN(p_id);

	return (double)nn->GetScore();
}

UnityDLL double NN_SetScore(double p_id, double p_score)
{
	NN* nn = gNN_GetNN(p_id);

	nn->SetScore(p_score);

	return 0.0;
}

UnityDLL double NN_UpdateScore(double p_id, char* p_world)
{
	NN* nn = gNN_GetNN(p_id);

	Matrix* world = World_Load(p_world);

	float distance = 0.0f;
	DList<int>* PCC = World_GetShortestPath(world, &distance);

	nn->SetScore(distance);

	delete PCC;

	delete world;

	return distance;
}

UnityDLL double NN_Crossover(double p_id_1, double p_id_2)
{
	NN* nn_1 = gNN_GetNN(p_id_1);
	NN* nn_2 = gNN_GetNN(p_id_2);
	NN* nn_3 = nn_1->Crossover(nn_2);

	int id_3 = gNN_GetEmptyID();

	assert(id_3 != -1);

	gNN_SetNN(id_3, nn_3);

	return (double)id_3;
}

UnityDLL double NN_Mutation(double p_id, double rate)
{
	NN* nn = gNN_GetNN(p_id);

	for (int i = 0; i < rate; i++)
	{
		nn->Mutation();
	}

	return 0.0;
}
