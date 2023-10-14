using UnityEngine;

using _Matrix;
using _Settings;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

using _DLL;
using _Settings;

using CaseID = _Settings.Settings.CaseID;

namespace _Environment
{
    public class Environment : MonoBehaviour
    {
        #region Variables

        // ############################
        // ########## Joueur ##########
        // ############################

        // Permet de savoir si le joueur est une IA.
        public bool m_isAI = false;

        // Joueur associé au monde.
        public GameObject m_playerCopy = null; // à définir sur Unity.

        // ####################################
        // ########## Monde - Matrix ##########
        // ####################################

        // Taille de la matrice.
        public static Vector2Int m_matrixSize = new Vector2Int(24, 12);

        // Matrice du monde qui s'actualise à chaque frame.
        public int[] m_matrix = new int[m_matrixSize.x * m_matrixSize.y];

        // Position départ/joueur/sortie dans la matrice.
        public Vector2Int m_matrixPositionSpawn = new Vector2Int(-1, -1);
        public Vector2Int m_matrixPositionPlayer = new Vector2Int(-1, -1);
        public Vector2Int m_matrixPositionExit = new Vector2Int(-1, -1);

        // ###########################
        // ########## Monde ##########
        // ###########################

        // Taille d'une tuile.
        public static Vector2Int m_tileSize = new Vector2Int(16, 16);

        // Taille du monde.
        public static Vector2Int m_size = m_matrixSize * m_tileSize;

        // Origine du monde.
        public Vector2 m_origin = new Vector2(0.0f, 0.0f);

        // Tuile pour les murs.
        public Tile m_tileCopy = null; // à définir sur Unity.

        // Grille des tuiles pour les murs.
        public Tilemap m_tilemapCopy = null; // à définir sur Unity.
        
        // Liste de tous les objets de l'environnement.
        public GameObject[] m_objectsCopy = new GameObject[(int)CaseID.CASE_COUNT]; // à définir sur Unity.

        // Niveaux.
        public int[] m_levels;

        // Curseur pour les niveaux.
        public int m_levelsCursor = 0;

        // Liste des objets dans le niveau actuel.
        public LinkedList<GameObject> m_objects = new LinkedList<GameObject>();

        #endregion

        #region Function

        void Start()
        {
            Debug.Assert(m_playerCopy);

            if (m_isAI)
            {
                Player playerScr = m_playerCopy.GetComponent<Player>();
                playerScr.m_isAI = true;
            }

            Debug.Assert(m_tileCopy);
            Debug.Assert(m_tilemapCopy);

            for (int i = 1; i < (int)CaseID.CASE_COUNT; i++) // 0 est null.
            {
                Debug.Assert(m_objectsCopy)
            }
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
