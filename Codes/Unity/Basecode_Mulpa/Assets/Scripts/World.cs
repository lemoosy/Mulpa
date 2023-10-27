using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using _Matrix;
using _Settings;

// Classe représentant un monde.
// Un monde représente plusieurs niveaux.
// Un monde possède un joueur unique.
public class World : MonoBehaviour
{





    #region Variables





    //////////////
    /// Joueur ///
    //////////////

    // Joueur associé au monde (depuis la liste m_objects).
    public GameObject m_player = null;





    //////////////
    /// Niveau ///
    //////////////

    // Difficulté des niveaux.
    //
    //      EASY -> m_levelIndex = [1, 2, 3, 4]
    //    MEDIUM -> m_levelIndex = [4, 5, 6, 7]
    //      HARD -> m_levelIndex = [7, 8, 9, 10]
    //       ALL -> m_levelIndex = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    //
    public Settings.Difficulty m_levelDifficulty = Settings.Difficulty.EASY;

    // Liste représentant les indices des niveaux (level_X.txt, X = m_levelIndex[i], X in [1, 10]).
    [HideInInspector]
    public int[] m_levelIndex = null;

    // Curseur niveau actuel (level_X.txt, X = m_levelIndex[m_levelCursor], X in [1, 10]).
    [HideInInspector]
    public int m_levelCursor = 0;





    ///////////////
    /// Matrice ///
    ///////////////

    // Taille de la matrice.
    [HideInInspector]
    public static Vector2Int s_matrixSize = new Vector2Int(24, 14);

    // Matrice du niveau actuel.
    // Elle représente le fichier level_X.txt avec X = m_levelIndex[m_levelCursor].
    // Chaque case est un CaseIDChar.
    // Se met à jour dans CreateLevel().
    [HideInInspector]
    public Matrix m_matrixChar = new Matrix(s_matrixSize.x, s_matrixSize.y);

    // Matrice du niveau actuel (optimisé pour l'agent).
    // Elle représente les tuiles + tous les objets du niveau actuel.
    // Chaque case est un CaseIDBin.
    // Se met à jour dans MatrixBinUpdate().
    [HideInInspector]
    public Matrix m_matrixBin = new Matrix(s_matrixSize.x, s_matrixSize.y);





    /////////////
    /// Tuile ///
    /////////////

    // Taille d'une tuile.
    [HideInInspector]
    public static Vector2Int s_tileSize = new Vector2Int(16, 16);

    // Carte des tuiles.
    public Tilemap m_tileMap = null;

    // Tuile d'un mur.
    public Tile m_tileWall = null;





    /////////////
    /// Objet ///
    /////////////

    // Préfabriqués des objets.
    public GameObject m_doorPrefab = null;
    public GameObject m_monsterPrefab = null;
    public GameObject m_coinPrefab = null;
    public GameObject m_spawnPrefab = null;
    public GameObject m_leverPrefab = null;
    public GameObject m_exitPrefab = null;

    // Liste de tous les objets du niveau actuel.
    // Se met à jour dans CreateLevel().
    [HideInInspector]
    public List<GameObject> m_objects = new List<GameObject>();
    
    // Objet spawn (depuis la liste m_objects).
    [HideInInspector]
    public GameObject m_spawn = null;

    // Objet lever (depuis la liste m_objects).
    [HideInInspector]
    public GameObject m_lever = null;
    
    // Objet exit (depuis la liste m_objects).
    [HideInInspector]
    public GameObject m_exit = null;





    /////////////
    /// Autre ///
    /////////////

    // Taille du monde.
    [HideInInspector]
    public static Vector2Int s_size = s_tileSize * s_matrixSize;

    // Position de l'objet parent (environnement).
    public Transform m_environment = null;





    #endregion





    #region Functions





    public void Init()
    {
        switch (m_levelDifficulty)
        {
            case Settings.Difficulty.EASY:
                m_levelIndex = new int[] { 1, 2, 3, 4 };
                break;

            case Settings.Difficulty.MEDIUM:
                m_levelIndex = new int[] { 4, 5, 6, 7 };
                break;

            case Settings.Difficulty.HARD:
                m_levelIndex = new int[] { 7, 8, 9, 10 };
                break;

            case Settings.Difficulty.ALL:
                m_levelIndex = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                break;

            default:
                Debug.Assert(false);
                break;
        }

        m_objects.Add(m_player);
    }

    public void DestroyDoor()
    {
        foreach (GameObject obj in m_objects)
        {
            if (obj.tag == "tag_door")
            {
                Destroy(obj);
            }
        }
    }





    //////////////
    /// Niveau ///
    //////////////

