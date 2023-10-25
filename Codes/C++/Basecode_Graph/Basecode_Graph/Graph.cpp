#include "Graph.h"

Graph::Graph(int p_size)
{
	assert(p_size > 0);

	m_size = p_size;

	m_negValency = new int[p_size]();
	m_posValency = new int[p_size]();

	m_matrix = new float*[p_size];

	for (int u = 0; u < p_size; u++)
	{
		m_matrix[u] = new float[p_size];

		for (int v = 0; v < p_size; v++)
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
	std::cout << "Graph (size=" << m_size << ") :\n\n";

	for (int u = 0; u < m_size; u++)
	{
		for (int v = 0; v < m_size; v++)
		{
			std::cout << std::fixed << std::setprecision(2) << m_matrix[u][v] << "\t";
		}

		std::cout << '\n';
	}

	std::cout << "\nnegValency : [";

	for (int i = 0; i < m_size; i++)
	{
		std::cout << m_negValency[i] << ", ";
	}

	std::cout << "]\nposValency : [";

	for (int i = 0; i < m_size; i++)
	{
		std::cout << m_posValency[i] << ", ";
	}

	std::cout << "]\n\n" << std::flush;
}

void Graph::SetWeight(int p_u, int p_v, float p_w)
{
	assert((0 <= p_u) && (p_u < m_size));
	assert((0 <= p_v) && (p_v < m_size));

	/// Si on souhaite supprimer l'arc.
	if (p_w < 0.0f)
	{
		/// Si l'arc existe, on le supprime et on met à jour les valences.
		if (m_matrix[p_u][p_v] >= 0.0f)
		{
			m_matrix[p_u][p_v] = -1.0f;
			m_negValency[p_v]--;
			m_posValency[p_u]--;
		}
	}

	/// Si on souhaite ajouter un arc.
	else
	{
		/// Si l'arc n'existe pas, on crée un arc et on met à jour les valences.
		if (m_matrix[p_u][p_v] < 0.0f)
		{
			m_negValency[p_v]++;
			m_posValency[p_u]++;
		}

		/// Dans tous les cas, on met à jour le poids de l'arc.
		m_matrix[p_u][p_v] = p_w;
	}
}

float Graph::GetWeight(int p_u, int p_v) const
{
	assert((0 <= p_u) && (p_u < m_size));
	assert((0 <= p_v) && (p_v < m_size));

	return m_matrix[p_u][p_v];
}

Arc* Graph::GetSuccessors(int p_u, int* p_size) const
{
	assert((0 <= p_u) && (p_u < m_size));
	assert(p_size);

	int size = m_posValency[p_u];

	if (size == 0)
	{
		*p_size = 0;
		return nullptr;
	}

	Arc* arcs = new Arc[size]();
	int arcsCursor = 0;

	for (int v = 0; v < m_size; v++)
	{
		float w = m_matrix[p_u][v];

		if (w >= 0.0f)
		{
			arcs[arcsCursor] = Arc(p_u, v, w);
			arcsCursor++;
		}
	}

	*p_size = size;

	return arcs;
}

Arc* Graph::GetPredecessors(int p_v, int* p_size) const
{
	assert((0 <= p_v) && (p_v < m_size));
	assert(p_size);

	int size = m_negValency[p_v];

	Arc* arcs = new Arc[size]();
	int arcsCursor = 0;

	for (int u = 0; u < m_size; u++)
	{
		float w = m_matrix[u][p_v];

		if (w >= 0.0f)
		{
			arcs[arcsCursor] = Arc(u, p_v, w);
			arcsCursor++;
		}
	}

	*p_size = size;

	return arcs;
}

std::vector<int>* Graph::Dijkstra(int p_start, int p_end, float* p_distance) const
{
	assert((0 <= p_start) && (p_start < m_size));
	assert((0 <= p_end) && (p_end < m_size));
	assert(p_distance);

	/// Liste qui stocke les distances entre
	/// le noeud de départ et un noeud donné.

	float* distances = new float[m_size];

	for (int i = 0; i < m_size; i++)
	{
		distances[i] = +1000.0f;
	}

	distances[p_start] = 0.0f;

	/// Liste qui stocke les Plus Court Chemin entre
	/// le noeud de départ et un noeud donné (méthode successor).
	/// Toutes les valeurs sont des identifiants de noeuds.

	int* PCC = new int[m_size];

	for (int i = 0; i < m_size; i++)
	{
		PCC[i] = -1;
	}

	/// Liste des noeuds marqués.

	bool* marked = new bool[m_size]();

	/// Algorithme de Dijkstra.
	
	while (1)
	{
		/// Recherche du noeud non marqué ET
		/// ayant la plus petite distance.

		int indexMinimum = -1;

		for (int i = 0; i < m_size; i++)
		{
			if (!marked[i])
			{
				if ((indexMinimum == -1) || (distances[i] < distances[indexMinimum]))
				{
					indexMinimum = i;
				}
			}
		}

		int u = indexMinimum;

		/// Si tout est marqué OU
		/// les autres noeuds sont inaccessibles, on quitte la boucle.

		if ((u == -1) || (distances[u] == 1000.0f))
		{
			break;
		}

		/// On marque le noeud u.

		marked[u] = true;

		/// On récupère les successeurs du noeud u
		/// puis on actualise les distances et les PCC.

		int size = 0;
		Arc* arcs = GetSuccessors(u, &size);

		for (int i = 0; i < size; i++)
		{
			int v = arcs[i].m_v;
			float w = arcs[i].m_w;

			if (!marked[v] && (distances[u] + w < distances[v]))
			{
				PCC[v] = u;
				distances[v] = distances[u] + w;
			}
		}
		
		if (arcs)
		{
			delete arcs;
		}
	}

	/// Création du chemin à partir des PCC.

	std::vector<int>* res = nullptr;
	
	if (PCC[p_end] != -1)
	{
		res = new std::vector<int>();

		int index = p_end;

		while (index != -1)
		{
			res->insert(res->begin(), index);
			index = PCC[index];
		}
	}

	/// Récupération de la distance.

	*p_distance = -1.0f;

	if (res)
	{
		*p_distance = distances[p_end];
	}

	/// Libération de la mémoire.

	delete distances;
	delete PCC;
	delete marked;

	return res;
}
