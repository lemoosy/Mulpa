#include "Functions.h"
#include "NN.h"

int main(void)
{
	NN nn(2);
	nn.AddLayer(2, sigmoid);
	nn.AddLayer(2, sigmoid);
	nn.Print(0);
	nn.Print(1);

	Matrix X = Matrix(2, 1);
	X.SetValue(0, 0, 0.0f);
	X.SetValue(1, 0, 1.0f);

	nn.Forward(&X);

	return EXIT_SUCCESS;
}
