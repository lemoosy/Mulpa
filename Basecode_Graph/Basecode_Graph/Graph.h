#pragma once

#include "List.h"
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

	Arc(int u, int v, float w);
};

/// @brief Classe repr�sentant un noeud dans un graphe
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

/// @brief Classe repr�sentant un graphe.
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
