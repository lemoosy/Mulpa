#pragma once

/// @brief Classe représentant un vecteur 2D.
class Vec2
{
public:

	/// @brief Abscisse du vecteur.
	float m_x;

	/// @brief Ordonnée du vecteur.
	float m_y;

	Vec2(float p_x, float p_y);

	void Add(const Vec2& p_v);
	
	void Sub(const Vec2& p_v);

	void Scale(float p_s);
};

Vec2 operator+(const Vec2& const p_v1, const Vec2& p_v2);
Vec2 operator-(const Vec2& const p_v1, const Vec2& p_v2);
Vec2 operator*(const Vec2& const p_v, float s);
