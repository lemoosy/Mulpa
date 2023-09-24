#pragma once

#include "DList.h"
#include "Matrix.h"
#include "Settings.h"

/// @brief Classe repr�sentant une couche dans un r�seau de neurones.
class Layer
{
public:

	/// @brief Nombre de neurones dans la couche.
	int m_size;

	/// @brief Matrice qui stocke les poids de la couche.
	Matrix* m_W;

	/// @brief Matrice qui stocke les biais de la couche.
	Matrix* m_B;

	/// @brief Fonction d'activation de la couche.
	float (*m_activationFunc)(float);

	/// @brief Sortie de la couche.
	Matrix* m_Y;

	/// @brief Constructeur par d�faut.
	/// @param size Nombre de noeuds dans la couche.
	/// @param sizePrev Nombre de noeuds dans la couche pr�c�dente.
	/// @param activationFunc Fonction d'activation de la couche.
	Layer(int size, int sizePrev, float (*activationFunc)(float));

	~Layer();
};

/// @brief Classe repr�sentant un r�seau de neurones.
class NN
{
private:

	/// @brief Nombre d'entr�es du r�seau de neurones.
	int m_inputSize;

	/// @brief Liste des couches du r�seau de neurones.
	DList<Layer>* m_layers;

	/// @brief Score du r�seau de neurones (PG).
	float m_score;

public:

	/// @brief Retourne le score du r�seau de neurones.
	inline int GetScore(void) const { return m_score; }

	/// @brief Modifie le score du r�seau de neurones.
	inline void SetScore(float score) { m_score = score; }

	/// @brief Cr�e un r�seau de neurones avec 'inputSize' entr�es.
	/// @brief Le score est initialis� � 0.
	NN(int inputSize);

	/// @brief Cr�e un r�seau de neurones � partir d'un autre r�seau de neurones.
	/// @brief Copie toutes les couches (m�me le score).
	NN(const NN& nnCopy);

	/// @brief D�truit le r�seau de neurones ainsi que toutes ses couches.
	~NN();

	/// @brief Ajoute une couche au r�seau de neurones.
	/// @param size Nombre de neurones dans la couche.
	/// @param activationFunc Fonction d'activation de la couche.
	void AddLayer(int size, float (*activationFunc)(float));

	/// @brief Affiche une couche du r�seau de neurones.
	void Print(int index) const;

	/// @brief R�alise la propagation avant du r�seau de neurones.
	/// @param X Matrice d'entr�e.
	void Forward(Matrix* X);

	/// @brief Renvoie la sortie du r�seau de neurones.
	Matrix* GetOutput(void) const;

	/// @brief Renvoie la couche d'index 'index'.
	Layer* GetLayer(int index) const;

	/// @brief R�alise un croisement entre deux r�seaux de neurones ('this' et 'nn2').
	/// @brief Le score de l'enfant est initialis� � 0.
	/// @return Un nouveau r�seau de neurones (l'enfant).
	NN* Crossover(NN* nn2) const;

	/// @brief Modifie un poids ou un biais al�atoirement.
	void Mutation(void);
};
