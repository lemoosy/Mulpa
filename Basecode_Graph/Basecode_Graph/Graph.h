#pragma once

#include "List.h"
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

	Arc(int u, int v, float w);
};

/// @brief Classe représentant un noeud dans un graphe
class GraphNode
{
public:

	/// @brief Liste des arcs sortants du noeud.
	List<Arc>* m_arcs;

	/// @brief Nombre de noeuds entrants dans le noeud.
	int m_negValency;

	/// @brief Nombre de noeuds sortants du noeud.
	int m_posValency;

	GraphNode();
};

/// @brief Classe représentant un graphe.
class Graph
{
private:

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

	/// @brief Noeuds du graphe.
	GraphNode* m_nodes;

public:

	Graph(int size);
};
