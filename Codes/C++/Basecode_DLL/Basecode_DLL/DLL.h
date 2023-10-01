#pragma once

#include "Settings.h"
#include "Utils.h"

// Fonctions pour le script scr_playerAI.cs

/// @brief Initialise la DLL.
UnityDLL void DLL_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate);

/// @brief Quitte la DLL.
UnityDLL void DLL_Quit(void);

/// @brief Modifie le score d'un réseau de neurones.
UnityDLL void DLL_NN_SetScore(int p_nnIndex, float score);

/// @brief Retourne le score d'un réseau de neurones.
UnityDLL float DLL_NN_GetScore(int p_nnIndex);

/// @brief MAJ de la population (selection, crossover, mutation).
/// @return true si erreur, false sinon.
UnityDLL bool DLL_Population_Update(void);

// Fonctions pour le script scr_player.cs

/// @brief Réalise la propagation avant d'un réseau de neurones à partir d'un monde.
/// @return true si erreur, false sinon.
UnityDLL bool DLL_NN_Forward(int p_nnIndex, int* p_world, int p_w, int p_h);

/// @brief Retourne la sortie d'un réseau de neurones (-1 = erreur, 0 = left, 1 = right, 2 = jump).
UnityDLL int DLL_NN_GetOutput(int p_nnIndex);

/// @brief Retourne la distance entre le joueur et la sortie (PCC).
/// @brief -1 si erreur.
UnityDLL float DLL_World_GetShortestPath(int* p_world, int p_w, int p_h);
