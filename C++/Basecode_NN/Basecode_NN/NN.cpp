#include "NN.h"

Layer::Layer(int size, int sizePrev, float (*activationFunc)(float))
{
	assert(size > 0);
	assert(sizePrev > 0);
	assert(activationFunc);

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
	assert(inputSize > 0);

	m_inputSize = inputSize;
	m_layers = new List<Layer>();
}

NN::~NN()
{
	delete m_layers;
}

void NN::AddLayer(int size, float (*activationFunc)(float))
{
	// TODO: Fonction à optimiser avec un DList.

	Layer* layer = nullptr;

	if (m_layers->IsEmpty())
	{
		layer = new Layer(size, m_inputSize, activationFunc);
	}
	else
	{
		Layer* last = m_layers->GetLast()->m_value;
		layer = new Layer(size, last->m_size, activationFunc);
	}

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
	curr->m_value->m_W->Print();

	cout << "# Bias :\n\n";
	curr->m_value->m_B->Print();
}

Matrix* NN::Forward(Matrix* X)
{
	/// TODO: à optimiser.

	Matrix* res = nullptr;

	ListNode<Layer>* curr = m_layers->GetFirst();

	Matrix* Xptr = new Matrix(*X);

	while (curr)
	{
		Layer* layer = curr->m_value;

		Matrix X = *Xptr;						// Entrées.
		Matrix W = *layer->m_W;					// Poids.
		Matrix B = *layer->m_B;					// Biais.
		Matrix Z = X * W + B;					// Sorties fonction somme.
		Z.Compose(layer->m_activationFunc);		// Sorties fonction d'activation.

		if (curr->m_next == nullptr)
		{
			res = new Matrix(Z);
			break;
		}

		delete Xptr;
		Xptr = new Matrix(Z);
		curr = curr->m_next;
	}

	delete Xptr;

	return res;
}

void NN::Crossover(NN* other)
{
}

void NN::Mutate() const
{
}
