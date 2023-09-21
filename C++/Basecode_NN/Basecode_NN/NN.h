#pragma once

#include "List.h"
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

	///// @brief Matrice qui stocke les biais de la couche.
	Matrix* m_B;

	///// @brief Fonction d'activation de la couche.
	float (*m_activationFunc)(float);

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
	List<Layer>* m_layers;

public:

	NN(int inputSize);

	~NN();

	/// @brief Ajoute une couche au réseau de neurones.
	/// @param size Nombre de neurones dans la couche.
	/// @param activationFunc Fonction d'activation de la couche.
	void AddLayer(int size, float (*activationFunc)(float));

	/// @brief Affiche une couche du réseau de neurones.
	void Print(int index) const;

	/// @brief Réalise la propagation avant du réseau de neurones.
	/// @param X Matrice d'entrée.
	/// @return Matrice de sortie.
	Matrix* Forward(Matrix* X);

	void Crossover(NN* other);

	void Mutate() const;
};
