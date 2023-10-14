using UnityEngine;

using _DLL;
using _Settings;
using _World;
using UnityEngine.Tilemaps;

public class GameSettings : MonoBehaviour
{



    #region Variables



    // #######################
    // ##### à définir ! #####
    // #######################

    // Tilemap pour les murs.
    public Tilemap m_tilemapCopy = null;

    // Objet pour instancier un monde.
    public GameObject m_worldCopy = null;
    
    // Objet pour instancier un joueur.
    public GameObject m_playerCopy = null;

    // #######################
    // ##### à définir ! #####
    // #######################



    // Mode de jeu.
    public Settings.ModeID m_mode = Settings.ModeID.MODE_TRAINING_EASY;


    
    // Taille de la population.
    public int m_populationSize = 200;

    // Taille de la sélection.
    public int m_selectionSize = 50;

    // Nombre d'enfants.
    public int m_childrenSize = 100;

    // Nombre de fois qu'un enfant est muté.
    public int m_mutationRate = 3;

    // Nombre de générations.
    public int m_generation = 0;



    // Mondes.
    public GameObject[] m_worlds;



    // Espacement entre les mondes.
    public float m_worldSpace = 256.0f;



    #endregion



    #region Functions



    void Start()
    {
        Debug.Assert(m_worldCopy);
        Debug.Assert(m_playerCopy);

        m_tilemapCopy.ClearAllTiles();

        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:
            {
                    m_worlds = new GameObject[1];

                    GameObject world = Instantiate(m_worldCopy);
                    GameObject player = Instantiate(m_playerCopy);

                    World worldScr = world.GetComponent<World>();
                    Player playerScr = player.GetComponent<Player>();

                    worldScr.m_player = player;
                    playerScr.m_world = world;
                    
                    worldScr.m_tilemap = m_tilemapCopy;
                    worldScr.m_origin = new Vector2(0.0f, 0.0f);
                    worldScr.m_levels = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                    worldScr.CreateScene();

                    playerScr.m_isAI = false;
                    playerScr.ResetPosition();

                    m_worlds[0] = world;
                    
                    break;
            }

            case Settings.ModeID.MODE_TRAINING_EASY:
            {
                    m_worlds = new GameObject[m_populationSize];

                    DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                    Vector2 origin = new Vector2(0.0f, 0.0f);
                    
                    for (int i = 0; i < m_populationSize; i++)
                    {
                        GameObject world = Instantiate(m_worldCopy);
                        GameObject player = Instantiate(m_playerCopy);

                        World worldScr = world.GetComponent<World>();
                        Player playerScr = player.GetComponent<Player>();

                        worldScr.m_player = player;
                        playerScr.m_world = world;

                        worldScr.m_tilemap = m_tilemapCopy;
                        worldScr.m_origin = origin;
                        worldScr.m_levels = new int[] { 1, 2, 3, 4, 5 };
                        worldScr.CreateScene();

                        playerScr.m_isAI = true;
                        playerScr.m_populationIndex = i;
                        playerScr.ResetPosition();
                        
                        m_worlds[i] = world;

                        origin.x += World.m_w + m_worldSpace;
                    }
                    
                    break;
            }

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            {
                break;
            }

            case Settings.ModeID.MODE_TRAINING_HARD:
            {
                break;
            }
        }
    }

    void Update()
    {
        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:
            break;

            case Settings.ModeID.MODE_TRAINING_EASY:
            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            case Settings.ModeID.MODE_TRAINING_HARD:

                bool res = false;

                for (int i = 0; i < m_populationSize; i++)
                {
                    GameObject world = m_worlds[i];

                    World worldScr = world.GetComponent<World>();
                    Player player = worldScr.m_player.GetComponent<Player>();

                    if (player.m_isDead == false)
                    {
                        res = true;
                        break;
                    }
                }

                if (res == false)
                {
                    m_tilemapCopy.ClearAllTiles();

                    string msg = "[";

                    for (int i = 0; i < m_populationSize; i++)
                    {
                        GameObject world = m_worlds[i];
                        World worldScr = world.GetComponent<World>();
                        Player playerScr = worldScr.m_player.GetComponent<Player>();
                        DLL.DLL_PG_SetScore(i, playerScr.m_score);

                        if (i < 10)
                        {
                            msg += DLL.DLL_PG_GetScore(i).ToString() + ", ";
                        }
                    }

                    print(msg + "]");

                    //DLL.DLL_PG_Update();

                    for (int i = 0; i < m_populationSize; i++)
                    {
                        GameObject world = m_worlds[i];

                        World worldScr = world.GetComponent<World>();
                        worldScr.m_levelsCursor = 0;
                        worldScr.DestroyScene();
                        worldScr.CreateScene();

                        Player playerScr = worldScr.m_player.GetComponent<Player>();
                        playerScr.m_tick = 0;
                        playerScr.ResetPosition();
                        playerScr.ResetItem();
                        playerScr.ResetState();
                        playerScr.m_score = 0.0f;
                    }

                    m_generation++;
                }

            break;
        }
    }

    void OnDestroy()
    {
        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:

                Destroy(m_worlds[0]);

            break;

            case Settings.ModeID.MODE_TRAINING_EASY:
            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            case Settings.ModeID.MODE_TRAINING_HARD:

                DLL.DLL_PG_Quit();

                for (int i = 0; i < m_populationSize; i++)
                {
                    Destroy(m_worlds[i]);
                }

            break;
        }
    }



    #endregion



}
