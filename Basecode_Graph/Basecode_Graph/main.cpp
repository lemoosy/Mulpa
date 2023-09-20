#include "Graph.h"

int main()
{
    Graph graph(5);

    graph.SetWeight(3, 2, 20);
    graph.SetWeight(3, 4, 10);

    graph.Print();

    return EXIT_SUCCESS;
}
