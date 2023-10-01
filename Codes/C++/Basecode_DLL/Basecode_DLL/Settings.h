#pragma once

#include <cassert>
#include <iostream>
#include <string>

#include "DList.h"
#include "Matrix.h"
#include "Graph.h"
#include "NN.h"

using namespace std;

#define UnityDLL extern "C" __declspec (dllexport)

#define WORLD_MATRIX_W		18
#define WORLD_MATRIX_H		10
#define NN_INPUT_SIZE		(WORLD_MATRIX_W * WORLD_MATRIX_H)

int g_populationSize = 0;
int g_selectionSize = 0;
int g_childrenSize = 0;
int g_mutationRate = 0;

NN** g_population = nullptr;
