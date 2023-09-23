#include "Settings.h"

void MySDL_Init(void)
{
	int res = SDL_Init(SDL_INIT_VIDEO);

	if (res == 0) return;

	printf("ERROR - MySDL_Init() \n");
	printf("ERROR - SDL_Init() \n");
	abort();
}

void MySDL_Quit(void)
{
	SDL_Quit();
}
