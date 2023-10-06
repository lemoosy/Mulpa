#pragma once

#include "Functions.h"
#include "Settings.h"

/// @brief Cr�e un r�seau de neurones (architecture dans la fonction).
NN* NN_Create(void);

/// @brief Retire et renvoie le r�seau de neurones ayant le score le moins �lev�.
NN* Population_RemoveMinimum(void);

/// @brief Retire et renvoie le r�seau de neurones ayant le score le plus �lev�.
NN* Population_RemoveMaximum(void);

/// @brief Retire et lib�re tous les r�seaux de neurones de la population.
void Population_Clear(void);

/// @brief Renvoie l'entr�e 'X' � partir d'un monde pour un r�seau de neurones.
Matrix* World_ToInput(int* p_world, int p_w, int p_h);

/// @brief V�rifie si les coordonn�es sont hors de la dimension.
bool Coord_OutOfDimension(int p_i, int p_j, int p_w, int p_h);

/// @brief Renvoie l'index d'une coordonn�e (i, j) dans un tableau � une dimension.
int Coord_ToIndex(int p_i, int p_j, int p_w);
