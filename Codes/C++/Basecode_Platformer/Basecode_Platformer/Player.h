#pragma once

#include "Settings.h"
#include "Vec2.h"
#include "Object.h"

/// @brief Classe repr�sentant un joueur.
class Player : Object
{
private:

	/// @brief R�seau de neurones du joueur.
	NN* m_brain;

public:

	Player(float p_x, float p_y, NN* p_brain = nullptr);

	~Player();

	void UpdateVelocity(DList* float p_timeStep);

	void UpdatePosition(float p_timeStep);

	void Draw();
};
