#pragma once

#include "Settings.h"
#include "World.h"
#include "gNN.h"
#include "Functions.h"

NN** g_nn = nullptr;


// 
// 
///// @brief Crée un réseau de neurones.
///// @return L'ID du réseau de neurones ou -1 si erreur.
//GameMakerDLL double NN_Create();
//
///// @return Détruit un réseau de neurones.
///// @return EXIT_FAILURE si erreur, sinon EXIT_SUCCESS.
//GameMakerDLL double NN_Destroy(double p_nnID);
//
//GameMakerDLL double NN_Forward(double p_nnID, char* p_world);
//
//GameMakerDLL double NN_GetOutput(double p_nnID);
//
