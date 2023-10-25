#include "Graph.h"

int main(void)
{
    Graph graph(7);
    
    graph.SetWeight(0, 1, 5.0f);
    graph.SetWeight(0, 2, 2.0f);
    graph.SetWeight(1, 3, 1.0f);
    graph.SetWeight(1, 4, 5.0f);
    graph.SetWeight(2, 1, 2.0f);
    graph.SetWeight(2, 3, 1.0f);
    graph.SetWeight(2, 5, 5.0f);
    graph.SetWeight(3, 6, 6.0f);
    graph.SetWeight(3, 5, 3.0f);
    graph.SetWeight(4, 6, 1.0f);
    graph.SetWeight(5, 6, 1.0f);

    graph.Print();

    float distance = 0.0f;
    std::vector<int>* res = graph.Dijkstra(0, 6, &distance);
    
    for (int i : *res)
    {
        std::cout << i << " ";
    }

    std::cout << "distance : " << distance << std::endl;
    
    delete res;

    return EXIT_SUCCESS;
}
