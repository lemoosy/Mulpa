#pragma once

#include "Settings.h"
#include "Utils.h"

/// @brief Classe repr�sentant une matrice.
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

	Matrix(const Matrix& copy);

	~Matrix();

	/// @brief Modifie la valeur d'une case de la matrice.
	void Set(int i, int j, float value);

	/// @brief Retourne la valeur d'une case de la matrice.
	float Get(int i, int j);

	/// @brief Affiche la matrice.
	void Print() const;

	/// @brief Additionne deux matrices.
	void Add(const Matrix& m);

	/// @brief Soustrait deux matrices.
	void Sub(const Matrix& m);

	/// @brief Multiplie chaque valeur de la matrice par un scalaire.
	void Scale(float s);

	/// @brief Multiplie deux matrices.
	/// @return Le produit des deux matrices.
	Matrix Mul(const Matrix& m) const;

	/// @brief Modifie toutes les valeurs de la matrice al�atoirement.
	void Randomize(float a, float b);

	/// @brief Modifie toutes les valeurs de la matrice avec la valeur pass�e en param�tre.
	void Fill(float value);

	/// @brief Compose chaque valeur de la matrice avec la fonction pass�e en param�tre.
	void Compose(Matrix& m, float (*func)(float));
};

void operator+=(Matrix& m1, const Matrix& m2);

void operator-=(Matrix& m1, const Matrix& m2);

void operator*=(Matrix& m, float s);

Matrix operator+(const Matrix& m1, const Matrix& m2);

Matrix operator-(const Matrix& m1, const Matrix& m2);

Matrix operator*(const Matrix& m1, const Matrix& m2);
