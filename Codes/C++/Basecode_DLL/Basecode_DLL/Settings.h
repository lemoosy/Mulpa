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

#define WORLD_MATRIX_WIDTH		18
#define WORLD_MATRIX_HEIGHT		10

#define CASE_COUNT				7

int g_populationSize = 0;
int g_selectionSize = 0;
int g_childrenSize = 0;

NN** g_population = nullptr;