#include "Utils.h"

NN* NN_Create(void)
{
	NN* nn = new NN(512);

	nn->AddLayer(256, &ReLU);
	nn->AddLayer(128, &ReLU);
	nn->AddLayer(3, &sigmoid);

	nn->SetScore((float)INT_MAX);

	return nn;
}

NN* NN_PopulationRemoveMinimum(void)
{
	int index = 0;

	for (int i = 1; i < g_populationSize; i++)
	{
		if (g_population[i]->GetScore() < g_population[index]->GetScore())
		{
			index = i;
		}
	}

	NN* res = g_population[index];
	
	g_population[index] = nullptr;

	return res;
}











UnityDLL void NN_UpdateScore(int p_id, string p_world)
{
	if (p_world == "") return;

	NN* nn = gNN_GetNN(p_id);

	Matrix* world = World_LoadMatrix(p_world);

	float distance = 0.0f;
	DList<int>* PCC = World_GetShortestPath(world, &distance);

	nn->SetScore(distance);

	delete PCC;

	delete world;
}

UnityDLL int NN_Crossover(int p_id_1, int p_id_2)
{
	NN* nn_1 = gNN_GetNN(p_id_1);
	NN* nn_2 = gNN_GetNN(p_id_2);
	NN* nn_3 = nn_1->Crossover(nn_2);

	int id_3 = gNN_GetEmptyID();

	assert(id_3 != -1);

	gNN_SetNN(id_3, nn_3);

	return id_3;
}

UnityDLL void NN_Mutation(int p_id, int rate)
{
	NN* nn = gNN_GetNN(p_id);

	for (int i = 0; i < rate; i++)
	{
		nn->Mutation();
	}
}
