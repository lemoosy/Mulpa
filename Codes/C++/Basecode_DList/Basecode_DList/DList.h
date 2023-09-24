#pragma once

#include "Settings.h"

/// @brief Classe représentant un noeud dans une liste doublement chaînée.
template <typename T>
class DListNode
{
public:

	/// @brief Pointeur vers la valeur du noeud.
	T* m_value;

	/// @brief Pointeur vers le noeud précédent.
	DListNode<T>* m_prev;

	/// @brief Pointeur vers le noeud suivant.
	DListNode<T>* m_next;

	DListNode(T* p_value, DListNode* p_prev = nullptr, DListNode* p_next = nullptr);
};

template <typename T>
DListNode<T>::DListNode(T* p_value, DListNode* p_prev, DListNode* p_next)
{
	m_value = p_value;
	m_prev = p_prev;
	m_next = p_next;
}

/// @brief Classe représentant une liste doublement chaînée.
template <typename T>
class DList
{
private:

	/// @brief Taille de la liste.
	int m_size;

	/// @brief Sentinelle de la liste.
	DListNode<T>* m_sentinel;

public:

	/// @brief Retourne la taille de la liste.
	inline int GetSize() const { return m_size; }

	/// @brief Retourne la sentinelle de la liste.
	inline DListNode<T>* GetSentinel() const { return m_sentinel; }

	/// @brief Vérifie si la liste est vide.
	inline bool IsEmpty() const { return m_size == 0; }

	/// @brief Construit une liste vide.
	DList();

	/// @brief Détruit la liste (les valeurs sont aussi détruites).
	~DList();

	/// @brief Affiche la liste.
	void Print() const;

	/// @brief Insère une valeur au début de la liste.
	void InsertFirst(T* p_value);

	/// @brief Insère une valeur à la fin de la liste.
	void InsertLast(T* p_value);

	///@brief Retire et retourne la première valeur de la liste.
	T* PopFirst();

	///@brief Retire et retourne la dernière valeur de la liste.
	T* PopLast();

	/// @brief Retourne la valeur à l'index donné.
	T* GetValue(int index) const;

	/// @brief Vérifie si une valeur est dans la liste.
	/// @return Pointeur vers la valeur si elle est dans la liste, nullptr sinon.
	T* IsIn(T* p_value) const;

	/// @brief Retire une valeur de la liste.
	/// @return Pointeur vers la valeur retirée si elle est dans la liste, nullptr sinon.
	T* Remove(T* p_value);
};

template<typename T>
DList<T>::DList()
{
	m_size = 0;
	m_sentinel = new DListNode<T>(nullptr);
	m_sentinel->m_prev = m_sentinel;
	m_sentinel->m_next = m_sentinel;
}

template<typename T>
DList<T>::~DList()
{
	DListNode<T>* curr = m_sentinel->m_next;

	while (curr != m_sentinel)
	{
		DListNode<T>* next = curr->m_next;

		delete curr->m_value;
		delete curr;

		curr = next;
	}

	delete m_sentinel;
}

template<typename T>
void DList<T>::Print() const
{
	cout << "(size=" << m_size << ") : ";

	DListNode<T>* curr = m_sentinel->m_next;

	while (curr != m_sentinel)
	{
		T* value = curr->m_value;
		cout << "[" << *value << "] -> ";
		curr = curr->m_next;
	}

	cout << "[nullptr]\n" << endl;
}

template<typename T>
void DList<T>::InsertFirst(T* value)
{
	DListNode<T>* node = new DListNode<T>(value, m_sentinel, m_sentinel->m_next);

	node->m_next->m_prev = node;
	node->m_prev->m_next = node;

	m_size++;
}

template<typename T>
void DList<T>::InsertLast(T* value)
{
	DListNode<T>* node = new DListNode<T>(value, m_sentinel->m_prev, m_sentinel);

	node->m_next->m_prev = node;
	node->m_prev->m_next = node;

	m_size++;
}

template<typename T>
T* DList<T>::PopFirst()
{
	assert(m_size > 0);

	DListNode<T>* node = m_sentinel->m_next;

	node->m_next->m_prev = node->m_prev;
	node->m_prev->m_next = node->m_next;

	T* res = node->m_value;

	delete node;

	m_size--;

	return res;
}

template<typename T>
T* DList<T>::PopLast()
{
	assert(m_size > 0);

	DListNode<T>* node = m_sentinel->m_prev;

	node->m_next->m_prev = node->m_prev;
	node->m_prev->m_next = node->m_next;

	T* res = node->m_value;

	delete node;

	m_size--;

	return res;
}

template<typename T>
T* DList<T>::GetValue(int index) const
{
	if (index < 0)
	{
		assert(-m_size <= index);

		DListNode<T>* curr = m_sentinel;

		while (index < 0)
		{
			index++;
			curr = curr->m_prev;
		}

		return curr->m_value;
	}
	else
	{
		assert(index < m_size);

		DListNode<T>* curr = m_sentinel->m_next;

		while (index > 0)
		{
			index--;
			curr = curr->m_next;
		}

		return curr->m_value;
	}
}

template<typename T>
T* DList<T>::IsIn(T* value) const
{
	DListNode<T>* curr = m_sentinel->m_next;

	while (curr != m_sentinel)
	{
		if (*value == *(curr->m_value))
		{
			return curr->m_value;
		}

		curr = curr->m_next;
	}

	return nullptr;
}

template<typename T>
T* DList<T>::Remove(T* value)
{
	DListNode<T>* curr = m_sentinel->m_next;

	while (curr != m_sentinel)
	{
		if (*value == *(curr->m_value))
		{
			T* res = curr->m_value;

			curr->m_next->m_prev = curr->m_prev;
			curr->m_prev->m_next = curr->m_next;

			delete curr;

			m_size--;

			return res;
		}
		
		curr = curr->m_next;
	}

	return nullptr;
}
