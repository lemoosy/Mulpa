#pragma once

#include "Settings.h"
#include "Utils.h"

/// @brief Classe représentant une matrice.
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

	Matrix(const Matrix& m);

	~Matrix();

	/// @brief Retourne la largeur de la matrice.
	inline int GetWidth() const { return m_w; }
	
	/// @brief Retourne la hauteur de la matrice.
	inline int GetHeight() const { return m_h; }

	/// @brief Modifie la valeur d'une case de la matrice.
	void Set(int i, int j, float value);

	/// @brief Retourne la valeur d'une case de la matrice.
	float Get(int i, int j) const;

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
	Matrix Multiply(const Matrix& m) const;

	/// @brief Modifie chaque case de la matrice par une valeur aléatoire comprise entre a et b.
	void Randomize(float a, float b);

	/// @brief Remplie la matrice avec la valeur passée en paramètre.
	void Fill(float value);

	/// @brief Compose chaque valeur de la matrice avec la fonction passée en paramètre.
	void Compose(float (*function)(float));

	/// @brief Mélange les valeurs de deux matrices aléatoirement.
	friend void Mix(Matrix* m1, Matrix* m2);

	void Copy(const Matrix& m);

	bool OutOfDimension(int i, int j);
};

void operator+=(Matrix& m1, const Matrix& m2);

void operator-=(Matrix& m1, const Matrix& m2);

void operator*=(Matrix& m, float s);

Matrix operator+(const Matrix& m1, const Matrix& m2);

Matrix operator-(const Matrix& m1, const Matrix& m2);

Matrix operator*(const Matrix& m1, const Matrix& m2);
