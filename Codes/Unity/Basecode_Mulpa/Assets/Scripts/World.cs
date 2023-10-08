using UnityEngine;

using _Matrix;
using _Settings;

namespace _World
{
    public class World : MonoBehaviour
    {



        #region Variables



        // #######################
        // ##### à définir ! #####
        // #######################

        // Joueur dans le monde.
        public GameObject m_player = null;

        // Origine du monde.
        public Vector2 m_origin;

        // Niveaux.
        public int[] m_levels;

        // #######################
        // ##### à définir ! #####
        // #######################



        // Taille d'une tuile.
        public const int m_tileW = 16;
        public const int m_tileH = 16;
    


        // Taille de la matrice.
        public const int m_matrixW = 24;
        public const int m_matrixH = 14;



        // Taille du monde.
        public const int m_w = m_matrixW * m_tileW;
        public const int m_h = m_matrixH * m_tileH;



        // Curseur pour les niveaux.
        public int m_levelsCursor = 0;



        // Matrice du monde qui s'actualise à chaque frame pour l'entrée du réseau de neurones du joueur.
        public int[] m_matrixForNN;

        // Matrice du monde pour calculer un PCC.
        public int[] m_matrixForPCC;



        // Position de départ du joueur.
        public int m_spawnPositionI = -1;
        public int m_spawnPositionJ = -1;



        // Position du joueur.
        public int m_playerPositionI = -1;
        public int m_playerPositionJ = -1;



        // Position de la sortie.
        public int m_exitPositionI = -1;
        public int m_exitPositionJ = -1;



        // Vérifie si la partie est terminée pour ce monde.
        public bool m_gameOver = false;



        #endregion



        void Start()
        {
            Debug.Assert(m_player, "ERROR (1) - World::Start()");
            Debug.Assert(m_levels.Length > 0, "ERROR (2) - World::Start()");

            CreateScene();
        }

        void Update()
        {
            Player playerScr = m_player.GetComponent<Player>();

            if (playerScr.m_isAI)
            {


                if (playerScr.m_isDead)
                {

                }
            }
        }

        public void CreateScene()
        {
            string path = "Levels/level_" + m_levels[m_levelsCursor].ToString() + ".txt";

            Matrix matrix = new Matrix(m_matrixW, m_matrixH);
            matrix.Import(path);

            for (int j = 0; j < m_matrixH; j++)
            {
                for (int i = 0; i < m_matrixW; i++)
                {
                    int value = matrix.Get(i, j);

                    switch (value)
                    {
                        case (int)Settings.CaseID.CASE_VOID:
                        break;

                        case (int)Settings.CaseID.CASE_WALL:
                        break;

                        case (int)Settings.CaseID.CASE_ATTACK:
                        break;

                        case (int)Settings.CaseID.CASE_COIN:
                        break;

                        case (int)Settings.CaseID.CASE_PLAYER: // CASE_SPAWN
                            m_spawnPositionI = i;
                            m_spawnPositionJ = j;
                        break;

                        case (int)Settings.CaseID.CASE_EXIT:
                            m_exitPositionI = i;
                            m_exitPositionJ = j;
                        break;

                        default:
                            Debug.Assert(false, "ERROR - World::LoadScene()");
                        break;
                    }
                }
            }
        }

        public void DestroyScene()
        {

        }
    }
}
