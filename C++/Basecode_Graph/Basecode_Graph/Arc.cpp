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

void Arc::Print() const
{
	cout << m_u << ", " << m_v << ", " << m_w;
}

bool operator==(const Arc& a, const Arc& b)
{
    return (a.m_v == b.m_v);
}

ostream& operator<<(ostream& stream, Arc& arc)
{
	arc.Print();

	return stream;
}
