#include "Object.h"

Object::Object(Vec2 p_size, Vec2 p_speed, Vec2 p_position, float p_mass, float p_friction, float p_gravity, int p_maskID)
{
	m_size = p_size;
	m_speed = p_speed;
	m_position = p_position;
	m_mass = p_mass;
	m_friction = p_friction;
	m_gravity = p_gravity;
	m_maskID = p_maskID;
}

Object::~Object()
{
}

void Object::UpdateVelocity(float p_timeStep)
{
	Vec2 P = Vec2(0.0f, m_mass * m_gravity);
	Vec2 f = m_velocity * -m_friction;
	Vec2 F = P + f;
	Vec2 a = F * 1.0f / m_mass;

	m_velocity = 
}

void Object::UpdatePosition(DList<Object>* p_objects, float p_timeStep)
{

}

