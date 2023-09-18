#pragma once

#include "Settings.h"

/// @brief Classe repr�sentant un noeud dans une liste simplement cha�n�e.
template <typename ListData>
class ListNode
{
public:

	/// @brief Pointeur vers la valeur du noeud.
	ListData* m_value;

	/// @brief Pointeur vers le noeud suivant.
	ListNode<ListData>* m_next;

	ListNode(ListData* value, ListNode* next = nullptr);
};

template <typename ListData>
ListNode<ListData>::ListNode(ListData* value, ListNode* next)
{
	m_value = value;
	m_next = next;
}

/// @brief Classe repr�sentant une liste simplement cha�n�e.
template <typename ListData>
class List
{
private:

	/// @brief Nombre de noeuds dans la liste.
	int m_size;

	/// @brief Pointeur vers le premier noeud de la liste.
	ListNode<ListData>* m_first;

public:

	/// @brief Retourne la taille de la liste.
	inline int GetSize() const { return m_size; }

	List();

	~List();

	/// @brief Ins�re une valeur au d�but de la liste.
	void InsertFirst(ListData* value);

	///@brief Retire et retourne la premi�re valeur de la liste.
	ListData* PopFirst();

	/// @brief V�rifie si une valeur est dans la liste.
	bool IsIn(ListData* value) const;

	/// @brief Affiche la liste.
	void Print() const;
};

template<typename ListData>
List<ListData>::List()
{
	m_size = 0;
	m_first = nullptr;
}

template<typename ListData>
List<ListData>::~List()
{
	ListNode<ListData>* curr = m_first;

	while (curr)
	{
		ListNode<ListData>* next = curr->m_next;

		delete curr->m_value;
		delete curr;

		curr = next;
	}
}

template<typename ListData>
void List<ListData>::InsertFirst(ListData* value)
{
	ListNode<ListData>* nodeInsert = new ListNode<ListData>(value, m_first);

	m_first = nodeInsert;

	m_size++;
}

template<typename ListData>
ListData* List<ListData>::PopFirst()
{
	assert(m_size > 0);

	ListNode<ListData>* nodePop = m_first;

	m_first = m_first->m_next;

	ListData* valuePop = nodePop->m_value;

	delete nodePop;

	m_size--;

	return valuePop;
}

template<typename ListData>
bool List<ListData>::IsIn(ListData* value) const
{
	ListNode<ListData>* curr = m_first;

	while (curr)
	{
		if (*value == *(curr->m_value))
		{
			return true;
		}
		
		curr = curr->m_next;
	}

	return false;
}

template<typename ListData>
void List<ListData>::Print() const
{
	cout << "(size=" << m_size << ") : ";

	ListNode<ListData>* curr = m_first;

	while (curr)
	{
		ListData* value = curr->m_value;
		cout << "[" << *value << "] -> ";
		curr = curr->m_next;
	}

	cout << "[nullptr]" << endl;
}
