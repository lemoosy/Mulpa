#include "Graph.h"

int main()
{
    Graph graph(7);

    graph.SetWeight(0, 1, 5);
    graph.SetWeight(0, 2, 2);
    
    graph.SetWeight(1, 3, 1);
    graph.SetWeight(1, 4, 5);

    graph.SetWeight(2, 1, 2);
    graph.SetWeight(2, 3, 1);
    graph.SetWeight(2, 5, 5);

    graph.SetWeight(3, 6, 6);
    graph.SetWeight(3, 5, 3);
    
    graph.SetWeight(4, 6, 1);

    graph.SetWeight(5, 6, 1);

    graph.Print();

    graph.Dijkstra(0, 6);

    return EXIT_SUCCESS;
}
