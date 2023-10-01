#pragma once

#include "Settings.h"
#include "Functions.h"

NN* NN_Create(void);

NN* Population_RemoveMinimum(void);

NN* Population_RemoveMaximum(void);

void Population_Clear(void);


/// @brief Liste des r�seaux de neurones.
extern NN** g_nn;

/// @brief Retourne un ID libre depuis la liste 'g_nn'.
/// @return -1 si aucun ID libre, sinon un ID libre.
int gNN_GetEmptyID(void);

/// @brief Retourne � l'emplacement 'p_id' de la liste 'g_nn' le r�seau de neurones.
NN* gNN_GetNN(int p_id);

/// @brief Attribue � l'emplacement 'p_id' de la liste 'g_nn' le r�seau de neurones 'p_nn'.
void gNN_SetNN(int p_id, NN* p_nn);

/// @brief D�truit � l'emplacement 'p_id' de la liste 'g_nn' le r�seau de neurones.
void gNN_DestroyNN(int p_id);
