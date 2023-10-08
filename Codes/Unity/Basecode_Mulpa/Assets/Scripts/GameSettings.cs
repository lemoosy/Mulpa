using UnityEngine;

using _DLL;
using _Settings;
using _World;

public class GameSettings : MonoBehaviour
{



    #region Variables



    // #######################
    // ##### à définir ! #####
    // #######################

    // Objet pour instancier un monde.
    public GameObject m_worldCopy = null;
    
    // Objet pour instancier un joueur.
    public GameObject m_playerCopy = null;

    // #######################
    // ##### à définir ! #####
    // #######################



    // Mode de jeu.
    public Settings.ModeID m_mode = Settings.ModeID.MODE_SOLO;


    
    // Taille de la population.
    public const int m_populationSize = 10;

    // Taille de la sélection.
    public const int m_selectionSize = 3;

    // Nombre d'enfants.
    public const int m_childrenSize = 3;

    // Nombre de fois qu'un enfant est muté.
    public const int m_mutationRate = 1;

    // Nombre de générations.
    public int m_generation = 0;



    // Mondes.
    public GameObject[] m_worlds;



    #endregion



    #region Functions



    void Start()
    {
        Debug.Assert(m_worldCopy, "ERROR (2) - GameSettings::Start()");
        Debug.Assert(m_playerCopy, "ERROR (1) - GameSettings::Start()");

        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:
            {
                m_worlds = new GameObject[1];

                GameObject world = Instantiate(m_worldCopy);
                GameObject player = Instantiate(m_playerCopy);

                World worldScr = world.GetComponent<World>();
                worldScr.m_player = player;
                worldScr.m_origin = new Vector2(0.0f, 0.0f);
                worldScr.m_levels = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

                Player playerScr = player.GetComponent<Player>();
                playerScr.m_world = world;
                playerScr.m_isAI = false;

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
                    worldScr.m_player = player;
                    worldScr.m_origin = origin;
                    worldScr.m_levels = new int[] { 1, 2, 3, 4, 5 };

                    Player playerScr = player.GetComponent<Player>();
                    playerScr.m_world = world;
                    playerScr.m_isAI = true;
                    playerScr.m_populationIndex = i;
                    
                    m_worlds[i] = world;

                    origin.x += World.m_w + 10.0f;
                }
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            {
                m_worlds = new GameObject[m_populationSize];

                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                Vector2 origin = new Vector2(0.0f, 0.0f);
                
                for (int i = 0; i < m_populationSize; i++)
                {
                    GameObject world = Instantiate(m_worldCopy);
                    GameObject player = Instantiate(m_playerCopy);

                    World worldScr = world.GetComponent<World>();
                    worldScr.m_player = player;
                    worldScr.m_origin = origin;
                    worldScr.m_levels = new int[] { 6, 7, 8, 9, 10 };

                    Player playerScr = player.GetComponent<Player>();
                    playerScr.m_world = world;
                    playerScr.m_isAI = true;
                    playerScr.m_populationIndex = i;
                    
                    m_worlds[i] = world;

                    origin.x += World.m_w + 10.0f;
                }
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_HARD:
            {
                m_worlds = new GameObject[m_populationSize];

                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);
                
                Vector2 origin = new Vector2(0.0f, 0.0f);
                
                for (int i = 0; i < m_populationSize; i++)
                {
                    GameObject world = Instantiate(m_worldCopy);
                    GameObject player = Instantiate(m_playerCopy);

                    World worldScr = world.GetComponent<World>();
                    worldScr.m_player = player;
                    worldScr.m_origin = origin;
                    worldScr.m_levels = new int[] { 11, 12, 13, 14, 15 };

                    Player playerScr = player.GetComponent<Player>();
                    playerScr.m_world = world;
                    playerScr.m_isAI = true;
                    playerScr.m_populationIndex = i;
                    
                    m_worlds[i] = world;

                    origin.x += World.m_w + 10.0f;
                }
                
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

                    if (worldScr.m_gameOver == false)
                    {
                        res = true;
                        break;
                    }
                }

                if (res == false)
                {
                    DLL.DLL_PG_Update();

                    for (int i = 0; i < m_populationSize; i++)
                    {
                        GameObject world = m_worlds[i];

                        World worldScr = world.GetComponent<World>();
                        worldScr.m_levelsCursor = 0;
                        worldScr.m_gameOver = false;
                        worldScr.DestroyScene();
                        worldScr.CreateScene();
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
