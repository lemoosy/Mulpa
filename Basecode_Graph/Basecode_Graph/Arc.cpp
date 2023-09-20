#include "Arc.h"

Arc::Arc()
{
	m_u = -1;
	m_v = -1;
	m_w = -1.0f;
}

Arc::Arc(int u, int v, float w)
{
	m_u = u;
	m_v = v;
	m_w = w;
}

bool operator==(const Arc& a, const Arc& b)
{
    return (a.m_v == b.m_v);
}
