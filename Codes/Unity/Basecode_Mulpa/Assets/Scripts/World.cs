using UnityEngine;

using _Matrix;
using _Settings;
using System.Collections.Generic;

namespace _World
{
    public class World : MonoBehaviour
    {



        #region Variables



        // #######################
        // ##### à définir ! #####
        // #######################

        // Tous les objets que peut contenir un monde.
        public GameObject[] m_objectsCopy = new GameObject[(int)Settings.CaseID.CASE_COUNT];

        // Joueur.
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



        public LinkedList<GameObject> m_objects = new LinkedList<GameObject>();



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

        }

        void Update()
        {
            Player playerScr = m_player.GetComponent<Player>();

            if (playerScr.m_isAI)
            {
                if (playerScr.m_isDead)
                {
                    m_gameOver = true;
                }

                if (playerScr.m_atExit)
                {
                    m_levelsCursor++;
                    DestroyScene();
                    CreateScene();
                    playerScr.ResetPosition();
                    playerScr.ResetState();
                }
            }
            else
            {
                if (playerScr.m_isDead)
                {
                    playerScr.m_isDead = false;
                    DestroyScene();
                    CreateScene();
                    playerScr.ResetPosition();
                }

                if (playerScr.m_atExit)
                {
                    m_levelsCursor++;
                    DestroyScene();
                    CreateScene();
                    playerScr.ResetPosition();
                    playerScr.ResetState();
                }


            }
        }

        public void CreateScene()
        {
            string path = "Assets/Levels/level_" + m_levels[m_levelsCursor].ToString() + ".txt";

            Matrix matrix = new Matrix(m_matrixW, m_matrixH);
            matrix.Import(path);

            for (int j = 0; j < m_matrixH; j++)
            {
                for (int i = 0; i < m_matrixW; i++)
                {
                    int value = matrix.Get(i, j);
                    
                    GameObject obj = null;

                    switch (value)
                    {
                        case (int)Settings.CaseCharID.CASE_VOID:
                        break;

                        case (int)Settings.CaseCharID.CASE_WALL:
                            {
                                int id = (int)Settings.CaseID.CASE_WALL;

                                obj = Instantiate(m_objectsCopy[id], new Vector2(m_origin.x + (i * m_tileW), m_origin.y + (j * m_tileH)), Quaternion.identity);
                            
                                break;
                            }
                        

                        case (int)Settings.CaseCharID.CASE_ATTACK:
                            {
                                int id = (int)Settings.CaseID.CASE_ATTACK;
                        
                                obj = Instantiate(m_objectsCopy[id], new Vector2(m_origin.x + (i * m_tileW), m_origin.y + (j * m_tileH)), Quaternion.identity);
                        
                                break;
                            }

                        case (int)Settings.CaseCharID.CASE_COIN:
                            {
                                int id = (int)Settings.CaseID.CASE_COIN;

                                obj = Instantiate(m_objectsCopy[id], new Vector2(m_origin.x + (i * m_tileW), m_origin.y + (j * m_tileH)), Quaternion.identity);
                                
                                break;
                            }

                        case (int)Settings.CaseCharID.CASE_PLAYER:
                            {
                                int id = (int)Settings.CaseID.CASE_SPAWN;

                            obj = Instantiate(m_objectsCopy[id], new Vector2(m_origin.x + (i * m_tileW), m_origin.y + (j * m_tileH)), Quaternion.identity);

                                m_spawnPositionI = i;
                                m_spawnPositionJ = j;

                                break;
                            }

                        case (int)Settings.CaseCharID.CASE_EXIT:
                            {
                                int id = (int)Settings.CaseID.CASE_EXIT;

                            obj = Instantiate(m_objectsCopy[id], new Vector2(m_origin.x + (i * m_tileW), m_origin.y + (j * m_tileH)), Quaternion.identity);
                                
                                m_exitPositionI = i;
                                m_exitPositionJ = j;
                                
                                break;
                            }

                        default:
                            Debug.Assert(false, "ERROR - World::LoadScene()");
                        break;
                    }

                    if (obj != null)
                    {
                        m_objects.AddLast(obj);
                    }
                }
            }

            Debug.Assert((m_spawnPositionI != -1) && (m_spawnPositionJ != -1));
            Debug.Assert((m_exitPositionI != -1) && (m_exitPositionJ != -1));
        }

        public void DestroyScene()
        {
            int size = m_objects.Count;

            for (int i = 0; i < size; i++)
            {
                GameObject obj = m_objects.First.Value;
                Destroy(obj);
                m_objects.RemoveFirst();
            }
        }
    }
}
