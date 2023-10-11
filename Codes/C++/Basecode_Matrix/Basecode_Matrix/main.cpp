#include "Matrix.h"

int main(void)
{
	int w = 24 * 14 * 5;
	int h = 500;

	Matrix* m1 = new Matrix(w, h);
	m1->SetValue(0, 0, 1);
	m1->SetValue(1, 0, 2);
	m1->SetValue(0, 1, 3);
	m1->SetValue(1, 1, 4);
	m1->SetValue(0, 2, 5);
	m1->SetValue(1, 2, 6);
	//m1->Print();

	Matrix* m2 = new Matrix(h, w);
	m2->SetValue(0, 0, 1);
	m2->SetValue(1, 0, 2);
	m2->SetValue(2, 0, 3);
	m2->SetValue(0, 1, 4);
	m2->SetValue(1, 1, 5);
	m2->SetValue(2, 1, 6);
	//m2->Print();

	for (int i = 0; i < 10; i++)
	{
		

		Matrix m3 = *m1 * *m2;
		//m3.Print();

		printf("WOW\n");
	}

	return EXIT_SUCCESS;
}
