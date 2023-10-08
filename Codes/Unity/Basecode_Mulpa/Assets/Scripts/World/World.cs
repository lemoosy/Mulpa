using UnityEngine;

using _Matrix;

namespace _World
{
    public class World : MonoBehaviour
    {



        // Taille d'une tuile.
        public const int m_tileW = 16;
        public const int m_tileH = 16;
    


        // Taille de la matrice.
        public const int m_matrixW = 24;
        public const int m_matrixH = 14;



        // Taille du monde.
        public const int m_w = m_matrixW * m_tileW;
        public const int m_h = m_matrixH * m_tileH;



        // Origine du monde.
        public Vector2 m_origin = Vector2.zero;

        // Index des niveaux.
        public int[] m_levels = null;

        // Objet joueur.
        public GameObject m_player = null;



        // Index du niveau actuel.
        public int m_levelsCursor = 0;



        // Position de départ du joueur.
        public int m_spawnPositionI = -1;
        public int m_spawnPositionJ = -1;



        // Position du joueur.
        public int m_playerPositionI = -1;
        public int m_playerPositionJ = -1;



        // Position de la sortie.
        public int m_exitPositionI = -1;
        public int m_exitPositionJ = -1;



        void Start()
        {
            Debug.Assert(m_player, "ERROR - World::Start()");

            LoadScene();
        }

        void Update()
        {
            Player playerScript = m_player.GetComponent<Player>();

            if (playerScript.m_atExit)
            {

            }
        }

        public void LoadScene()
        {

        }

        public bool IsOver()
        {

        }


    }
}
