#pragma once

#include "Settings.h"
#include "World.h"
#include "gNN.h"
#include "Functions.h"
 
NN** g_nn = nullptr;

/// @brief Initialise la DLL.
UnityDLL double DLL_Init(double p_window);

/// @brief Quitte la DLL.
UnityDLL double DLL_Free();

/// @brief Crée un réseau de neurones.
/// @return L'ID du réseau de neurones ou -1 si erreur.
UnityDLL double NN_Create();

/// @return Détruit un réseau de neurones.
/// @return EXIT_FAILURE si erreur, sinon EXIT_SUCCESS.
UnityDLL double NN_Destroy(double p_nnID);

UnityDLL double NN_Forward(double p_nnID, char* p_world);

UnityDLL double NN_GetOutput(double p_nnID);
