#pragma once

#include "Color.h"
#include "Settings.h"

/// @brief Structure représentant une fenêtre.
typedef struct sWindow
{
	/// @brief Largeur de la fenêtre.
	int m_w;

	/// @brief Hauteur de la fenêtre.
	int m_h;

	/// @brief Structure qui permet de dessiner dans la fenêtre.
	SDL_Renderer* m_renderer;

	/// @brief Structure qui représente la fenêtre.
	SDL_Window* m_window;
}Window;

/// @brief Crée une fenêtre.
/// @param p_title Titre de la fenêtre.
/// @param p_w Largeur de la fenêtre.
/// @param p_h Hauteur de la fenêtre.
/// @return La fenêtre.
Window* Window_Create(const char* p_title, int p_w, int p_h);

/// @brief Détruit une fenêtre.
void Window_Destroy(Window* window);

/// @brief Nettoie une fenêtre et la remplit d'une couleur.
void Window_Clear(Window* p_window, ColorRGBA p_color);

/// @brief Dessine une ligne dans une fenêtre.
void Window_DrawLine(Window* p_window, int p_x1, int p_y1, int p_x2, int p_y2, ColorRGBA p_color);

/// @brief Dessine un rectangle dans une fenêtre.
void Window_DrawRectangle(Window* p_window, int p_x, int p_y, int p_w, int p_h, ColorRGBA p_color, bool p_fill);

/// @brief Actualise une fenêtre (le rendu).
void Window_Refresh(Window* p_window);
