#pragma once

#include "List.h"
#include "Matrix.h"
#include "Settings.h"

/// @brief Classe repsésentant une couche dans un réseau de neurones.
class Layer
{
public:

	/// @brief Nombre de neurones dans la couche.
	int m_size;

	/// @brief Poids de la couche.
	Matrix* m_W;

	///// @brief Biais de la couche.
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

	void AddLayer(int size, float (*activationFunc)(float));

	void Print(int index) const;

	Matrix* Forward(Matrix* X);

	void Crossover(NN* other);

	void Mutate() const;
};
