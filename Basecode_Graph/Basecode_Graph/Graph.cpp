#include "Settings.h"
#include "Graph.h"

List<int>* Graph::Dijkstra(int startID, int endID)
{
	/// Liste qui donne la distance de chaque noeud par rapport au noeud de départ.

	float* distances = new float[m_size];

	for (int i = 0; i < m_size; i++)
	{
		distances[i] = +1000.0f;
	}

	distances[startID] = 0.0f;

	/// Liste qui permet d'obtenir le plus court chemin
	/// entre le noeud de départ et un noeud donné.

	int* path = new int[m_size];

	for (int i = 0; i < m_size; i++)
	{
		path[i] = -1;
	}

	/// Liste des noeuds marqués.

	bool* marked = new bool[m_size]();

	/// Algorithme de Dijkstra.
	
	while (1)
	{
		/// Recherche du noeud non marqué ayant la plus petite distance.

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

		if (u == -1) break;

		marked[u] = true;

		int size = 0;
		Arc* arcs = GetSuccessors(u, &size);

		for (int i = 0; i < size; i++)
		{
			int v = arcs[i].m_v;
			float w = arcs[i].m_w;

			if (!marked[v] && (distances[u] + w < distances[v]))
			{
				distances[v] = distances[u] + w;
				path[v] = u;
			}
		}
		
		delete arcs;
	}

	for (int i = 0; i < m_size; i++)
	{
		cout << path[i] << ", ";
	}

	cout << flush;

	return nullptr;
}
