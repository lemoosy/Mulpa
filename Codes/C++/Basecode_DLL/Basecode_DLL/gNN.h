#pragma once

#include "Settings.h"
#include "Variables.h"

/// @brief Liste des r�seaux de neurones.
extern NN** g_nn;

/// @brief Recherche dans la liste des r�seaux de neurones (g_nn) un ID libre.
/// @return L'ID libre ou -1 si aucun ID n'est libre.
int gNN_GetEmptyID(void);

/// @brief Retourne � partir du tableau 'g_nn' le r�seau de neurones correspondant � l'index.
NN* gNN_GetNN(int p_id);

/// @brief Attribut � l'index sp�cifi� le r�seau de neurones (si l'index est libre).
void gNN_SetNN(int p_id, NN* p_nn);

/// @brief D�truit un r�seau de neurones � l'index sp�cifi�.
void gNN_Destroy(int p_id);

/// @brief D�truit tous les r�seaux de neurones.
void gNN_DestroyALL(void);
