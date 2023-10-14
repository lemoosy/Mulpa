#pragma once

#include "Settings.h"
#include "Utils.h"

/// @brief Initialise les varialbes globales pour la PG.
UnityDLL void DLL_PG_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate);

/// @brief Libère les variables globales pour la PG.
UnityDLL void DLL_PG_Quit(void);

/// @brief Modifie le score d'un réseau de neurones.
UnityDLL void DLL_PG_SetScore(int p_populationIndex, float p_score);

/// @brief Retourne le score d'un réseau de neurones.
UnityDLL float DLL_PG_GetScore(int p_populationIndex);

/// @brief MAJ de la population (selection, crossover, mutation).
UnityDLL void DLL_PG_Update(void);

/// @brief Réalise la propagation avant d'un réseau de neurones à partir d'un monde.
UnityDLL void DLL_PG_Forward(int p_populationIndex, int* p_world, int p_w, int p_h);

/// @brief Retourne la sortie d'un réseau de neurones (0 = left, 1 = right, 2 = jump).
UnityDLL int DLL_PG_GetOutput(int p_populationIndex);

/// @brief Retourne le plus court chemin (la distance) entre (p_i1, p_j1) et (p_i2, p_j2).
/// @param p_cross true si on peut traverser les murs, false sinon.
UnityDLL float DLL_PCC(int* p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2, bool p_cross);
