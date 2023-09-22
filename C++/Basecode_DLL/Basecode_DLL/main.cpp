#include "Settings.h"
#include "Variables.h"

GameMakerDLL void GML_Init()
{
	g_a = new int();
	*g_a = 10;

	g_b = new int(20);
}

GameMakerDLL double GML_GetA()
{
	return (double)*g_a;
}

GameMakerDLL int GML_GetB()
{
	return *g_b;
}
//
//GameMakerDLL int NN_Create()
//{
//	assert(m_nnCursor < NN_CAPACITY);
//
//	int res = m_nnCursor;
//
//	m_nn[m_nnCursor] = new NN(NN_SIZE_INPUT);
//	m_nnCursor++;
//
//	return res;
//}
//
//GameMakerDLL void NN_Destroy(int id)
//{
//
//}


int main()
{
	return EXIT_SUCCESS;
}
