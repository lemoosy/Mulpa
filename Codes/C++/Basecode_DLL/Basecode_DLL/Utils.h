#pragma once

#include "Settings.h"
#include "Functions.h"

NN* NN_Create(void);

NN* Population_RemoveMinimum(void);

NN* Population_RemoveMaximum(void);

void Population_Clear(void);


/// @brief Liste des réseaux de neurones.
extern NN** g_nn;

/// @brief Retourne un ID libre depuis la liste 'g_nn'.
/// @return -1 si aucun ID libre, sinon un ID libre.
int gNN_GetEmptyID(void);

/// @brief Retourne à l'emplacement 'p_id' de la liste 'g_nn' le réseau de neurones.
NN* gNN_GetNN(int p_id);

/// @brief Attribue à l'emplacement 'p_id' de la liste 'g_nn' le réseau de neurones 'p_nn'.
void gNN_SetNN(int p_id, NN* p_nn);

/// @brief Détruit à l'emplacement 'p_id' de la liste 'g_nn' le réseau de neurones.
void gNN_DestroyNN(int p_id);
