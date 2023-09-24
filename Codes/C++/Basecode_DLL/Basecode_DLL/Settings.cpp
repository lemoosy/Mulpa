#include "Settings.h"

GameMakerDLL double DLL_Init()
{
	srand(time(nullptr));

	//g_nn = new NN*[NN_CAPACITY]();

	/*if (WINDOW_DISPLAY)
	{
		g_window = Window_Create("Basecode_DLL - 2023", WINDOW_WIDTH, WINDOW_HEIGHT);
	}*/

	return 0.0;
}

GameMakerDLL double DLL_Free()
{
	//for (int i = 0; i < NN_CAPACITY; i++)
	//{
	//	if (g_nn[i])
	//	{
	//		delete g_nn[i];
	//	}
	//}

	//delete[] g_nn;

	return 0.0;
}
