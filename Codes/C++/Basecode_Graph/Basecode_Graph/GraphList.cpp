#include "Graph.h"

#ifndef GRAPH_MATRIX

GraphNode::GraphNode()
{
	m_id = -1;

	m_arcs = new DList<Arc>();

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

void Graph::Print() const
{
	cout << "Graph (size=" << m_size << ") :\n\n";

	for (int u = 0; u < m_size; u++)
	{
		cout << "(" << u << ") >> ";
		m_nodes[u].m_arcs->Print();
	}

	cout << endl;
}

void Graph::SetWeight(int u, int v, float w)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	GraphNode* node_u = &(m_nodes[u]);
	GraphNode* node_v = &(m_nodes[v]);
	
	DList<Arc>* arcs = node_u->m_arcs;

	Arc* arc = new Arc(u, v, w);

	/// Si on souhaite supprimer l'arc.
	if (w < 0.0f)
	{
		Arc* res = arcs->Remove(arc);

		/// Si l'arc existe, on le supprime et on met à jour les valences.
		if (res)
		{
			delete res;

			node_v->m_negValency--;
			node_u->m_posValency--;
		}

		delete arc;
	}

	/// Si on souhaite ajouter un arc.
	else
	{
		Arc* res = arcs->IsIn(arc);

		/// Si l'arc existe, on met à jour le poids de l'arc.
		if (res)
		{
			delete arc;

			res->m_w = w;
		}

		/// Si l'arc n'existe pas, on insère l'arc et on met à jour les valences.
		else
		{
			arcs->InsertLast(arc);

			node_v->m_negValency++;
			node_u->m_posValency++;
		}
	}
}

float Graph::GetWeight(int u, int v)
{
	assert((0 <= u) && (u < m_size));
	assert((0 <= v) && (v < m_size));

	DList<Arc>* arcs = m_nodes[u].m_arcs;

	Arc* arc = new Arc(u, v, 0.0f);

	Arc* res = arcs->IsIn(arc);

	delete arc;

	return (res ? res->m_w : -1.0f);
}

Arc* Graph::GetSuccessors(int u, int* size)
{
	assert((0 <= u) && (u < m_size));
	assert(size);

	int _size = m_nodes[u].m_posValency;

	if (_size == 0)
	{
		*size = 0;
		return nullptr;
	}

	Arc* arcs = new Arc[_size];
	int arcsCursor = 0;

	DListNode<Arc>* sent = m_nodes[u].m_arcs->GetSentinel();
	DListNode<Arc>* curr = sent->m_next;

	while (curr != sent)
	{
		arcs[arcsCursor] = *(curr->m_value);
		arcsCursor++;

		curr = curr->m_next;
	}

	*size = _size;

	return arcs;
}

Arc* Graph::GetPredecessors(int v, int* size)
{
	assert((0 <= v) && (v < m_size));

	int _size = m_nodes[v].m_negValency;

	if (_size == 0)
	{
		*size = 0;
		return nullptr;
	}

	Arc* arcs = new Arc[_size];
	int arcsCursor = 0;

	Arc* arcSearch = new Arc(0, v, 0.0f);

	for (int u = 0; u < m_size; u++)
	{
		Arc* res = m_nodes[u].m_arcs->IsIn(arcSearch);

		if (res)
		{
			arcs[arcsCursor] = *res;
			arcsCursor++;
		}
	}

	delete arcSearch;

	*size = _size;

	return arcs;
}

#endif // GRAPH_MATRIX
