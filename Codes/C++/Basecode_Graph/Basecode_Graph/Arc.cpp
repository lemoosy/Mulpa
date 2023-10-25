#include "Arc.h"

Arc::Arc()
{
	m_u = -1;
	m_v = -1;
	m_w = -1.0f;
}

Arc::Arc(int u, int v, float w)
{
	assert(u >= 0);
	assert(v >= 0);
	assert(w >= 0.0f);

	m_u = u;
	m_v = v;
	m_w = w;
}

void Arc::Print() const
{
	std::cout << "Arc: [" << m_u << ", " << m_v << ", " << m_w << "]" << std::endl;
}
