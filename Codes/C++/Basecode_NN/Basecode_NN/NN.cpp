#include "NN.h"

Layer::Layer(int size, int sizePrev, float (*activationFunc)(float))
{
	assert(size > 0);
	assert(sizePrev > 0);
	assert(activationFunc);

	m_size = size;

	m_W = new Matrix(size, sizePrev);
	m_W->FillValueRandom(-1.0f, +1.0f);

	m_B = new Matrix(size, 1);
	m_B->FillValueRandom(-1.0f, +1.0f);

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
	m_layers = new DList<Layer>();
	m_score = 0.0f;
}

NN::NN(const NN& nnCopy)
{
	m_inputSize = nnCopy.m_inputSize;
	m_layers = new DList<Layer>();
	m_score = nnCopy.m_score;

	DListNode<Layer>* nnCopySent = nnCopy.m_layers->GetSentinel();
	DListNode<Layer>* nnCopyCurr = nnCopySent->m_next;

	while (nnCopyCurr != nnCopySent)
	{
		Layer* nnCopyLayer = nnCopyCurr->m_value;

		Layer* insertLayer = new Layer(
			nnCopyLayer->m_size,
			nnCopyLayer->m_W->GetHeight(),
			nnCopyLayer->m_activationFunc
		);

		insertLayer->m_W->Copy(*nnCopyLayer->m_W);
		insertLayer->m_B->Copy(*nnCopyLayer->m_B);
		insertLayer->m_Y->Copy(*nnCopyLayer->m_Y);

		m_layers->InsertLast(insertLayer);

		nnCopyCurr = nnCopyCurr->m_next;
	}
}

NN::~NN()
{
	delete m_layers;
}

void NN::AddLayer(int size, float (*activationFunc)(float))
{
	Layer* insertLayer = nullptr;

	if (m_layers->IsEmpty())
	{
		insertLayer = new Layer(size, m_inputSize, activationFunc);
	}
	else
	{
		Layer* lastLayer = m_layers->GetValue(-1);
		insertLayer = new Layer(size, lastLayer->m_size, activationFunc);
	}

	m_layers->InsertLast(insertLayer);
}

void NN::Print(int index) const
{
	cout << "---------- Neural Network (index=" << index << ") ----------\n\n";

	Layer* layer = m_layers->GetValue(index);

	cout << "# Weights :\n\n";
	layer->m_W->Print();

	cout << "# Bias :\n\n";
	layer->m_B->Print();
}

void NN::Forward(Matrix* X)
{
	DListNode<Layer>* sent = m_layers->GetSentinel();
	DListNode<Layer>* curr = sent->m_next;

	Matrix* Xptr = X;

	Eigen::MatrixXd m1(1500, 500);
	Eigen::MatrixXd m2(500, 1500);

	while (curr != sent)
	{
		Layer* layer = curr->m_value;

		Eigen::MatrixXd m3 = m1 * m2;

		//layer->m_Y->Copy(*Xptr);						// Poids.
		//Matrix B = *layer->m_B;						// Biais.
		//Matrix Z = (*Xptr) * W + B;					// Sorties fonction somme.
		//Z.Composition(layer->m_activationFunc);		// Sorties fonction d'activation.
		//
		//layer->m_Y->Copy(Z);						// Sorties de la couche.

		Xptr = layer->m_Y;

		curr = curr->m_next;
	}
}

Matrix* NN::GetOutput(void) const
{
	return m_layers->GetValue(-1)->m_Y;
}

Layer* NN::GetLayer(int index) const
{
	return m_layers->GetValue(index);
}

NN* NN::Crossover(NN* nn2) const
{
	const NN* nn1 = this;

	assert(nn1->m_layers->GetSize() == nn2->m_layers->GetSize());

	NN* res = new NN(*nn1);

	DListNode<Layer>* resSent = res->m_layers->GetSentinel();
	DListNode<Layer>* resCurr = resSent->m_next;

	DListNode<Layer>* nn2Curr = nn2->m_layers->GetSentinel()->m_next;

	while (resCurr != resSent)
	{
		Layer* resLayer = resCurr->m_value;
		Layer* nn2Layer = nn2Curr->m_value;

		resLayer->m_W->Mix(*nn2Layer->m_W);
		resLayer->m_B->Mix(*nn2Layer->m_B);

		resCurr = resCurr->m_next;
		nn2Curr = nn2Curr->m_next;
	}

	res->m_score = 0.0f;

	return res;
}

void NN::Mutation(void)
{
	int index = Int_Random(0, m_layers->GetSize() - 1);

	Layer* layer = m_layers->GetValue(index);

	Matrix* matrix = nullptr;

	if (Int_Random(0, 1) == 0)
	{
		matrix = layer->m_W;
	}
	else
	{
		matrix = layer->m_B;
	}

	int i = rand() % matrix->GetWidth();
	int j = rand() % matrix->GetHeight();

	float value = Float_Random(-1.0f, +1.0f);

	matrix->SetValue(i, j, value);
}
