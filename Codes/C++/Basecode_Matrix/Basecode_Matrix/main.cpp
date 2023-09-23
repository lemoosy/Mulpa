#include "Matrix.h"

int main(void)
{
	Matrix m1(2, 3);
	m1.SetValue(0, 0, 1);
	m1.SetValue(1, 0, 2);
	m1.SetValue(0, 1, 3);
	m1.SetValue(1, 1, 4);
	m1.SetValue(0, 2, 5);
	m1.SetValue(1, 2, 6);
	m1.Print();

	Matrix m2(4, 2);
	m2.SetValue(0, 0, 1);
	m2.SetValue(1, 0, 2);
	m2.SetValue(2, 0, 0);
	m2.SetValue(3, 0, 0);
	m2.SetValue(0, 1, 1);
	m2.SetValue(1, 1, 4);
	m2.SetValue(2, 1, 5);
	m2.SetValue(3, 1, 1);
	m2.Print();

	Matrix m3 = m1 * m2;
	m3.Print();

	return EXIT_SUCCESS;
}
