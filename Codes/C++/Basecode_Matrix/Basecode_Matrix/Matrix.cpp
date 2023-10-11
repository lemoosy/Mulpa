#include "Matrix.h"

Matrix::Matrix(int w, int h)
{
	assert(w > 0);
	assert(h > 0);

	m_w = w;
	m_h = h;

	m_matrix = new Eigen::MatrixXd(h, w);
}

Matrix::Matrix(const Matrix& m)
{
	m_w = m.m_w;
	m_h = m.m_h;

	m_matrix = new Eigen::MatrixXd(*(m.m_matrix));
}

Matrix::~Matrix()
{
	delete m_matrix;
}

void Matrix::SetValue(int i, int j, float value)
{
	assert((0 <= i) && (i < m_w));
	assert((0 <= j) && (j < m_h));

	(*m_matrix)(j, i) = value;
}

float Matrix::GetValue(int i, int j) const
{
	assert((0 <= i) && (i < m_w));
	assert((0 <= j) && (j < m_h));

	return (*m_matrix)(j, i);
}

void Matrix::Print() const
{
	cout << "Matrix (" << m_w << "x" << m_h << ") :\n";
	cout << *m_matrix;
	cout << endl;
}

void Matrix::Copy(const Matrix& m)
{
	assert(m_w == m.m_w);
	assert(m_h == m.m_h);

	(*m_matrix) = (*m.m_matrix);
}

void Matrix::Add(const Matrix& m)
{
	assert(m_w == m.m_w);
	assert(m_h == m.m_h);
	
	(*m_matrix) += (*m.m_matrix);
}

void Matrix::Sub(const Matrix& m)
{
	assert(m_w == m.m_w);
	assert(m_h == m.m_h);

	(*m_matrix) -= (*m.m_matrix);
}

void Matrix::Scale(float s)
{
	(*m_matrix) *= s;
}

Matrix Matrix::Multiply(const Matrix& m) const
{
	assert(m_w == m.m_h);

	Eigen::MatrixXd matrix = (*m_matrix) * (*m.m_matrix);

	Matrix res(m.m_w, m_h);
	(*res.m_matrix) = matrix;

	return res;
}

void Matrix::FillValue(float value)
{
	m_matrix->fill(value);
}

void Matrix::FillValueRandom(float a, float b)
{
	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			SetValue(i, j, Float_Random(a, b));
		}
	}
}

void Matrix::Composition(float (*function)(float))
{
	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			SetValue(i, j, function(GetValue(i, j)));
		}
	}
}

bool Matrix::OutOfDimension(int i, int j) const
{
	return ((i < 0) || (i >= m_w) || (j < 0) || (j >= m_h));
}

void Matrix::Mix(const Matrix& m)
{
	assert(m_w == m.m_w);
	assert(m_h == m.m_h);

	for (int j = 0; j < m_h; j++)
	{
		for (int i = 0; i < m_w; i++)
		{
			if (rand() % 2)
			{
				SetValue(i, j, m.GetValue(i, j));
			}
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
	Matrix res(m1);
	res.Add(m2);
	return res;
}

Matrix operator-(const Matrix& m1, const Matrix& m2)
{
	Matrix res(m1);
	res.Sub(m2);
	return res;
}

Matrix operator*(const Matrix& m1, const Matrix& m2)
{
	return m1.Multiply(m2);
}
