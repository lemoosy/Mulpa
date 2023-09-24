#pragma once

#include "Settings.h"
#include "Variables.h"

/// @brief Liste des réseaux de neurones.
extern NN** g_nn;

/// @brief Recherche dans la liste des réseaux de neurones (g_nn) un ID libre.
/// @return L'ID libre ou -1 si aucun ID n'est libre.
int gNN_GetEmptyID(void);

/// @brief Retourne à partir du tableau 'g_nn' le réseau de neurones correspondant à l'index.
NN* gNN_GetNN(int p_id);

/// @brief Attribut à l'index spécifié le réseau de neurones (si l'index est libre).
void gNN_SetNN(int p_id, NN* p_nn);

/// @brief Détruit un réseau de neurones à l'index spécifié.
void gNN_Destroy(int p_id);

/// @brief Détruit tous les réseaux de neurones.
void gNN_DestroyALL(void);
