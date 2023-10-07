using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

using _Matrix;

namespace _World
{
    // Enumération des différents types de cases dans un monde.
    public enum CaseID
    {
        CASE_VOID,
        CASE_WALL,
        CASE_ATTACK,
        CASE_COIN,
        CASE_PLAYER,
        CASE_EXIT,
        CASE_COUNT
    }

    public void Load(string p_path)
    {

    }

    // Classe représentant un monde.
    public class World : MonoBehaviour
    {
    
    
    
        #region DLL
    
    
    
        [DllImport("Basecode_DLL.dll")]
        private static extern float DLL_World_GetShortestPath(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2);

        [DllImport("Basecode_DLL.dll")]
        private static extern float DLL_World_GetShortestPathCross(int[] p_world, int p_w, int p_h, int p_i1, int p_j1, int p_i2, int p_j2);
    
    
    
        #endregion
    
    
    
        #region Variables



        // Largeur d'une tuile.
        public const int m_tileWidth = 16;

        // Hauteur d'une tuile.
        public const int m_tileHeight = 16;
        


        // Largeur de la matrice.
        public const int m_matrixWidth = 24;

        // Hauteur de la matrice.
        public const int m_matrixHeight = 14;



        // Taille du monde.
        public Vector2 m_size;



        // Nombre de modèles par difficulté.
        public const int m_modelsCount = 4;



        // Chemin vers les mondes faciles (fichiers TXT).
        private const string m_modelsPathEasy = "Models/Easy/"; 

        // Mondes faciles.
        public Matrix[] m_modelsEasy;



        // Chemin vers les mondes normaux (fichiers TXT).
        private const string m_modelsPathNormal = "Models/Normal/";

        // Mondes normaux.
        public Matrix[] m_modelsNormal;



        // Chemin vers les modèles difficiles (fichiers TXT).
        private const string m_modelsPathHard = "Models/Hard/";

        // Mondes difficiles.
        public Matrix[] m_modelsHard;



        // Monde actuel.
        public Matrix m_current;



        #endregion
    
    
    
        #region Functions
    
    
    
        public void Start()
        {
    
    
            m_size = new Vector2(m_matrixWidth * m_tileWidth, m_matrixHeight * m_tileHeight);
    
            Debug.Assert(m_modelsCount < 10, "ERROR - m_modelsCount >= 10");
    
            m_modelsEasy = new Matrix[m_modelsCount];
    
            for (int i = 0; i < m_modelsCount; i++)
            {
                m_modelsEasy[i] = new Matrix(m_modelsPathEasy + i.ToString() + ".txt");
            }
    
            m_modelsNormal = new Matrix[m_modelsCount];
    
            for (int i = 0; i < m_modelsCount; i++)
            {
                m_modelsNormal[i] = new Matrix(m_modelsPathNormal + i.ToString() + ".txt");
            }
    
            m_modelsHard = new Matrix[m_modelsCount];
    
            for (int i = 0; i < m_modelsCount; i++)
            {
                m_modelsEasy[i] = new Matrix(m_modelsPathEasy + i.ToString() + ".txt");
            }
    
    
        }
    
        public void Update()
        {
            
        }
    

        public Matrix SceneToMatrix()
        {
            Matrix res = new Matrix(m_matrixWidth, m_matrixHeight);
    
    
    
            return res;
        }
    
        public void Load(Matrix p_world)
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
            Matrix res = new Matrix(m_matrixWidth, m_matrixHeight, (int)CaseID.CASE_VOID, (int)CaseID.CASE_COUNT);

            // ...

            return res;
        }
    
    
    
        #endregion
    
    
    
    }
}
