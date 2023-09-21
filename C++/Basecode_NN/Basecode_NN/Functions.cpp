#include "Functions.h"

float sigmoid(float x)
{
	return (1.0f / (1.0f + exp(-x)));
}

float ReLU(float x)
{
	return 0.0f;
}
