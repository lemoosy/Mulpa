#pragma once

#include "Settings.h"

/// @brief Structure représentant une couleur sour le format RGBA.
typedef struct ColorRGBA
{
	/// @brief Composante rouge.
	Uint8 m_r;

	/// @brief Composante verte.
	Uint8 m_g;
	
	/// @brief Composante bleue.
	Uint8 m_b;
	
	/// @brief Composante alpha.
	Uint8 m_a;

}ColorRGBA;

/// @brief Crée une couleur.
/// @param p_r Composante rouge.
/// @param p_g Composante verte.
/// @param p_b Composante bleue.
/// @param p_a Composante alpha.
/// @return La couleur.
ColorRGBA ColorRGBA_Create(int p_r, int p_g, int p_b, int p_a);
