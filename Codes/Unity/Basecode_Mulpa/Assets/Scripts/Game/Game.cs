using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

using _Matrix;
using _World;
using System;

namespace _Game
{
    // Classe représentant un monde.
    public class Game : MonoBehaviour
    {
    

    
        #region Variables

        

        // Origine du monde.
        public Vector2 m_origin;

        // Mondes.
        public int[] m_worlds;

        // Index du monde actuel.
        public int m_worldsCursor = 0;

        // Joueur.
        public GameObject m_player;



        // Position (i, j) du joueur dans la matrice du monde.
        public int m_playerPositionI = -1;
        public int m_playerPositionJ = -1;

        // Position (i, j) de la sortie dans la matrice du monde.
        public int m_exitPositionI = -1;
        public int m_exitPositionJ = -1;



        #endregion



        #region Functions

        

        public Game(Vector2 p_origin, int[] p_worlds, GameObject p_player)
        {
            int a = World.m_tileH;

            m_origin = p_origin;

            m_worlds = p_worlds;
        
            UpdateWorld();

            m_player = Instantiate(p_player); // TODO
        }

        public void Next()
        {
            m_matrixCursor++;

            Debug.Assert(m_matrixCursor < m_matrixCount);

            LoadScene(m_matrixCursor);
        }
    
        public void Update()
        {
            UpdateWorld();
        }

        public void LoadScene(int p_index)
        {
            
        }

        // Calcule le PCC entre (p_i1, p_j1) et (p_i2, p_j2).
        // Retourne -1.0f si le PCC n'existe pas.
        public float ShortestPath(int p_i1, int p_j1, int p_i2, int p_j2)
        {
            // ...

            return 0.0f;
        }

        // Calcule le PCC entre (p_i1, p_j1) et (p_i2, p_j2) en prennant en compte les murs.
        public float ShortestPathCross(int p_i1, int p_j1, int p_i2, int p_j2)
        {
            // ...

            return 0.0f;
        }
    
        // Génère un monde aléatoire.
        public Matrix GenWorld()
        {
            Matrix res = new Matrix(m_matrixW, m_matrixH, (int)CaseID.CASE_VOID, (int)CaseID.CASE_COUNT);

            // ...

            return res;
        }
    
    

        public void UpdateWorld()
        {

        }

    
        #endregion
    
    
    
    }
}
