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

        // Tuile pour les murs.
        public Tile m_tile = null;

        // Tilemap pour les murs.
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
                if (playerScr.m_atExit)
                {
                    m_levelsCursor++;
                    DestroyScene();
                    CreateScene();
                    playerScr.m_tick = 0;
                    playerScr.ResetPosition();
                    playerScr.ResetState();
                }
            }
            else
            {
            }

            UpdateMatrix();
        }



        // Scene.

        public void CreateScene()
        {
            string path = "Assets/Levels/level_" + m_levels[m_levelsCursor].ToString() + ".txt";

            Matrix matrix = new Matrix(m_matrixW, m_matrixH);
            matrix.Import(path);

            Tilemap tilemapScr = m_tilemap.GetComponent<Tilemap>();

            for (int j = 0; j < m_matrixH; j++)
            {
                for (int i = 0; i < m_matrixW; i++)
                {
                    int value = matrix.Get(i, j);
                    
                    GameObject obj = null;

                    int _i = (int)m_origin.x / m_tileW + i;
                    int _j = (int)m_origin.y / m_tileH + j;

                    Vector3Int pos = new Vector3Int(_i, _j, 0);
                    tilemapScr.SetTile(pos, null);

                    switch (value)
                    {
                        case ' ':
                        break;

                        case 'O':
                            {
                                _i = (int)m_origin.x / m_tileW + i;
                                _j = (int)m_origin.y / m_tileH + j;

                                pos = new Vector3Int(_i, _j, 0);
                                tilemapScr.SetTile(pos, m_tile);

                                break;
                            }

                        case '!':
                            {
                                int id = (int)Settings.CaseID.CASE_ATTACK;

                                Vector2 position = new Vector2(i * m_tileW, j * m_tileH);
                                position += m_origin;

                                obj = Instantiate(m_objectsCopy[id], position, Quaternion.identity);
                        
                                break;
                            }

                        case '.':
                            {
                                int id = (int)Settings.CaseID.CASE_COIN;

                                Vector2 position = new Vector2(i * m_tileW, j * m_tileH);
                                position += m_origin;

                                obj = Instantiate(m_objectsCopy[id], position, Quaternion.identity);
                                
                                break;
                            }

                        case 'A':
                            {
                                int id = (int)Settings.CaseID.CASE_SPAWN;

                                Vector2 position = new Vector2(i * m_tileW, j * m_tileH);
                                position += m_origin;

                                obj = Instantiate(m_objectsCopy[id], position, Quaternion.identity);

                                m_spawnPositionI = i;
                                m_spawnPositionJ = j;

                                break;
                            }

                        case 'B':
                            {
                                int id = (int)Settings.CaseID.CASE_EXIT;

                                Vector2 position = new Vector2(i * m_tileW, j * m_tileH);
                                position += m_origin;

                                obj = Instantiate(m_objectsCopy[id], position, Quaternion.identity);
                                
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
            // Tiles.

            Tilemap tilemapScr = m_tilemap.GetComponent<Tilemap>();

            for (int j = 0; j < m_matrixH; j++)
            {
                for (int i = 0; i < m_matrixW; i++)
                {
                    int _i = (int)m_origin.x / m_tileW + i;
                    int _j = (int)m_origin.y / m_tileH + j;

                    Vector3Int pos = new Vector3Int(_i, _j, 0);

                    if (tilemapScr.GetTile(pos))
                    {
                        int index = (j * m_matrixW) + i;

                        m_matrix[index] = (int)Settings.CaseID.CASE_WALL;
                    }
                }
            }

            // Objects.

            foreach (GameObject obj in m_objects)
            {
                if (!obj)
                {
                    continue;
                }

                Vector2 position = obj.transform.position;
                position -= m_origin;

                float x = position.x + (float)m_tileW / 2.0f;
                float y = position.y + (float)m_tileH / 2.0f;

                int i = (int)(x / (float)m_tileW);
                int j = (int)(y / (float)m_tileH);

                if ((i < 0) || (i >= m_matrixW) || (j < 0) || (j >= m_matrixH))
                {
                    continue;
                }

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

                    case "tag_spawn":
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

            // Player.
            {
                Player playerScr = m_player.GetComponent<Player>();

                m_playerPositionI = -1;
                m_playerPositionJ = -1;

                Vector2 position = m_player.transform.position;
                position -= m_origin;

                float x = position.x + (float)m_tileW / 2.0f;
                float y = position.y + (float)m_tileH / 2.0f;

                int i = (int)(x / (float)m_tileW);
                int j = (int)(y / (float)m_tileH);

                if ((i < 0) || (i >= m_matrixW) || (j < 0) || (j >= m_matrixH))
                {
                    return;
                }

                int index = (j * m_matrixW) + i;

                m_matrix[index] = (int)Settings.CaseID.CASE_SPAWN;

                m_playerPositionI = i;
                m_playerPositionJ = j;
            }
        }



        #endregion



    }
}
