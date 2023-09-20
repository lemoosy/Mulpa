#pragma once

#include "Arc.h"
#include "List.h"
#include "Settings.h"

#ifdef GRAPH_MATRIX

/// @brief Classe m�re pour la classe Graph (matrice d'adjacence).
class GraphMother
{
public:

	/// @brief Tableau qui stocke le nombre d'arcs entrants pour chaque noeud.
	int* m_negValency;

	/// @brief Tableau qui stocke le nombre d'arcs sortants pour chaque noeud.
	int* m_posValency;

	/// @brief Matrice qui stocke les poids du graphe.
	float** m_matrix;
};

#else

/// @brief Classe repr�sentant un noeud dans un graphe (liste d'adjacence).
class GraphNode
{
public:

	/// @brief Identifiant du noeud.
	int m_id;

	/// @brief Liste des arcs sortant du noeud.
	List<Arc>* m_arcs;

	/// @brief Nombre de noeuds entrant dans le noeud.
	int m_negValency;

	/// @brief Nombre de noeuds sortant dans le noeud.
	int m_posValency;

	GraphNode();

	~GraphNode();
};

/// @brief Classe m�re pour la classe Graph (liste d'adjacence).
class GraphMother
{
public:

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

	/// @brief Noeuds du graphe.
	GraphNode* m_nodes;
};

#endif // GRAPH_MATRIX

/// @brief Classe repr�sentant un graphe.
class Graph : public GraphMother
{
private:

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

public:

	Graph(int size);

	~Graph();

	/// @brief Retourne le nombre de noeuds dans le graphe.
	inline int GetSize() const { return m_size; }

	/// @brief Affiche le graphe.
	void Print() const;

	/// @brief Modifie le poids d'un arc dans le graphe.
	/// @param u Identifiant du noeud de d�part.
	/// @param v Identifiant du noeud d'arriv�e.
	/// @param w Si w est n�gatif, l'arc est supprim�, sinon il est cr�� ou modifi�.
	void SetWeight(int u, int v, float w);

	/// @brief Retourne le poids d'un arc dans le graphe.
	float GetWeight(int u, int v);

	/// @brief Retourne la liste des arcs sortants d'un noeud u.
	/// @param u Identifiant du noeud.
	/// @param size Pointeur vers un entier qui contiendra le nombre d'arcs sortants.
	/// @return Un tableau d'arcs sortants.
	Arc* GetSuccessors(int u, int* size);

	/// @brief Retourne la liste des arcs entrants d'un noeud v.
	/// @param v Identifiant du noeud.
	/// @param size Pointeur vers un entier qui contiendra le nombre d'arcs entrants.
	/// @return Un tableau d'arcs entrants.
	Arc* GetPredecessors(int v, int* size);
};

List<int>* Dijkstra(Graph& graph, int u, int v);
