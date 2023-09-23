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

	m_Y = new Matrix(size, 1);

	m_activationFunc = activationFunc;
}

Layer::~Layer()
{
	delete m_W;
	delete m_B;
	delete m_Y;
}

NN::NN(int inputSize)
{
	assert(inputSize > 0);

	m_inputSize = inputSize;
	m_layers = new List<Layer>();
}

NN::NN(const NN* other)
{
	assert(other);

	m_inputSize = other->m_inputSize;
	m_layers = new List<Layer>();

	ListNode<Layer>* curr = other->m_layers->GetFirst();

	while (curr)
	{
		Layer* layer = curr->m_value;

		Layer* newLayer = new Layer(
			layer->m_size,
			layer->m_W->GetHeight(),
			layer->m_activationFunc
		);

		newLayer->m_W->Copy(*layer->m_W);
		newLayer->m_B->Copy(*layer->m_B);

		m_layers->InsertLast(newLayer);

		curr = curr->m_next;
	}
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

void NN::Forward(Matrix* X)
{
	/// TODO: à optimiser.

	ListNode<Layer>* curr = m_layers->GetFirst();

	Matrix* Xptr = X;

	while (curr)
	{
		Layer* layer = curr->m_value;

		Matrix W = *layer->m_W;					// Poids.
		Matrix B = *layer->m_B;					// Biais.
		Matrix Z = (*Xptr) * W + B;				// Sorties fonction somme.
		Z.Compose(layer->m_activationFunc);		// Sorties fonction d'activation.

		layer->m_Y->Copy(Z);

		Xptr = layer->m_Y;

		curr = curr->m_next;
	}
}

Layer* NN::GetLayer(int index)
{
	ListNode<Layer>* curr = m_layers->GetFirst();

	if (index < 0)
	{
		index += m_layers->GetSize();
	}

	assert((0 <= index) && (index < m_layers->GetSize()));

	while (index > 0)
	{
		index--;
		curr = curr->m_next;
	}

	return curr->m_value;
}

NN* NN::Crossover(NN* other)
{
	assert(m_layers->GetSize() == other->m_layers->GetSize());

	NN* res = new NN(other);

	ListNode<Layer>* curr1 = res->m_layers->GetFirst();
	ListNode<Layer>* curr2 = other->m_layers->GetFirst();

	while (curr1)
	{
		Layer* layer1 = curr1->m_value;
		Layer* layer2 = curr2->m_value;

		Mix(layer1->m_W, layer2->m_W);
		Mix(layer1->m_B, layer2->m_B);

		curr1 = curr1->m_next;
		curr2 = curr2->m_next;
	}

	return res;
}

void NN::Mutate() const
{
}
