#include "Graph.h"

Arc::Arc(int u, int v, float w)
{
	assert(w >= 0.0f);

	u = u;
	v = v;
	w = w;
}

GraphNode::GraphNode()
{
	m_arcs = new List<Arc>();
	m_negValency = 0;
	m_posValency = 0;
}

Graph::Graph(int size)
{
	assert(size > 0);

	m_size = size;
	m_nodes = new GraphNode[size];
}

