#pragma once

#include "DList.h"
#include "Matrix.h"
#include "Settings.h"

/// @brief Classe représentant une couche dans un réseau de neurones.
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

	/// @brief Constructeur par défaut.
	/// @param size Nombre de noeuds dans la couche.
	/// @param sizePrev Nombre de noeuds dans la couche précédente.
	/// @param activationFunc Fonction d'activation de la couche.
	Layer(int size, int sizePrev, float (*activationFunc)(float));

	~Layer();
};

/// @brief Classe représentant un réseau de neurones.
class NN
{
private:

	/// @brief Nombre d'entrées du réseau de neurones.
	int m_inputSize;

	/// @brief Liste des couches du réseau de neurones.
	DList<Layer>* m_layers;

	/// @brief Score du réseau de neurones (PG).
	float m_score;

public:

	/// @brief Retourne le score du réseau de neurones.
	inline int GetScore(void) const { return m_score; }

	/// @brief Modifie le score du réseau de neurones.
	inline void SetScore(float score) { m_score = score; }

	/// @brief Crée un réseau de neurones avec 'inputSize' entrées.
	/// @brief Le score est initialisé à 0.
	NN(int inputSize);

	/// @brief Crée un réseau de neurones à partir d'un autre réseau de neurones.
	/// @brief Copie toutes les couches (même le score).
	NN(const NN& nnCopy);

	/// @brief Détruit le réseau de neurones ainsi que toutes ses couches.
	~NN();

	/// @brief Ajoute une couche au réseau de neurones.
	/// @param size Nombre de neurones dans la couche.
	/// @param activationFunc Fonction d'activation de la couche.
	void AddLayer(int size, float (*activationFunc)(float));

	/// @brief Affiche une couche du réseau de neurones.
	void Print(int index) const;

	/// @brief Réalise la propagation avant du réseau de neurones.
	/// @param X Matrice d'entrée.
	void Forward(Matrix* X);

	/// @brief Renvoie la sortie du réseau de neurones.
	Matrix* GetOutput(void) const;

	/// @brief Renvoie la couche d'index 'index'.
	Layer* GetLayer(int index) const;

	/// @brief Réalise un croisement entre deux réseaux de neurones ('this' et 'nn2').
	/// @brief Le score de l'enfant est initialisé à 0.
	/// @return Un nouveau réseau de neurones (l'enfant).
	NN* Crossover(NN* nn2) const;

	/// @brief Modifie un poids ou un biais aléatoirement.
	void Mutation(void);
};
