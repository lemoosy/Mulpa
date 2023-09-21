#pragma once

#include "List.h"
#include "Matrix.h"
#include "Settings.h"

/// @brief Classe reps�sentant une couche dans un r�seau de neurones.
class Layer
{
public:

	/// @brief Nombre de neurones dans la couche.
	int m_size;

	/// @brief Poids de la couche.
	Matrix* m_weights;

	/// @brief Biais de la couche.
	Matrix* m_bias;

	/// @brief Fonction d'activation de la couche.
	float (*m_activationFunc)(float);

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
	int m_sizeInput;

	List<Layer>* m_layers;

public:

	NN(int sizeInput);

	~NN();

	void AddLayer(int size, float (*activationFunc)(float));

	Matrix Forward(Matrix X);

	void Crossover(NN* other);

	void Mutate() const;
};
