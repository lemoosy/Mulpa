//#include "Arc.h"
//#include "Settings.h"
//
//#ifndef GRAPH_MATRIX
//
//GraphNode::GraphNode()
//{
//	m_id = -1;
//
//	m_arcs = new List<Arc>();
//
//	m_negValency = 0;
//	m_posValency = 0;
//}
//
////Graph::Graph(int size)
////{
////	assert(size > 0);
////
////	m_size = size;
////
////	m_nodes = new GraphNode[size];
////}
//
//Graph::~Graph()
//{
//	delete[] m_nodes;
//}
//
//void Graph::SetWeight(int u, int v, float w)
//{
//	assert((0 <= u) && (u < m_size));
//	assert((0 <= v) && (v < m_size));
//
//	List<Arc>* node = m_nodes[u].m_arcs;
//
//	Arc* arc = new Arc(u, v, w);
//
//	/// Si on souhaite supprimer un arc.
//	if (w < 0.0f)
//	{
//		Arc* res = node->Remove(arc);
//
//		/// Si l'arc existe, on le supprime.
//		if (res)
//		{
//			delete res;
//		}
//
//		delete arc;
//	}
//
//	/// Si on souhaite ajouter un arc.
//	else
//	{
//		Arc* res = node->IsIn(arc);
//
//		/// 
//		if (res)
//		{
//			res->m_w = w;
//		}
//		else
//		{
//			node->InsertFirst(arc);
//		}
//	}
//}
//
//float Graph::GetWeight(int u, int v)
//{
//	assert((0 <= u) && (u < m_size));
//	assert((0 <= v) && (v < m_size));
//
//	List<Arc>* node = m_nodes[u].m_arcs;
//
//	Arc* arc = new Arc(u, v);
//
//	Arc* res = node->IsIn(arc);
//
//	delete arc;
//
//	return (res ? res->m_w : -1.0f);
//}
//
//Arc* Graph::GetSuccessors(int u, int* size)
//{
//	assert((0 <= u) && (u < m_size));
//
//	int _size = m_nodes[u].m_posValency;
//
//	Arc* arcs = new Arc[_size];
//	int arcsCursor = 0;
//
//	ListNode<Arc>* curr = m_nodes[u].m_arcs->GetFirst();
//
//	while (curr)
//	{
//		arcs[arcsCursor] = *(curr->m_value);
//		arcsCursor++;
//		curr->m_next;
//	}
//
//	*size = _size;
//
//	return arcs;
//}
//
//Arc* Graph::GetPredecessors(int v, int* size)
//{
//	assert((0 <= v) && (v < m_size));
//
//	int _size = m_nodes[v].m_negValency;
//
//	Arc* arcs = new Arc[_size];
//	int arcsCursor = 0;
//
//	for (int u = 0; u < m_size; u++)
//	{
//		ListNode<Arc>* curr = m_nodes[u].m_arcs->GetFirst();
//
//		while (curr)
//		{
//			Arc* arc = curr->m_value;
//
//			if (arc->m_v == v)
//			{
//				arcs[arcsCursor] = *arc;
//				arcsCursor++;
//			}
//			
//			curr->m_next;
//		}
//	}
//
//	*size = _size;
//
//	return arcs;
//}
//
//#endif // GRAPH_MATRIX
