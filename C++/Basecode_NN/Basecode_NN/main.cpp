#include "Functions.h"
#include "NN.h"

int main()
{
	NN nn(3);

	nn.AddLayer(2, sigmoid);
	nn.AddLayer(3, sigmoid);
	nn.AddLayer(4, sigmoid);

	Matrix X = Matrix(3, 1);

	X.Set(0, 0, 1.0f);
	X.Set(1, 0, 2.0f);
	X.Set(2, 0, 3.0f);

	Matrix* Y = nn.Forward(&X);

	nn.Print(-1);

	return EXIT_SUCCESS;
}
