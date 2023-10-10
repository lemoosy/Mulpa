using UnityEngine;

using _Matrix;
using _Settings;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using _DLL;

namespace _World
{
    public class World : MonoBehaviour
    {



        #region Variables



        // #######################
        // ##### à définir ! #####
        // #######################

        public Tile myTile = null;

        public Tilemap m_tilemap = null;

        // Tous les objets que peut contenir un monde.
        public GameObject[] m_objectsCopy = new GameObject[(int)Settings.CaseID.CASE_COUNT];

        // Joueur associé au monde.
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



        // Liste des objets dans le niveau actuel.
        public LinkedList<GameObject> m_objects = new LinkedList<GameObject>();



        // Matrice du monde qui s'actualise à chaque frame.
        public int[] m_matrix = new int[m_matrixW * m_matrixH];



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



        #region Function



        // Unity.

        void Start()
        {
        }

        void Update()
        {
            Player playerScr = m_player.GetComponent<Player>();

            if (playerScr.m_isAI)
            {
                if (playerScr.m_isDead)
                {
                    DLL.DLL_PG_SetScore(playerScr.m_populationIndex, playerScr.m_score);
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
                    DestroyScene();
                    CreateScene();
                    playerScr.ResetPosition();
                    playerScr.ResetState();
                }

                if (playerScr.m_collisions[(int)Settings.CaseID.CASE_EXIT])
                {
                    m_levelsCursor++;
                    DestroyScene();
                    CreateScene();
                    playerScr.ResetPosition();
                    playerScr.ResetState();
                }
            }

            UpdateMatrix();
        }



        // Scene.

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

                    Tilemap tilemap = m_tilemap.GetComponent<Tilemap>();
                    Vector3Int position = new Vector3Int(i + (int)(m_origin.x / (float)m_tileW), j, 0);
                    tilemap.SetTile(position, null);

                    switch (value)
                    {
                        case (int)Settings.CaseCharID.CASE_VOID:
                        break;

                        case (int)Settings.CaseCharID.CASE_WALL:
                            {
                                position = new Vector3Int(i + (int)(m_origin.x / (float)m_tileW), j, 0);
                                tilemap.SetTile(position, myTile);

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
                            {
                                Debug.Assert(false);
                            }
                        break;
                    }

                    if (obj)
                    {
                        m_objects.AddLast(obj);
                    }
                }
            }

            Debug.Assert((m_spawnPositionI != -1) && (m_spawnPositionJ != -1));
            Debug.Assert((m_exitPositionI != -1) && (m_exitPositionJ != -1));

            UpdateMatrix();
        }

        public void DestroyScene()
        {
            int size = m_objects.Count;

            for (int i = 0; i < size; i++)
            {
                GameObject obj = m_objects.First.Value;

                if (obj)
                {
                    Destroy(obj);
                }

                m_objects.RemoveFirst();
            }
        }



        // Matrix.

        public void UpdateMatrix()
        {
            // Objects.

            foreach (GameObject obj in m_objects)
            {
                if (obj == null) continue;

                Vector2 position = obj.transform.position;
                
                position -= m_origin;

                int i = (int)((position.x + (float)m_tileW / 2.0f) / (float)m_tileW);
                int j = (int)((position.y + (float)m_tileH / 2.0f) / (float)m_tileH);

                int index = (j * m_matrixW) + i;

                int value = -1;

                switch (obj.tag)
                {
                    case "tag_wall":
                        break;

                    case "tag_attack":
                        value = (int)Settings.CaseID.CASE_ATTACK;
                        break;

                    case "tag_coin":
                        value = (int)Settings.CaseID.CASE_COIN;
                        break;

                    case "tag_exit":
                        value = (int)Settings.CaseID.CASE_EXIT;
                        break;

                    default:
                        break;
                }

                if (value == -1)
                {
                    continue;
                }

                m_matrix[index] = value;
            }

            // Tiles.

            Tilemap tilemap = m_tilemap.GetComponent<Tilemap>();

            for (int j = 0; j < m_matrixH; j++)
            {
                for (int i = 0; i < m_matrixW; i++)
                {
                    Vector3Int pos = new Vector3Int(i + (int)(m_origin.x / (float)m_tileW), j, 0);

                    if (tilemap.GetTile(pos))
                    {
                        int index = (j * m_matrixW) + i;
                        m_matrix[index] = (int)Settings.CaseID.CASE_WALL;
                    }

                }
            }

            // Player.

            Player playerScr = m_player.GetComponent<Player>();

            if (playerScr.OutOfDimension()) return;

            Vector2 positionPlayer = m_player.transform.position;

            positionPlayer -= m_origin;

            int iPlayer = (int)((positionPlayer.x + (float)m_tileW / 2.0f) / (float)m_tileW);
            int jPlayer = (int)((positionPlayer.y + (float)m_tileH / 2.0f) / (float)m_tileH);

            int indexPlayer = (jPlayer * m_matrixW) + iPlayer;

            m_playerPositionI = -1;
            m_playerPositionJ = -1;

            if (iPlayer < 0 || iPlayer >= m_matrixW) return;
            if (jPlayer < 0 || jPlayer >= m_matrixH) return;

            m_matrix[indexPlayer] = (int)Settings.CaseID.CASE_SPAWN;

            m_playerPositionI = iPlayer;
            m_playerPositionJ = jPlayer;
        }



        #endregion



    }
}
