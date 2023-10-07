#include "Vec2.h"

Vec2::Vec2(float p_x, float p_y)
{
	m_x = p_x;
	m_y = p_y;
}

void Vec2::Add(const Vec2& p_v)
{
	m_x += p_v.m_x;
	m_y += p_v.m_y;
}

void Vec2::Sub(const Vec2& p_v)
{
	m_x -= p_v.m_x;
	m_y -= p_v.m_y;
}

void Vec2::Scale(float p_s)
{
	m_x *= p_s;
	m_y *= p_s;
}
