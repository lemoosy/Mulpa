#pragma once

#include "Settings.h"

/// @brief Classe repr�sentant un arc.
class Arc
{
public:

	/// @brief Identifiant du noeud de d�part de l'arc.
	int m_u;

	/// @brief Identifiant du noeud d'arriv�e de l'arc.
	int m_v;

	/// @brief Valeur du poids de l'arc.
	float m_w;

	Arc();

	Arc(int u, int v, float w = 0.0f);
};

bool operator==(const Arc& a, const Arc& b);
