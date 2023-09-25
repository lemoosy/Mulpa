#include "gNN.h"

int gNN_GetEmptyID(void)
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

NN* gNN_GetNN(int p_id)
{
	assert((0 <= p_id) && (p_id < NN_CAPACITY));

	return g_nn[p_id];
}

void gNN_SetNN(int p_id, NN* p_nn)
{
	assert((0 <= p_id) && (p_id < NN_CAPACITY));
	assert(g_nn[p_id] == nullptr);

	g_nn[p_id] = p_nn;
}

void gNN_DestroyNN(int p_id)
{
	assert((0 <= p_id) && (p_id < NN_CAPACITY));

	if (g_nn[p_id])
	{
		delete g_nn[p_id];

		g_nn[p_id] = nullptr;
	}
}
