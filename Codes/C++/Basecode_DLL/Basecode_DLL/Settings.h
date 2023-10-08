#pragma once

#include <cassert>
#include <iostream>
#include <string>

#include "DList.h"
#include "Graph.h"
#include "Matrix.h"
#include "NN.h"

using namespace std;

typedef enum eCaseID
{
	CASE_VOID,
	CASE_WALL,
	CASE_ATTACK,
	CASE_COIN,
	CASE_PLAYER,
	CASE_EXIT,
	CASE_COUNT
}CaseID;

/// Macros.

#define UnityDLL			extern "C" __declspec (dllexport)

#define WORLD_MATRIX_W		24
#define WORLD_MATRIX_H		14

#define NN_INPUT_SIZE		(WORLD_MATRIX_W * WORLD_MATRIX_H) * (CASE_COUNT - 1)

/// Variables globales.

extern int g_populationSize;
extern int g_selectionSize;
extern int g_childrenSize;
extern int g_mutationRate;

extern NN** g_population;
