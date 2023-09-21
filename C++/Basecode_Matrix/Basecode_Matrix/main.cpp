#include "Matrix.h"
#include "Settings.h"

int main()
{
	Matrix m1(3, 3);
	//m1.Randomize(0, 9);
	m1.Fill(2);
	m1.Print();

	Matrix m2(3, 3);
	//m2.Randomize(0, 9);
	m2.Fill(3);
	m2.Print();

	m1 += m2;
	m1.Print();

	return EXIT_SUCCESS;
}
