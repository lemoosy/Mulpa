#pragma once

#include "Matrix.h"
#include "Settings.h"

typedef enum eCaseID
{
	CASE_VOID = ' ',
	CASE_WALL = 'W',
	CASE_PLAYER = 'P',
	CASE_END = 'E',
	CASE_MONSTER = 'M'
}CaseID;

Matrix* World_To_Matrix(char* p_world)
{
	string world(p_world);			// "123x456:..."

	int posX = world.find('x');		// 'x'
	int posF = world.find(':');		// ':'

	int w = stoi(world.substr(0, posX));
	int h = stoi(world.substr(posX + 1, posF - posX - 1));

	Matrix* res = new Matrix(w, h);

	int index = posF + 1;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			res->Set(i, j, (float)world[index]);
			index++;
		}
	}
	
	return res;
}

Matrix* World_To_NN(char* world)
{
	Matrix* worldMatrix = World_To_Matrix(world);

	int w = worldMatrix->GetWidth();
	int h = worldMatrix->GetHeight();

	Matrix* res = new Matrix(NN_NB_NEURAL_INPUT, 1);

	int block = 16 * 8;

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			int idCase = w * j + i;

			switch ((int)worldMatrix->Get(i, j))
			{
			case CASE_VOID:
				break;

			case CASE_PLAYER:
				res->Set(idCase, 0, 1.0f);
				break;

			case CASE_END:
				res->Set(idCase + idCase * 2, 0, 1.0f);
				break;

			case CASE_WALL:
				res->Set(idCase + idCase * 3, 0, 1.0f);
				break;

			case CASE_MONSTER:
				res->Set(idCase + idCase * 4, 0, 1.0f);
				break;

			default:
				break;
			}
		}
	}

	delete worldMatrix;

	return res;
}