#pragma once

#include "Functions.h"
#include "Settings.h"

typedef enum eCaseID
{
	CASE_WALL,
	CASE_MONSTER,
	CASE_COIN,
	CASE_PLAYER,
	CASE_EXIT,
	CASE_BUTTON,
	CASE_DOOR,
	CASE_COUNT
}CaseID;

/// @brief Crée un réseau de neurones (architecture dans la fonction).
NN* NN_Create(void);

/// @brief Retire et renvoie le réseau de neurones ayant le score le moins élevé.
NN* Population_RemoveMinimum(void);

/// @brief Retire et renvoie le réseau de neurones ayant le score le plus élevé.
NN* Population_RemoveMaximum(void);

/// @brief Libère la mémoire de tous les réseaux de neurones de la population.
void Population_Clear(void);

/// @brief Renvoie les entrées pour un réseau de neurones.
Matrix* World_ToInput(int* p_world, int w, int h);

bool Coord_OutOfDimension(int i, int j, int w, int h);

int Coord_ToIndex(int i, int j, int w);
