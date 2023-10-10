#include <iostream>
#include <time.h>

#include "Matrix.h"

#include "Eigen/Dense"

int main()
{
    int w = 2000; // 2K
    int h = 4000; // 4K

    Matrix m1(w, h);
    m1.FillValueRandom(-1.0f, +1.0f);

    Matrix m2(h, w);
    m2.FillValueRandom(-1.0f, +1.0f);

    clock_t start = clock();

    Matrix m3 = m1 * m2; // 140s

    clock_t end = clock();

    double time = (double)(end - start) / CLOCKS_PER_SEC;

    std::cout << "Time: " << time << "s" << std::endl;

    start = clock();

    Eigen::MatrixXd A(h, w);
    A.setRandom();

    Eigen::MatrixXd B(w, h);
    B.setRandom();

    Eigen::MatrixXd C = A * B; // 5s

    end = clock();

    time = (double)(end - start) / CLOCKS_PER_SEC;

    std::cout << "Time: " << time << "s" << std::endl;
}
