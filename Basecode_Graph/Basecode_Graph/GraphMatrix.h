#pragma once

#include "Arc.h"
#include "Settings.h"

#ifdef GRAPH_MATRIX

/// @brief Classe représentant un graphe.
class Graph
{
private:

	/// @brief Matrice qui stocke les poids du graphe.
	float** m_matrix;

	/// @brief Nombre de noeuds dans le graphe.
	int m_size;

	/// @brief Tableau qui stocke le nombre d'arcs entrants pour chaque noeud.
	int* m_negValency;

	/// @brief Tableau qui stocke le nombre d'arcs sortants pour chaque noeud.
	int* m_posValency;

public:

	Graph(int size);

	~Graph();

	/// @brief Retourne le nombre de noeuds dans le graphe.
	inline int GetSize() const { return m_size; }

	/// @brief Affiche le graphe.
	void Print() const;

	/// @brief Modifie le poids d'un arc.
	/// @param u Identifiant du noeud de départ.
	/// @param v Identifiant du noeud d'arrivée.
	/// @param w Si w est négatif, l'arc est supprimé, sinon il est créé ou modifié.
	void SetWeight(int u, int v, float w);

	float GetWeight(int u, int v);

	Arc* GetSuccessors(int u, int* size);

	Arc* GetPredecessors(int v, int* size);
};

Graph::Graph(int size)
{
	assert(size > 0);

	m_matrix = new float* [size];

	for (int u = 0; u < size; u++)
	{
		m_matrix[u] = new float[size];

		for (int v = 0; v < size; v++)
		{
			m_matrix[u][v] = -1.0f;
		}
	}

	m_size = size;

	m_negValency = new int[size];
	m_posValency = new int[size];
}

Graph::~Graph()
{
	delete[] m_posValency;
	delete[] m_negValency;

	for (int i = 0; i < m_size; i++)
	{
		delete[] m_matrix[i];
	}

	delete[] m_matrix;
}

void Graph::Print() const
{
	cout << "Graph (size=" << m_size << ") :\n" << endl;

	for (int u = 0; u < m_size; u++)
	{
		for (int v = 0; v < m_size; v++)
		{
			cout << "[" << u << ", " << v << ", " << m_matrix[u][v] << "]\t";
		}

		cout << endl;
	}

	cout << endl;
}

void Graph::SetWeight(int u, int v, float w)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	/// Si on souhaite supprimer un arc.
	if (w < 0.0f)
	{
		/// Si l'arc existe, on le supprime.
		if (m_matrix[u][v] >= 0.0f)
		{
			m_matrix[u][v] = -1.0f;
			m_negValency[v]--;
			m_posValency[u]--;
		}
	}

	/// Si on souhaite ajouter un arc.
	else
	{
		/// Si l'arc n'existe pas, on met a jour les valences.
		if (m_matrix[u][v] < 0.0f)
		{
			m_negValency[v]++;
			m_posValency[u]++;
		}

		/// Dans tous les cas, on met a jour le poids de l'arc.
		m_matrix[u][v] = w;
	}
}

float Graph::GetWeight(int u, int v)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	return m_matrix[u][v];
}

Arc* Graph::GetSuccessors(int u, int* size)
{
	assert((0 <= u) && (u < m_size));
	assert(size);

	int _size = m_posValency[u];

	Arc* arcs = new Arc[_size];
	int arcsCursor = 0;

	for (int v = 0; v < _size; v++)
	{
		float w = m_matrix[u][v];

		if (w >= 0.0f)
		{
			arcs[arcsCursor] = Arc(u, v, w);
			arcsCursor++;
		}
	}

	*size = _size;

	return arcs;
}

Arc* Graph::GetPredecessors(int v, int* size)
{
	assert((0 <= v) && (v < m_size));
	assert(size);

	int _size = m_negValency[v];

	Arc* arcs = new Arc[_size];
	int arcsCursor = 0;

	for (int u = 0; u < _size; u++)
	{
		float w = m_matrix[u][v];

		if (w >= 0.0f)
		{
			arcs[arcsCursor] = Arc(u, v, w);
			arcsCursor++;
		}
	}

	*size = _size;

	return arcs;
}

#endif // GRAPH_MATRIX
