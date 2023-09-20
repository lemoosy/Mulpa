#include "Graph.h"

#ifndef GRAPH_MATRIX

GraphNode::GraphNode()
{
	m_id = -1;

	m_arcs = new List<Arc>();

	m_negValency = 0;
	m_posValency = 0;
}

GraphNode::~GraphNode()
{
	delete m_arcs;
}

Graph::Graph(int size)
{
	assert(size > 0);

	m_size = size;

	m_nodes = new GraphNode[size]();

	for (int u = 0; u < size; u++)
	{
		m_nodes[u].m_id = u;
	}
}

Graph::~Graph()
{
	delete[] m_nodes;
}

void Graph::SetWeight(int u, int v, float w)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	GraphNode* node_u = &(m_nodes[u]);
	GraphNode* node_v = &(m_nodes[v]);
	
	List<Arc>* arcs = node_u->m_arcs;

	Arc* arc = new Arc(u, v, w);

	/// Si on souhaite supprimer l'arc.
	if (w < 0.0f)
	{
		Arc* res = arcs->Remove(arc);

		/// Si l'arc existe, on le supprime et on met à jour les valences.
		if (res)
		{
			delete res;

			node_u->m_posValency--;
			node_v->m_negValency--;
		}

		delete arc;
	}

	/// Si on souhaite ajouter un arc.
	else
	{
		Arc* res = arcs->IsIn(arc);

		/// Si l'arc existe, on le modifie.
		if (res)
		{
			delete arc;

			res->m_w = w;
		}

		/// Si l'arc n'existe pas, on crée un arc et on met à jour les valences.
		else
		{
			arcs->InsertFirst(arc);

			node_u->m_posValency++;
			node_v->m_negValency++;
		}
	}
}

float Graph::GetWeight(int u, int v)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	List<Arc>* arcs = m_nodes[u].m_arcs;

	Arc* arc = new Arc(u, v);

	Arc* res = arcs->IsIn(arc);

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
		arcs[arcsCursor] = *(curr->m_value);
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

	for (int u = 0; u < m_size; u++)
	{
		if (u == v) continue;

		ListNode<Arc>* curr = m_nodes[u].m_arcs->GetFirst();

		while (curr)
		{
			Arc* arc = curr->m_value;

			if (arc->m_v == v)
			{
				arcs[arcsCursor] = *arc;
				arcsCursor++;
				break;
			}
			
			curr->m_next;
		}
	}

	*size = _size;

	return arcs;
}

#endif // GRAPH_MATRIX
