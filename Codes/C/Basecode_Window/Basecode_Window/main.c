#include "Window.h"

int main(void)
{
	MySDL_Init();

	Window* window = Window_Create("TEST", 800, 800);

	Window_DrawRectangle(
		window,
		100, 100,
		100, 100, 
		ColorRGBA_Create(100, 100, 100, 255),
		true
	);

	Window_Refresh(window);

	system("pause");

	Window_Destroy(window);

	MySDL_Quit();

	return EXIT_SUCCESS;
}
