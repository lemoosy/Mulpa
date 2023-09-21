#pragma once

#include "Settings.h"

/// @brief Classe représentant un noeud dans une liste simplement chaînée.
template <typename ListData>
class ListNode
{
public:

	/// @brief Pointeur vers la valeur du noeud.
	ListData* m_value;

	/// @brief Pointeur vers le noeud suivant.
	ListNode<ListData>* m_next;

	ListNode();

	ListNode(ListData* value, ListNode* next = nullptr);
};

template<typename ListData>
inline ListNode<ListData>::ListNode()
{
	m_value = nullptr;
	m_next = nullptr;
}

template <typename ListData>
ListNode<ListData>::ListNode(ListData* value, ListNode* next)
{
	m_value = value;
	m_next = next;
}

/// @brief Classe représentant une liste simplement chaînée.
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

	/// @brief Vérifie si la liste est vide.
	inline bool IsEmpty() const { return m_size == 0; }

	/// @brief Retourne le premier noeud de la liste.
	inline ListNode<ListData>* GetFirst() const { return m_first; }

	List();

	~List();

	/// @brief Insère une valeur au début de la liste.
	void InsertFirst(ListData* value);

	/// @brief Insère une valeur à la fin de la liste.
	void InsertLast(ListData* value);

	///@brief Retire et retourne la première valeur de la liste.
	ListData* PopFirst();

	/// @brief Vérifie si une valeur est dans la liste.
	/// @return Pointeur vers la valeur si elle est dans la liste, nullptr sinon.
	ListData* IsIn(ListData* value) const;

	/// @brief Retire une valeur de la liste.
	/// @return Pointeur vers la valeur retirée si elle est dans la liste, nullptr sinon.
	ListData* Remove(ListData* value);

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
void List<ListData>::InsertLast(ListData* value)
{
	if (IsEmpty())
	{
		return InsertFirst(value);
	}

	ListNode<ListData>* curr = m_first;

	while (curr->m_next)
	{
		curr = curr->m_next;
	}

	curr->m_next = new ListNode<ListData>(value);
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
ListData* List<ListData>::IsIn(ListData* value) const
{
	ListNode<ListData>* curr = m_first;

	while (curr)
	{
		if (*value == *(curr->m_value))
		{
			return curr->m_value;
		}
		
		curr = curr->m_next;
	}

	return nullptr;
}

template<typename ListData>
ListData* List<ListData>::Remove(ListData* value)
{
	ListNode<ListData>* prev = nullptr;
	ListNode<ListData>* curr = m_first;

	while (curr)
	{
		if (*value == *(curr->m_value))
		{
			ListNode<ListData>* next = curr->m_next;

			if (prev)
			{
				prev->m_next = next;
			}
			else
			{
				m_first = next;
			}

			ListData* res = curr->m_value;

			delete curr;

			m_size--;

			return res;
		}
		else
		{
			prev = curr;
			curr = curr->m_next;
		}
	}

	return nullptr;
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
