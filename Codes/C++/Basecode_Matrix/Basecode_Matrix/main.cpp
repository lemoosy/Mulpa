#include "Matrix.h"
#include "Settings.h"

int main()
{
	Matrix m1(2, 3);
	m1.Set(0, 0, 2);
	m1.Set(1, 0, 3);
	m1.Set(0, 1, 5);
	m1.Set(1, 1, 4);
	m1.Set(0, 2, 6);
	m1.Set(1, 2, 7);
	m1.Print();

	Matrix m2(4, 2);
	m2.Set(0, 0, 1);
	m2.Set(1, 0, 2);
	m2.Set(2, 0, 0);
	m2.Set(3, 0, 0);
	m2.Set(0, 1, 1);
	m2.Set(1, 1, 4);
	m2.Set(2, 1, 5);
	m2.Set(3, 1, 1);
	m2.Print();

	Matrix m3 = m1 * m2;
	m3.Print();

	Matrix* m4 = new Matrix(5, 5);
	m4->Randomize(0, 9);
	m4->Print();
	delete m4;

	return EXIT_SUCCESS;
}
