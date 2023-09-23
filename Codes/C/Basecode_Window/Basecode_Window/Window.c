#include "Window.h"

Window* Window_Create(const char* p_title, int p_w, int p_h)
{
	assert(p_title);
	assert(p_w > 0);
	assert(p_h > 0);

	Window* window = window = (Window*)calloc(1, sizeof(Window));
	assert(window);

	window->m_w = p_w;
	window->m_h = p_h;

	window->m_window = SDL_CreateWindow(
		p_title,
		SDL_WINDOWPOS_CENTERED,
		SDL_WINDOWPOS_CENTERED,
		p_w,
		p_h,
		SDL_WINDOW_SHOWN
	);

	if (window->m_window == NULL)
	{
		printf("ERROR - Window_Create() \n");
		printf("ERROR - SDL_CreateWindow() \n");
		abort();
	}

	window->m_renderer = SDL_CreateRenderer(
		window->m_window,
		-1,
		SDL_RENDERER_SOFTWARE
	);

	if (window->m_renderer == NULL)
	{
		printf("ERROR - Window_Create() \n");
		printf("ERROR - SDL_CreateRenderer() \n");
		abort();
	}

	return window;
}

void Window_Destroy(Window* p_window)
{
	if (!p_window) return;

	SDL_DestroyRenderer(p_window->m_renderer);

	SDL_DestroyWindow(p_window->m_window);

	free(p_window);
}

void Window_Clear(Window* p_window, ColorRGBA p_color)
{
	SDL_SetRenderDrawColor(
		p_window->m_renderer,
		p_color.m_r,
		p_color.m_g,
		p_color.m_b,
		p_color.m_a
	);

	SDL_RenderClear(
		p_window->m_renderer
	);
}

void Window_DrawLine(Window* p_window, int p_x1, int p_y1, int p_x2, int p_y2, ColorRGBA p_color)
{
	SDL_SetRenderDrawColor(
		p_window->m_renderer,
		p_color.m_r,
		p_color.m_g,
		p_color.m_b,
		p_color.m_a
	);

	SDL_RenderDrawLineF(
		p_window->m_renderer,
		p_x1,
		p_y1,
		p_x2,
		p_y2
	);
}

void Window_DrawRectangle(Window* p_window, int p_x, int p_y, int p_w, int p_h, ColorRGBA p_color, bool p_fill)
{
	SDL_SetRenderDrawColor(
		p_window->m_renderer,
		p_color.m_r,
		p_color.m_g,
		p_color.m_b,
		p_color.m_a
	);

	SDL_Rect rectangle = {
		.x = p_x,
		.y = p_y,
		.w = p_w,
		.h = p_h
	};

	if (p_fill)
	{
		SDL_RenderFillRect(p_window->m_renderer, &rectangle);
	}
	else
	{
		SDL_RenderDrawRect(p_window->m_renderer, &rectangle);
	}
}

void Window_Refresh(Window* p_window)
{
	SDL_RenderPresent(p_window->m_renderer);
}
