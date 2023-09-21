#pragma once

#include "Settings.h"

class Matrix
{
private:

	/// @brief Largeur de la matrice.
	int m_w;

	/// @brief Hauteur de la matrice.
	int m_h;

	/// @brief Valeurs de la matrice.
	float** m_values;

public:

	Matrix(int w, int h);

	~Matrix();

	void Set(int i, int j, float value);

	float Get(int i, int j);

	void Print();

	void Add(Matrix& m);

	void Sub(Matrix& m);

	void Scale(float s);
};
