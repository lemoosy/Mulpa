#pragma once

#include "Settings.h"
#include "Vec2.h"

class Object
{
private:

	///@brief Taille de l'objet (.x = largeur, .y = hauteur).
	Vec2 m_size;

	/// @brief Vitesse de l'objet (.x = horizontale, .y = verticale).
	Vec2 m_speed;

	/// @brief Vélocité de l'objet.
	Vec2 m_velocity;

	/// @brief Position de l'objet.
	Vec2 m_position;

	/// @brief Position précédente de l'objet(à la frame précédente).
	Vec2 m_positionPrevious;

	/// @brief Masse de l'objet.
	float m_mass;

	/// @brief Coefficient de friction de l'objet.
	float m_friction;

	/// @brief Gravité de l'objet.
	float m_gravity;

	int m_maskID;

public:

	Object(
		Vec2 p_size,
		Vec2 p_speed,
		Vec2 p_position,
		float p_mass,
		float p_friction,
		float p_gravity,
		int p_maskID
	);

	~Object();

	void UpdateVelocity(float p_timeStep);

	void UpdatePosition(DList<Object>* p_objects, float p_timeStep);

	void Draw();
};
