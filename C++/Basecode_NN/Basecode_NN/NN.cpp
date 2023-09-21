#include "NN.h"

Layer::Layer(int size, int sizePrev, float (*activationFunc)(float))
{
	m_size = size;

	m_W = new Matrix(size, sizePrev);
	m_W->Randomize(-1.0f, +1.0f);

	m_B = new Matrix(size, 1);
	m_B->Randomize(-1.0f, +1.0f);

	m_activationFunc = activationFunc;
}

Layer::~Layer()
{
	delete m_W;
	delete m_B;
}

NN::NN(int inputSize)
{
	m_inputSize = inputSize;
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
		layer = new Layer(size, m_inputSize, activationFunc);
	}
	else
	{
		// TODO: opti avec DList
		Layer* last = m_layers->GetLast()->m_value;
		layer = new Layer(size, last->m_size, activationFunc);
	}

	// TODO: opti avec DList
	m_layers->InsertLast(layer);
}

void NN::Print(int index) const
{
	if (index < 0)
	{
		index += m_layers->GetSize();
	}

	assert((0 <= index) && (index < m_layers->GetSize()));

	cout << "---------- Neural Network (index=" << index << ") ----------\n\n";

	ListNode<Layer>* curr = m_layers->GetFirst();

	while (index > 0)
	{
		curr = curr->m_next;
		index--;
	}
	
	cout << "# Weights :\n\n";
	curr->m_value->m_weights->Print();

	cout << "# Bias :\n\n";
	curr->m_value->m_bias->Print();
}

Matrix* NN::Forward(Matrix* X)
{
	ListNode<Layer>* curr = m_layers->GetFirst();

	Matrix* Y = new Matrix(*X);

	while (curr)
	{
		Y->Print();

		Layer* layer = curr->m_value;

		Matrix sum = (*Y) * (*layer->m_W);
		sum.Compose(layer->m_activationFunc);

		if (curr->m_next == nullptr)
		{
			Matrix* res = new Matrix(sum);
			delete Y;
			return res;
		}

		delete Y;
		Y = new Matrix(sum);
		curr = curr->m_next;
	}

	return nullptr;
}

void NN::Crossover(NN* other)
{
}

void NN::Mutate() const
{
}
