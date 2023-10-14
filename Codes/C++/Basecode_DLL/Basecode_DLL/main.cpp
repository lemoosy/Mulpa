#include <iostream>
#include <time.h>

#include "DLL.h"

int main()
{


	DLL_PG_Init(1000, 10, 5, 3);

	int* tab = new int[24 * 14]();

		clock_t start = clock();
	for (int i = 0; i < 1000; i++)
	{
		DLL_PG_Forward(i, tab, 24, 14);
		printf("%d \n", i);
	}
		clock_t end = clock();
		cout << (end - start) / CLOCKS_PER_SEC << endl;

	delete tab;

	DLL_PG_Quit();

	system("pause");

	return 0;
}