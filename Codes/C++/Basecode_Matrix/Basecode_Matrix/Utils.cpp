#include "Utils.h"

int Int_Random(int a, int b)
{
	assert(a < b);

	return (rand() % (b - a + 1) + a);
}

float Float_Random(float a, float b)
{
	assert(a < b);

	float r = (float)rand();	// [0, RAND_MAX]
	r /= (float)RAND_MAX;		// [0, 1]
	r *= (b - a);				// [0, b - a]
	r += a;						// [a, b]

	return r;
}