    public void CreateLevel()
    {
        // m_matrixChar

        int level = m_levelIndex[m_levelCursor];

        string path = "Assets/Levels/level_" + level.ToString() + ".txt";

        m_matrixChar.Import(path);

        // m_objects

        Tilemap tileMapScr = m_tileMap.GetComponent<Tilemap>();

        bool existDoor = false;

        for (int j = 0; j < s_matrixSize.y; j++)
        {
            for (int i = 0; i < s_matrixSize.x; i++)
            {
                int value = m_matrixChar.Get(i, j);

                GameObject obj = null;

                Vector2Int positionIJ = new Vector2Int(i, j);

                // Suppression des tuiles.

                tileMapScr.SetTile((Vector3Int)positionIJ, null);

                // Création des objets.

                switch (value)
                {
                    case (int)Settings.CaseIDChar.CASE_VOID:
                        break;

                    case (int)Settings.CaseIDChar.CASE_LIGHT:
                        break;

                    case (int)Settings.CaseIDChar.CASE_WALL:
                        tileMapScr.SetTile((Vector3Int)positionIJ, m_tileWall);
                        break;

                    case (int)Settings.CaseIDChar.CASE_DOOR:
                        obj = Instantiate(m_doorPrefab, m_environment);
                        existDoor = true;
                        break;

                    case (int)Settings.CaseIDChar.CASE_MONSTER:
                        obj = Instantiate(m_monsterPrefab, m_environment);
                        break;

                    case (int)Settings.CaseIDChar.CASE_SPADE:
                        obj = Instantiate(m_monsterPrefab, m_environment);
                        break;

                    case (int)Settings.CaseIDChar.CASE_LAVA:
                        obj = Instantiate(m_monsterPrefab, m_environment);
                        break;

                    case (int)Settings.CaseIDChar.CASE_COIN:
                        obj = Instantiate(m_coinPrefab, m_environment);
                        break;

                    case (int)Settings.CaseIDChar.CASE_SPAWN:
                        Debug.Assert(!m_spawn);
                        obj = Instantiate(m_spawnPrefab, m_environment);
                        m_spawn = obj;
                        break;

                    case (int)Settings.CaseIDChar.CASE_LEVER:
                        Debug.Assert(!m_lever);
                        obj = Instantiate(m_leverPrefab, m_environment);
                        m_lever = obj;
                        break;

                    case (int)Settings.CaseIDChar.CASE_EXIT:
                        Debug.Assert(!m_exit);
                        obj = Instantiate(m_exitPrefab, m_environment);
                        m_exit = obj;
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }

                // Ajout de l'objet dans la liste m_objects.

                if (obj)
                {
                    Vector2 distance = new Vector2(0.5f, 0.5f);
                    Vector2 position = (Vector2)positionIJ + distance;
                    obj.transform.localPosition = (Vector3)position;
                    m_objects.Add(obj);
                }
            }
        }

        Debug.Assert(m_spawn);
        Debug.Assert((m_lever && existDoor) || (!m_lever && !existDoor));
        Debug.Assert(m_exit);
    }

    public void DestroyLevel()
    {
        foreach (GameObject obj in m_objects)
        {
            if (!obj || (obj == m_player))
            {
                continue;
            }

            Destroy(obj);
        }

        m_objects.Clear();
     
        m_spawn = null;
        m_lever = null;
        m_exit = null;
    }





    ///////////////
    /// Matrice ///
    ///////////////

    public void MatrixBinClear()
    {
        for (int j = 0; j < s_matrixSize.y; j++)
        {
            for (int i = 0; i < s_matrixSize.x; i++)
            {
                int value = (int)Settings.CaseIDBin.CASE_VOID;

                m_matrixBin.Set(i, j, value);
            }
        }
    }

    public void MatrixBinUpdate()
    {
        MatrixBinClear();

        // Tuiles

        Tilemap tileMapScr = m_tileMap.GetComponent<Tilemap>();

        for (int j = 0; j < s_matrixSize.y; j++)
        {
            for (int i = 0; i < s_matrixSize.x; i++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);

                if (tileMapScr.GetTile(position))
                {
                    int value = (int)Settings.CaseIDBin.CASE_WALL;

                    m_matrixBin.Set(i, j, value);
                }
            }
        }

        // Objets

        foreach (GameObject obj in m_objects)
        {
            if (!obj)
            {
                continue;
            }

            Vector2 distance = new Vector2(0.5f, 0.5f);
            Vector2 position = (Vector2)obj.transform.localPosition;
            position -= distance;

            int i = (int)position.x;
            int j = (int)position.y;

            if ((i < 0) || (i >= s_matrixSize.x) || (j < 0) || (j >= s_matrixSize.y))
            {
                continue;
            }

            // On évite d'écraser le joueur.
            if (m_matrixBin.Get(i, j) == (int)Settings.CaseIDBin.CASE_PLAYER)
            {
                continue;
            }

            int value = -1;

            switch (obj.tag)
            {
                case "tag_light":
                    value = (int)Settings.CaseIDBin.CASE_LIGHT;
                    break;

                case "tag_wall":
                    value = (int)Settings.CaseIDBin.CASE_WALL;
                    break;

                case "tag_door":
                    value = (int)Settings.CaseIDBin.CASE_DOOR;
                    break;

                case "tag_monster":
                    value = (int)Settings.CaseIDBin.CASE_MONSTER;
                    break;

                case "tag_spade":
                    value = (int)Settings.CaseIDBin.CASE_SPADE;
                    break;

                case "tag_lava":
                    value = (int)Settings.CaseIDBin.CASE_LAVA;
                    break;

                case "tag_coin":
                    value = (int)Settings.CaseIDBin.CASE_COIN;
                    break;

                case "tag_spawn":
                    value = (int)Settings.CaseIDBin.CASE_SPAWN;
                    break;

                case "tag_lever":
                    value = (int)Settings.CaseIDBin.CASE_LEVER;
                    break;

                case "tag_exit":
                    if (m_lever) value = (int)Settings.CaseIDBin.CASE_VOID;
                    else value = (int)Settings.CaseIDBin.CASE_EXIT;
                    break;

                case "tag_player":
                    value = (int)Settings.CaseIDBin.CASE_PLAYER;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            m_matrixBin.Set(i, j, value);
        }
    }





    //////////////////
    /// Générateur ///
    //////////////////

    public Matrix GenerateRandom()
    {
        Matrix matrix = new Matrix(s_matrixSize.x, s_matrixSize.y);
        
        // ...
        
        return matrix;
    }





    #endregion





}
