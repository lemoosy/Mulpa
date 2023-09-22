#include "Functions.h"
#include "NN.h"

int main()
{
	NN nn(2);
	nn.AddLayer(2, sigmoid);
	nn.AddLayer(2, sigmoid);
	nn.Print(0);
	nn.Print(1);

	Matrix X = Matrix(2, 1);
	X.Set(0, 0, 0.0f);
	X.Set(1, 0, 1.0f);

	nn.Forward(&X);

	return EXIT_SUCCESS;
}
