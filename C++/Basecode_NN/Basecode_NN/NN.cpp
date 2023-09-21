#include "NN.h"

Layer::Layer(int size, int sizePrev, float (*activationFunc)(float))
{
	m_size = size;

	m_weights = new Matrix(size, sizePrev);
	m_weights->Randomize(-1.0f, +1.0f);

	m_bias = new Matrix(size, 1);
	m_bias->Randomize(-1.0f, +1.0f);

	m_activationFunc = activationFunc;
}

Layer::~Layer()
{
	delete m_weights;
	delete m_bias;
}

NN::NN(int sizeInput)
{
	m_sizeInput = sizeInput;
	m_layers = new List<Layer>();
}

NN::~NN()
{
	delete m_layers;
}

void NN::AddLayer(int size, float (*activationFunc)(float))
{
	Layer* layer = nullptr;

	if (m_layers->IsEmpty())
	{
		layer = new Layer(size, m_sizeInput, activationFunc);
	}
	else
	{
		// TODO: opti avec DList
		Layer* last = m_layers->GetLast()->m_value;
		layer = new Layer(size, last->m_size, activationFunc);
		m_layers->InsertLast(layer);
	}
}

Matrix NN::Forward(Matrix X)
{
	ListNode<Layer>* curr = m_layers->GetFirst();

	while (curr)
	{



	}

	return Matrix();
}
