#include "Utils.h"

NN* NN_Create(void)
{
	NN* nn = new NN(NN_INPUT_SIZE);

	nn->AddLayer(512, &ReLU);
	nn->AddLayer(256, &ReLU);
	nn->AddLayer(3, &sigmoid);

	nn->SetScore((float)INT_MAX);

	return nn;
}

NN* Population_RemoveMinimum(void)
{
	int index = -1;

	for (int i = 0; i < g_populationSize; i++)
	{
		if (g_population[i])
		{
			if ((index == -1) || (g_population[i]->GetScore() < g_population[index]->GetScore()))
			{
				index = i;
			}
		}
	}

	if (index == -1)
	{
		return nullptr;
	}

	NN* res = g_population[index];
	
	g_population[index] = nullptr;

	return res;
}

NN* Population_RemoveMaximum(void)
{
	int index = -1;

	for (int i = 0; i < g_populationSize; i++)
	{
		if (g_population[i])
		{
			if ((index == -1) || (g_population[i]->GetScore() > g_population[index]->GetScore()))
			{
				index = i;
			}
		}
	}

	if (index == -1)
	{
		return nullptr;
	}

	NN* res = g_population[index];

	g_population[index] = nullptr;

	return res;
}

void Population_Clear(void)
{
	for (int i = 0; i < g_populationSize; i++)
	{
		if (g_population[i])
		{
			delete g_population[i];
			g_population[i] = nullptr;
		}
	}
}

Matrix* World_ToInput(int* p_world, int p_w, int p_h)
{
	if ((p_w != WORLD_MATRIX_W) || (p_h != WORLD_MATRIX_H))
	{
		return nullptr;
	}

	Matrix* res = new Matrix(NN_INPUT_SIZE, 1);

	int size = p_w * p_h;

	for (int j = 0; j < p_h; j++)
	{
		for (int i = 0; i < p_w; i++)
		{
			int index = Coord_ToIndex(i, j, p_w);
			int value = p_world[index];

			if (value == CASE_VOID) continue;

			res->SetValue(index + size * (value - 1), 0, 1.0f);
		}
	}

	return res;
}

bool Coord_OutOfDimension(int p_i, int p_j, int p_w, int p_h)
{
	return ((p_i < 0) || (p_j < 0) || (p_i >= p_w) || (p_j >= p_h));
}

int Coord_ToIndex(int p_i, int p_j, int p_w)
{
	return (p_j * p_w) + p_i;
}
