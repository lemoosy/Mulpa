#pragma once

#include "Arc.h"
#include "Settings.h"

#ifndef GRAPH_MATRIX

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

GraphNode::GraphNode()
{
	m_arcs = new List<Arc>();

	m_negValency = 0;
	m_posValency = 0;
}

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

	~Graph();

	inline int GetSize() { return m_size; }

	/// @brief Affiche le graphe.
	void Print() const;

	void SetWeight(int u, int v, float w);

	float GetWeight(int u, int v);

	Arc* GetSuccessors(int u, int* size);

	Arc* GetPredecessors(int v, int* size);
};

Graph::Graph(int size)
{
	assert(size > 0);

	m_size = size;

	m_nodes = new GraphNode[size];
}

Graph::~Graph()
{
	delete[] m_nodes;
}

void Graph::SetWeight(int u, int v, float w)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	List<Arc>* node = m_nodes[u].m_arcs;

	Arc* arc = new Arc(u, v, w);

	/// Si on souhaite supprimer un arc.
	if (w < 0.0f)
	{
		Arc* res = node->Remove(arc);

		/// Si l'arc existe, on le supprime.
		if (res)
		{
			delete res;
		}

		delete arc;
	}

	/// Si on souhaite ajouter un arc.
	else
	{
		Arc* res = node->IsIn(arc);

		/// 
		if (res)
		{
			res->m_w = w;
		}
		else
		{
			node->InsertFirst(arc);
		}
	}
}

float Graph::GetWeight(int u, int v)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	List<Arc>* node = m_nodes[u].m_arcs;

	Arc* arc = new Arc(u, v);

	Arc* res = node->IsIn(arc);

	delete arc;

	return (res ? res->m_w : -1.0f);
}

Arc* Graph::GetSuccessors(int u, int* size)
{
	assert((0 <= u) && (u < m_size));

	int _size = m_nodes[u].m_posValency;

	Arc* arcs = new Arc[_size];
	int arcsCursor = 0;

	ListNode<Arc>* curr = m_nodes[u].m_arcs->GetFirst();

	while (curr)
	{
		arcs[arcsCursor] = curr->;
		arcsCursor++;
		curr->m_next;
	}


	*size = _size;

	return arcs;
}

Arc* Graph::GetPredecessors(int v, int* size)
{
	assert((0 <= v) && (v < m_size));

	int _size = m_nodes[v].m_negValency;

	Arc* arcs = new Arc[_size];
	int arcsCursor = 0;




	*size = _size;

	return arcs;
}

#endif // GRAPH_MATRIX
