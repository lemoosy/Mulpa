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

Matrix::Matrix(const Matrix& m)
{
	m_w = m.m_w;
	m_h = m.m_h;

	m_values = new float*[m_h];

	for (int j = 0; j < m_h; j++)
	{
		m_values[j] = new float[m_w];

		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] = m.m_values[j][i];
		}
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

float Matrix::Get(int i, int j) const
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
			cout << fixed << setprecision(2) << m_values[j][i] << '\t';
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

Matrix Matrix::Multiply(const Matrix& m) const
{
	assert(m_w == m.m_h);

	int w = m.m_w;
	int h = m_h;

	Matrix res = Matrix(w, h);

	for (int j = 0; j < h; j++)
	{
		for (int i = 0; i < w; i++)
		{
			float sum = 0.0f;

			for (int k = 0; k < m_w; k++)
			{
				sum += m_values[j][k] * m.m_values[k][i];
			}

			res.m_values[j][i] = sum;
		}
	}

	return res;
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

void Matrix::Compose(float (*function)(float))
{
	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			m_values[j][i] = function(m_values[j][i]);
		}
	}
}

void operator+=(Matrix& m1, const Matrix& m2)
{
	m1.Add(m2);
}

void operator-=(Matrix& m1, const Matrix& m2)
{
	m1.Sub(m2);
}

void operator*=(Matrix& m, float s)
{
	m.Scale(s);
}

Matrix operator+(const Matrix& m1, const Matrix& m2)
{
	Matrix res = m1;
	res.Add(m2);
	return res;
}

Matrix operator-(const Matrix& m1, const Matrix& m2)
{
	Matrix res = m1;
	res.Sub(m2);
	return res;
}

Matrix operator*(const Matrix& m1, const Matrix& m2)
{
	return m1.Multiply(m2);
}
