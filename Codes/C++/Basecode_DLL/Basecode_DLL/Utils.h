#pragma once

#include "Functions.h"
#include "Settings.h"

/// @brief Crée un réseau de neurones (architecture dans la fonction).
NN* NN_Create(void);

/// @brief Retire et renvoie le réseau de neurones ayant le score le moins élevé.
NN* Population_RemoveMinimum(void);

/// @brief Retire et renvoie le réseau de neurones ayant le score le plus élevé.
NN* Population_RemoveMaximum(void);

/// @brief Retire et libère tous les réseaux de neurones de la population.
void Population_Clear(void);

/// @brief Renvoie l'entrée 'X' à partir d'un monde pour un réseau de neurones.
Matrix* World_ToInput(int* p_world, int p_w, int p_h);

/// @brief Vérifie si les coordonnées sont hors de la dimension.
bool Coord_OutOfDimension(int p_i, int p_j, int p_w, int p_h);

/// @brief Renvoie l'index d'une coordonnée (i, j) dans un tableau à une dimension.
int Coord_ToIndex(int p_i, int p_j, int p_w);
