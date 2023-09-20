#include "Graph.h"

#ifdef GRAPH_MATRIX

Graph::Graph(int size)
{
	assert(size > 0);

	m_size = size;

	m_negValency = new int[size]();
	m_posValency = new int[size]();

	m_matrix = new float*[size];

	for (int u = 0; u < size; u++)
	{
		m_matrix[u] = new float[size];

		for (int v = 0; v < size; v++)
		{
			m_matrix[u][v] = -1.0f;
		}
	}
}

Graph::~Graph()
{
	delete[] m_negValency;
	delete[] m_posValency;

	for (int u = 0; u < m_size; u++)
	{
		delete[] m_matrix[u];
	}

	delete[] m_matrix;
}

void Graph::Print() const
{
	cout << "Graph (size=" << m_size << ") :\n\n";

	for (int u = 0; u < m_size; u++)
	{
		for (int v = 0; v < m_size; v++)
		{
			cout << "[" << u << ", " << v << ", " << m_matrix[u][v] << "]\t";
		}

		cout << '\n';
	}

	cout << "\nnegValency : [";

	for (int i = 0; i < m_size; i++)
	{
		cout << m_negValency[i] << ", ";
	}

	cout << "]\nposValency : [";

	for (int i = 0; i < m_size; i++)
	{
		cout << m_posValency[i] << ", ";
	}

	cout << "]\n\n" << flush;
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
