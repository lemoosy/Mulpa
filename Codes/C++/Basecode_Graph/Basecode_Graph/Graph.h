#pragma once

#include "Arc.h"
#include "DList.h"
#include "Settings.h"

#ifdef GRAPH_MATRIX

/// @brief Classe mère pour la classe Graph (matrice d'adjacence).
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

/// @brief Classe représentant un noeud dans un graphe (liste d'adjacence).
class GraphNode
{
public:

	/// @brief Identifiant du noeud.
	int m_id;

	/// @brief Liste des arcs sortant du noeud.
	DList<Arc>* m_arcs;

	/// @brief Nombre de noeuds entrant dans le noeud.
	int m_negValency;

	/// @brief Nombre de noeuds sortant dans le noeud.
	int m_posValency;

	GraphNode();

	~GraphNode();
};

/// @brief Classe mère pour la classe Graph (liste d'adjacence).
class GraphMother
{
public:

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

	/// @brief Noeuds du graphe.
	GraphNode* m_nodes;
};

#endif // GRAPH_MATRIX

/// @brief Classe représentant un graphe.
class Graph : private GraphMother
{
private:

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

public:

	/// @brief Crée un graphe avec 'size' noeuds.
	Graph(int size);

	~Graph();

	/// @brief Retourne le nombre de noeuds dans le graphe.
	inline int GetSize() const { return m_size; }

	/// @brief Affiche le graphe.
	void Print() const;

	/// @brief Modifie le poids d'un arc dans le graphe.
	/// @param u Identifiant du noeud de départ.
	/// @param v Identifiant du noeud d'arrivée.
	/// @param w Si 'w' est négatif, l'arc est supprimé, sinon il est créé ou modifié.
	void SetWeight(int u, int v, float w);

	/// @brief Retourne le poids d'un arc dans le graphe.
	float GetWeight(int u, int v);

	/// @brief Retourne la liste des arcs sortant du noeud 'u'.
	/// @param u Identifiant du noeud.
	/// @param size Pointeur vers un entier qui contiendra le nombre d'arcs sortant.
	/// @return Un tableau d'arcs sortant.
	Arc* GetSuccessors(int u, int* size);

	/// @brief Retourne la liste des arcs entrant du noeud 'v'.
	/// @param v Identifiant du noeud.
	/// @param size Pointeur vers un entier qui contiendra le nombre d'arcs entrant.
	/// @return Un tableau d'arcs entrant.
	Arc* GetPredecessors(int v, int* size);

	/// @brief Calcule le Plus Court Chemin (PCC) entre deux noeuds.
	/// @param start Identifiant du noeud de départ.
	/// @param end Identifiant du noeud d'arrivée.
	/// @param distance Pointeur vers un float qui contiendra la distance totale du chemin (négatif si le chemin n'existe pas).
	/// @return Liste d'entiers (identifiants des noeuds) correspondant au PCC.
	DList<int>* Dijkstra(int start, int end, float* distance);
};
