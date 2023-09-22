#pragma once

#include <cassert>
#include <iostream>

using namespace std;

#define GameMakerDLL extern "C" __declspec (dllexport)

#define NN_CAPACITY		1024
#define NN_SIZE_INPUT	5
#define NN_SIZE_HIDDEN	2
#define NN_SIZE_OUTPUT	5
