#include "Mulpa.h"

UnityDLL float GetSP(int* p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross, float p_crossValue)
{
	if (OutOfDimension(p_i1, p_j1, p_w, p_h))
	{
		return -1.0f;
	}

	if (OutOfDimension(p_i2, p_j2, p_w, p_h))
	{
		return -1.0f;
	}

	int A = CoordToID(p_i1, p_j1, p_w);
	int B = CoordToID(p_i2, p_j2, p_w);

	if (!p_cross)
	{
		if ((p_world[A] == CASE_BLOCK))
		{
			return -1.0f;
		}
	}

	// Création du graphe.

	Graph* graph = new Graph(p_w * p_h);

	// Création des arcs.

	for (int j = 0; j < p_h; j++)
	{
		for (int i = 0; i < p_w; i++)
		{
			int u = CoordToID(i, j, p_w);

			if (!OutOfDimension(i + 1, j, p_w, p_h))
			{
				int v = CoordToID(i + 1, j, p_w);

				if ((p_world[v] == CASE_BLOCK) || (p_world[v] == CASE_DANGER))
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, p_crossValue);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}

			if (!OutOfDimension(i - 1, j, p_w, p_h))
			{
				int v = CoordToID(i - 1, j, p_w);

				if ((p_world[v] == CASE_BLOCK) || (p_world[v] == CASE_DANGER))
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, p_crossValue);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}

			if (!OutOfDimension(i, j + 1, p_w, p_h))
			{
				int v = CoordToID(i, j + 1, p_w);

				if ((p_world[v] == CASE_BLOCK) || (p_world[v] == CASE_DANGER))
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, p_crossValue);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}

			if (!OutOfDimension(i, j - 1, p_w, p_h))
			{
				int v = CoordToID(i, j - 1, p_w);

				if ((p_world[v] == CASE_BLOCK) || (p_world[v] == CASE_DANGER))
				{
					if (p_cross)
					{
						graph->SetWeight(u, v, p_crossValue);
					}
				}
				else
				{
					graph->SetWeight(u, v, 1.0f);
				}
			}
		}
	}

	// Calcule du PCC.

	float distance = 0.0f;
	std::vector<int>* PCC = graph->Dijkstra(A, B, &distance);

	if (PCC)
	{
		delete PCC;
	}

	delete graph;

	return distance;
}
