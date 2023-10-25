#pragma once

#include "Arc.h"
#include "Settings.h"

/// @brief Classe représentant un graphe (matrice d'adjacence).
class Graph
{
private:

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

	/// @brief Tableau qui stocke le nombre d'arcs entrants pour chaque noeud.
	int* m_negValency;

	/// @brief Tableau qui stocke le nombre d'arcs sortants pour chaque noeud.
	int* m_posValency;

	/// @brief Matrice qui stocke les poids du graphe.
	float** m_matrix;

public:

	/// @brief Crée un graphe avec 'p_size' noeuds.
	Graph(int p_size);

	~Graph();

	/// @brief Retourne le nombre de noeuds dans le graphe.
	inline int GetSize() const { return m_size; }

	/// @brief Affiche le graphe.
	void Print() const;

	/// @brief Modifie le poids d'un arc dans le graphe.
	/// @param p_u Identifiant du noeud de départ.
	/// @param p_v Identifiant du noeud d'arrivée.
	/// @param p_w Si 'w' est négatif, l'arc est supprimé, sinon il est ajouté ou modifié.
	void SetWeight(int p_u, int p_v, float p_w);

	/// @brief Retourne le poids d'un arc dans le graphe.
	float GetWeight(int p_u, int p_v) const;

	/// @brief Retourne la liste des arcs sortant du noeud 'u'.
	/// @param p_u Identifiant du noeud.
	/// @param p_size Pointeur vers un entier qui contiendra le nombre d'arcs sortant.
	/// @return Un tableau d'arcs sortant.
	Arc* GetSuccessors(int p_u, int* p_size) const;

	/// @brief Retourne la liste des arcs entrant du noeud 'v'.
	/// @param p_v Identifiant du noeud.
	/// @param p_size Pointeur vers un entier qui contiendra le nombre d'arcs entrant.
	/// @return Un tableau d'arcs entrant.
	Arc* GetPredecessors(int p_v, int* p_size) const;

	/// @brief Calcule le Plus Court Chemin (PCC) entre deux noeuds.
	/// @param p_start Identifiant du noeud de départ.
	/// @param p_end Identifiant du noeud d'arrivée.
	/// @param p_distance Pointeur vers un float qui contiendra la distance totale du chemin (négatif si le chemin n'existe pas).
	/// @return Liste d'entiers (identifiants des noeuds) correspondant au PCC.
	std::vector<int>* Dijkstra(int p_start, int p_end, float* p_distance) const;
};
