#pragma once

#include "Graph.h"

#define UnityDLL extern "C" __declspec (dllexport)

enum eCaseIDBin
{
    CASE_VOID      =   0,
    CASE_BLOCK     =   1,
    CASE_DANGER    =   2
};

bool OutOfDimension(int p_i, int p_j, int p_w, int p_h)
{
    return ((p_i < 0) || (p_i >= p_w) || (p_j < 0) || (p_j >= p_h));
}

int CoordToID(int p_i, int p_j, int p_w)
{
    return (p_j * p_w + p_i);
}

/// @brief Retourne la distance du Plus Court Chemin entre (p_i1, p_j1) et (p_i2, p_j2).
/// @param p_cross true si on veut traverser les murs/monstres, false sinon.
UnityDLL float GetSP(int* p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross, float p_crossValue);
