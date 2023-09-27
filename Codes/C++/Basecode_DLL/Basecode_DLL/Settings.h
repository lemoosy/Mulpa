#pragma once

#include <cassert>
#include <iostream>
#include <string>

#include "DList.h"
#include "Matrix.h"
#include "Graph.h"
#include "NN.h"

//#include "Window.h"
//#include "SDL.h"

using namespace std;

#define UnityDLL extern "C" __declspec (dllexport)	/// Pour la DLL.

#define NN_CAPACITY				1024					/// Nombre de réseaux de neurones maximum (Dashboard::nn).
#define NN_INPUT_SIZE			512						/// Taille de l'entrée du réseau de neurones.

#define WINDOW_DISPLAY			false
#define WINDOW_WIDTH			256
#define WINDOW_HEIGHT			128
#define WINDOW_CASE_SIZE		16

#define WORLD_MATRIX_WIDTH		16
#define WORLD_MATRIX_HEIGHT		8
