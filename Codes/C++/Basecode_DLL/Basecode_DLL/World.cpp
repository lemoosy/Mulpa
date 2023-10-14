#include "World.h"

Matrix* World_LoadMatrix(string p_world)
{
	int posX = p_world.find('x');			// 'x'
	int posSep = p_world.find(':');		// ':'

	int w = stoi(p_world.substr(0, posX));
	int h = stoi(p_world.substr(posX + 1, posSep - posX - 1));

	Matrix* res = new Matrix(w, h);

	int index = posSep + 1;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			res->SetValue(i, j, p_world[index]);
			index++;
		}
	}

	return res;
}

Matrix* World_ToInputsNN(Matrix* p_world)
{
	int w = p_world->GetWidth();
	int h = p_world->GetHeight();

	Matrix* res = new Matrix(NN_INPUT_SIZE, 1);

	int block = WORLD_MATRIX_WIDTH * WORLD_MATRIX_HEIGHT;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			int index = World_CoordToID(i, j, w);

			switch ((int)p_world->GetValue(i, j))
			{
			case CASE_VOID:
				break;

			case CASE_PLAYER:
				res->SetValue(index + block * 0, 0, 1.0f);
				break;

			case CASE_END:
				res->SetValue(index + block * 1, 0, 1.0f);
				break;

			case CASE_WALL:
				res->SetValue(index + block * 2, 0, 1.0f);
				break;

			case CASE_MONSTER:
				res->SetValue(index + block * 3, 0, 1.0f);
				break;

			default:
				assert(false);
				break;
			}
		}
	}

	return res;
}

DList<int>* World_GetShortestPath(Matrix* p_world, float* p_distance)
{
	int w = p_world->GetWidth();
	int h = p_world->GetHeight();

	/// Création du graphe.

	Graph* graph = new Graph(w * h);

	/// Création des arcs.

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			if (
				(p_world->OutOfDimension(i + 1, j) == false) &&
				(p_world->GetValue(i + 1, j) != CASE_WALL)
				)
			{
				graph->SetWeight(
					World_CoordToID(i, j, w),
					World_CoordToID(i + 1, j, w),
					1.0f
				);
			}

			if (
				(p_world->OutOfDimension(i - 1, j) == false) && 
				(p_world->GetValue(i - 1, j) != CASE_WALL)
				)
			{
				graph->SetWeight(
					World_CoordToID(i, j, w),
					World_CoordToID(i - 1, j, w),
					1.0f
				);
			}

			if (
				(p_world->OutOfDimension(i, j + 1) == false) && 
				(p_world->GetValue(i, j + 1) != CASE_WALL)
				)
			{
				graph->SetWeight(
					World_CoordToID(i, j, w),
					World_CoordToID(i, j + 1, w),
					1.0f
				);
			}

			if (
				(p_world->OutOfDimension(i, j - 1) == false) && 
				(p_world->GetValue(i, j - 1) != CASE_WALL)
				)
			{
				graph->SetWeight(
					World_CoordToID(i, j, w),
					World_CoordToID(i, j - 1, w),
					1.0f
				);
			}
		}
	}

	/// Recherche du joueur et de la sortie.

	int start = -1;
	int end = -1;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			if (p_world->GetValue(i, j) == CASE_PLAYER)
			{
				start = World_CoordToID(i, j, w);
			}

			if (p_world->GetValue(i, j) == CASE_END)
			{
				end = World_CoordToID(i, j, w);
			}
		}
	}

	assert(start != -1);
	assert(end != -1);

	/// Calcule du plus court chemin.

	DList<int>* PCC = graph->Dijkstra(start, end, p_distance);

	delete graph;

	return PCC;
}
