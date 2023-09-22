#pragma once

#include "Matrix.h"
#include "Settings.h"

typedef enum eCaseID
{
	CASE_VOID = ' ',
	CASE_WALL = 'O',
	CASE_PLAYER = 'P',
	CASE_END = 'E',
	CASE_TRAP = '!'
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
			res->Set(i, j, world[index]);
			index++;
		}
	}

	return res;
}
