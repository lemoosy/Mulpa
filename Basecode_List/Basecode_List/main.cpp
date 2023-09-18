#include "List.h"
#include "Settings.h"

int main()
{
	srand(time(nullptr));

	List<int> list;

	for (int i = 0; i < 100; i++)
	{
		int r = rand() % 100;
		list.InsertFirst(new int(r));
	}

	list.Print();

	for (int i = 0; i < 50; i++)
	{
		int* res = list.PopFirst();
		delete res;
	}

	list.Print();

	return EXIT_SUCCESS;
}
