#pragma once

#include "Color.h"
#include "Settings.h"

/// @brief Structure repr�sentant une fen�tre.
typedef struct sWindow
{
	/// @brief Largeur de la fen�tre.
	int m_w;

	/// @brief Hauteur de la fen�tre.
	int m_h;

	/// @brief Structure qui permet de dessiner dans la fen�tre.
	SDL_Renderer* m_renderer;

	/// @brief Structure qui repr�sente la fen�tre.
	SDL_Window* m_window;
}Window;

/// @brief Cr�e une fen�tre.
/// @param p_title Titre de la fen�tre.
/// @param p_w Largeur de la fen�tre.
/// @param p_h Hauteur de la fen�tre.
/// @return La fen�tre.
Window* Window_Create(const char* p_title, int p_w, int p_h);

/// @brief D�truit une fen�tre.
void Window_Destroy(Window* window);

/// @brief Nettoie une fen�tre et la remplit d'une couleur.
void Window_Clear(Window* p_window, ColorRGBA p_color);

/// @brief Dessine une ligne dans une fen�tre.
void Window_DrawLine(Window* p_window, int p_x1, int p_y1, int p_x2, int p_y2, ColorRGBA p_color);

/// @brief Dessine un rectangle dans une fen�tre.
void Window_DrawRectangle(Window* p_window, int p_x, int p_y, int p_w, int p_h, ColorRGBA p_color, bool p_fill);

/// @brief Actualise une fen�tre (le rendu).
void Window_Refresh(Window* p_window);
