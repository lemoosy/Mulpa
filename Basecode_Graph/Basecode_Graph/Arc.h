#pragma once

#include "Settings.h"

/// @brief Classe représentant un arc.
class Arc
{
public:

	/// @brief Identifiant du noeud de départ de l'arc.
	int m_u;

	/// @brief Identifiant du noeud d'arrivée de l'arc.
	int m_v;

	/// @brief Valeur du poids de l'arc.
	float m_w;

	Arc();

	Arc(int u, int v, float w = -1.0f);

	/// @brief Affiche les informations de l'arc.
	void Print() const;
};

/// @brief Vérifie à l'aide de la commande == si deux arcs sont égaux (si les noeuds d'arrivée sont égaux).
bool operator==(const Arc& a, const Arc& b);

/// @brief Affiche les informations de l'arc à l'aide de la commande <<.
ostream& operator<<(ostream& stream, Arc& arc);
