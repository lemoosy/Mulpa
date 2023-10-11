#pragma once

#include "Settings.h"
#include "Utils.h"

/// @brief Classe repr�sentant une matrice.
class Matrix
{
public:
	/// @brief Largeur de la matrice.
	int m_w;

	/// @brief Hauteur de la matrice.
	int m_h;

	/// @brief Matrice.
	Eigen::MatrixXd* m_matrix;


	/// @brief Construit une matrice w x h (toutes les valeurs sont initialis�es � 0).
	Matrix(int w, int h);

	/// @brief Construit une matrice � partir d'une matrice (copie la matrice pass�e en param�tre).
	Matrix(const Matrix& m);

	~Matrix();

	/// @brief Retourne la largeur de la matrice.
	inline int GetWidth() const { return m_w; }
	
	/// @brief Retourne la hauteur de la matrice.
	inline int GetHeight() const { return m_h; }

	/// @brief Modifie la valeur d'une case de la matrice.
	void SetValue(int i, int j, float value);

	/// @brief Retourne la valeur d'une case de la matrice.
	float GetValue(int i, int j) const;

	/// @brief Affiche la matrice.
	void Print() const;

	/// @brief Copie les valeurs de la matrice pass�e en param�tre.
	void Copy(const Matrix& m);

	/// @brief Additionne deux matrices.
	void Add(const Matrix& m);

	/// @brief Soustrait deux matrices.
	void Sub(const Matrix& m);

	/// @brief Multiplie chaque valeur de la matrice par un scalaire.
	void Scale(float s);

	/// @brief Multiplie deux matrices.
	/// @return Le produit des deux matrices.
	Matrix Multiply(const Matrix& m) const;

	/// @brief Remplit la matrice avec la valeur pass�e en param�tre.
	void FillValue(float value);

	/// @brief Remplit la matrice avec des valeurs al�atoires entre a et b.
	void FillValueRandom(float a, float b);

	/// @brief Compose la matrice avec la fonction pass�e en param�tre.
	void Composition(float (*function)(float));

	/// @brief V�rifie si les coordonn�es pass�es en param�tre sont en dehors de la matrice.
	bool OutOfDimension(int i, int j) const;
	
	/// @brief Attribue al�atoirement des valeurs de la matrice pass�e en param�tre � la matrice courante (50%).
	void Mix(const Matrix& m);
};

void operator+=(Matrix& m1, const Matrix& m2);

void operator-=(Matrix& m1, const Matrix& m2);

void operator*=(Matrix& m, float s);

Matrix operator+(const Matrix& m1, const Matrix& m2);

Matrix operator-(const Matrix& m1, const Matrix& m2);

Matrix operator*(const Matrix& m1, const Matrix& m2);
