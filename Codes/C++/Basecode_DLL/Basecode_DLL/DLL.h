#pragma once

#include "Functions.h"
#include "Settings.h"
#include "Utils.h"

// Fonctions pour le script scr_playerAI.cs

/// @brief Initialise la DLL.
UnityDLL void DLL_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate);

/// @brief Quitte la DLL.
UnityDLL void DLL_Quit(void);

/// @brief Modifie le score d'un r�seau de neurones.
UnityDLL void DLL_NN_SetScore(int p_nnIndex, float score);

/// @brief Retourne le score d'un r�seau de neurones.
UnityDLL float DLL_NN_GetScore(int p_nnIndex);

/// @brief Met � jour la population (s�lection, croisement, mutation).
UnityDLL int DLL_Population_Update();

// Fonctions pour le script scr_playerAI.cs

/// @brief R�alise la propagation avant d'un r�seau de neurones.
UnityDLL bool DLL_NN_Forward(int p_id, int* p_world, int w, int h);

/// @brief Retourne la sortie d'un r�seau de neurones (0 = left, 1 = right, 2 = jump).
UnityDLL int DLL_NN_GetOutput(int p_nnIndex);

/// @brief Retourne la distance entre le joueur et la sortie (PCC).
UnityDLL float DLL_World_GetShortestPath(int* p_world, int w, int h);
