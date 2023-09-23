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

	Arc(int u, int v, float w = -1.0f);

	/// @brief Affiche les informations de l'arc.
	void Print() const;
};

/// @brief V�rifie � l'aide de la commande == si deux arcs sont �gaux (si les noeuds d'arriv�e sont �gaux).
bool operator==(const Arc& a, const Arc& b);

/// @brief Affiche les informations de l'arc � l'aide de la commande <<.
ostream& operator<<(ostream& stream, Arc& arc);
