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

/// @brief Cr�e un r�seau de neurones.
/// @return L'ID du r�seau de neurones ou -1 si erreur.
UnityDLL double NN_Create();

/// @return D�truit un r�seau de neurones.
/// @return EXIT_FAILURE si erreur, sinon EXIT_SUCCESS.
UnityDLL double NN_Destroy(double p_nnID);

UnityDLL double NN_Forward(double p_nnID, char* p_world);

UnityDLL double NN_GetOutput(double p_nnID);
