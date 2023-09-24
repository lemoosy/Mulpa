#include "DList.h"

int main(void)
{
    DList<int> list;

    list.InsertLast(new int(0));
    list.InsertLast(new int(5));
    list.InsertLast(new int(9));
    list.InsertLast(new int(3));
    list.InsertLast(new int(1));

    list.Print();

    return EXIT_SUCCESS;
}
