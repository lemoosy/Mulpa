#include "Graph.h"

DList<int>* Graph::Dijkstra(int start, int end, float* distance)
{
	assert(start != end);
	assert((0 <= start) && (start < m_size));
	assert((0 <= end) && (end < m_size));
	assert(distance);

	/// Liste qui stocke les distances entre
	/// le noeud de d�part et un noeud donn�.

	float* distances = new float[m_size];

	for (int i = 0; i < m_size; i++)
	{
		distances[i] = +1000.0f;
	}

	distances[start] = 0.0f;

	/// Liste qui stocke les Plus Court Chemin entre
	/// le noeud de d�part et un noeud donn� (m�thode successor).
	/// Toutes les valeurs sont des identifiants de noeuds.

	int* PCC = new int[m_size];

	for (int i = 0; i < m_size; i++)
	{
		PCC[i] = -1;
	}

	/// Liste des noeuds marqu�s.

	bool* marked = new bool[m_size]();

	/// Algorithme de Dijkstra.
	
	while (1)
	{
		/// Recherche du noeud non marqu� ET
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

		/// Si tout est marqu� OU
		/// les autres noeuds sont inaccessibles, on quitte la boucle.

		if ((u == -1) || (distances[u] == 1000.0f))
		{
			break;
		}

		/// On marque le noeud u.

		marked[u] = true;

		/// On r�cup�re les successeurs du noeud u
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

	/// Cr�ation du chemin � partir des PCC.

	DList<int>* res = nullptr;
	
	if (PCC[end] != -1)
	{
		res = new DList<int>();

		int index = end;

		while (index != -1)
		{
			res->InsertFirst(new int(index));
			index = PCC[index];
		}
	}

	/// R�cup�ration de la distance.

	*distance = -1.0f;

	if (res)
	{
		*distance = distances[end];
	}

	/// Lib�ration de la m�moire.

	delete distances;
	delete PCC;
	delete marked;

	return res;
}
