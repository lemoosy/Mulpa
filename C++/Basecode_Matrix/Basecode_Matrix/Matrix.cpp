#include "Matrix.h"

Matrix::Matrix(int w, int h)
{
	assert(w > 0);
	assert(h > 0);

	m_w = w;
	m_h = h;

	m_values = new float*[h];

	for (int j = 0; j < h; j++)
	{
		m_values[j] = new float[w]();
	}
}

Matrix::~Matrix()
{
	for (int j = 0; j < m_h; j++)
	{
		delete[] m_values[j];
	}

	delete[] m_values;
}

void Matrix::Set(int i, int j, float value)
{
	assert((0 <= i) && (i < m_w));
	assert((0 <= j) && (j < m_h));

	m_values[j][i] = value;
}

float Matrix::Get(int i, int j)
{
	assert((0 <= i) && (i < m_w));
	assert((0 <= j) && (j < m_h));

	return m_values[j][i];
}

void Matrix::Print() const
{
	cout << "Matrix (" << m_w << "x" << m_h << ") :\n\n";

	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			cout << m_values[j][i] << '\t';
		}

		cout << '\n';
	}

	cout << endl;
}

void Matrix::Add(const Matrix& m)
{
	assert(m_w == m.m_w);
	assert(m_h == m.m_h);

	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] += m.m_values[j][i];
		}
	}
}

void Matrix::Sub(const Matrix& m)
{
	assert(m_w == m.m_w);
	assert(m_h == m.m_h);

	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] -= m.m_values[j][i];
		}
	}
}

void Matrix::Scale(float s)
{
	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] *= s;
		}
	}
}

Matrix Matrix::Mul(const Matrix& m) const
{
	return Matrix(0, 0);
}

void Matrix::Randomize(float a, float b)
{
	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] = Float_Random(a, b);
		}
	}
}

void Matrix::Fill(float value)
{
	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] = value;
		}
	}
}
