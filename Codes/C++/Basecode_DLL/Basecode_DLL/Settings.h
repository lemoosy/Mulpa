#pragma once

#include <cassert>
#include <iostream>
#include <string>

using namespace std;

#define GameMakerDLL extern "C" __declspec (dllexport)

#define NN_CAPACITY				1024

#define NN_NB_NEURAL_INPUT		512

#define NN_NB_LAYER_HIDDEN		3
#define NN_NB_NEURAL_HIDDEN		512

#define NN_NB_NEURAL_OUTPUT		3
