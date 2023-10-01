#pragma once

#include <cassert>
#include <iostream>
#include <string>

#include "DList.h"
#include "Matrix.h"
#include "Graph.h"
#include "NN.h"

using namespace std;

typedef enum eCaseID
{
	CASE_WALL,
	CASE_MONSTER,
	CASE_COIN,
	CASE_PLAYER,
	CASE_EXIT,
	CASE_BUTTON,
	CASE_DOOR,
	CASE_COUNT
}CaseID;

/// Macros.

#define UnityDLL			extern "C" __declspec (dllexport)

#define WORLD_MATRIX_W		18
#define WORLD_MATRIX_H		10

#define NN_INPUT_SIZE		(WORLD_MATRIX_W * WORLD_MATRIX_H) * CASE_COUNT

/// Variables globales.

extern int g_populationSize;
extern int g_selectionSize;
extern int g_childrenSize;
extern int g_mutationRate;

extern NN** g_population;
