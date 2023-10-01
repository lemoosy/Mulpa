#pragma once

#include "Settings.h"
#include "World.h"
#include "Utils.h"
#include "Functions.h"

// Fonctions pour le script scr_playerAI.cs

/// @brief Initialise la DLL.
UnityDLL void DLL_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate);

/// @brief Quitte la DLL.
UnityDLL void DLL_Quit(void);

/// @brief Modifie le score d'un réseau de neurones.
UnityDLL void DLL_NN_SetScore(int p_nnIndex, float score);

/// @brief Retourne le score d'un réseau de neurones.
UnityDLL float DLL_NN_GetScore(int p_nnIndex);

/// @brief Met à jour la population (sélection, croisement, mutation).
UnityDLL int DLL_Population_Update();

// Fonctions pour le script scr_playerAI.cs

/// @brief Réalise la propagation avant d'un réseau de neurones.
UnityDLL void NN_Forward(int p_id, string p_world);

/// @brief Retourne la sortie d'un réseau de neurones (0 = left, 1 = right, 2 = jump).
UnityDLL int NN_GetOutput(int p_id);
