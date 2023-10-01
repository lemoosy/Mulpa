#pragma once

#include "Settings.h"

typedef enum eCaseID
{
	CASE_VOID		=		' ',
	CASE_WALL		=		'W',
	CASE_PLAYER		=		'P',
	CASE_END		=		'E',
	CASE_MONSTER	=		'M'
}CaseID;

inline int World_CoordToID(int i, int j, int w) { return (j * w + i); }

/// @brief Initialise un monde à partir d'un string.
Matrix* World_LoadMatrix(string p_world);

/// @brief Initialise les entrées du réseau de neurones avec le monde.
Matrix* World_ToInputsNN(Matrix* p_world);

DList<int>* World_GetShortestPath(Matrix* p_world, float* distance);
