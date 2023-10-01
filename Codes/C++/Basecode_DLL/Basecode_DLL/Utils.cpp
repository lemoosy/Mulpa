#include "Utils.h"

NN* NN_Create(void)
{
	NN* nn = new NN(NN_INPUT_SIZE);

	nn->AddLayer(256, &ReLU);
	nn->AddLayer(128, &ReLU);
	nn->AddLayer(3, &sigmoid);

	nn->SetScore((float)INT_MAX);

	return nn;
}

NN* Population_RemoveMinimum(void)
{
	int index = 0;

	for (int i = 1; i < g_populationSize; i++)
	{
		if (g_population[i])
		{
			if (g_population[i]->GetScore() < g_population[index]->GetScore())
			{
				index = i;
			}
		}
	}

	NN* res = g_population[index];
	
	g_population[index] = nullptr;

	return res;
}

NN* Population_RemoveMaximum(void)
{
	int index = 0;

	for (int i = 1; i < g_populationSize; i++)
	{
		if (g_population[i])
		{
			if (g_population[i]->GetScore() > g_population[index]->GetScore())
			{
				index = i;
			}
		}
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

Matrix* World_ToInput(int* p_world, int w, int h)
{
	if ((w != WORLD_MATRIX_W) || (h != WORLD_MATRIX_H))
	{
		return nullptr;
	}

	Matrix* res = new Matrix(NN_INPUT_SIZE, 1);

	int size = w * h;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			int index = (j * w) + i;

			int value = p_world[index];

			res->SetValue(index + size * value, 0, 1.0f);
		}
	}

	return res;
}

bool Coord_OutOfDimension(int i, int j, int w, int h)
{
	return ((i < 0) || (j < 0) || (i >= w) || (j >= h));
}

int Coord_ToIndex(int i, int j, int w)
{
	return (j * w) + i;
}
