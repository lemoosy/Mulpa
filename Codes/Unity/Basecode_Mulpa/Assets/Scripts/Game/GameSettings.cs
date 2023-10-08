using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

using _DLL;
using _Settings;
using _World;

public class GameSettings : MonoBehaviour
{



    #region Variables



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

    // Curseur pour la liste de population.
    public int m_populationCursor = 0;

    // Nombre de générations.
    public int m_generation = 0;



    // Objet pour instancier un joueur.
    public GameObject m_playerCopy = null;

    // Objets pour instancier un monde.
    public GameObject m_worldCopy = null;

    // Mondes.
    public GameObject[] m_worlds = null;



    #endregion



    #region Fonctions



    void Start()
    {
        Debug.Assert(m_playerCopy, "ERROR (1) - GameSettings::Start()");
        Debug.Assert(m_worldCopy, "ERROR (2) - GameSettings::Start()");

        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:
            {
                GameObject player = Instantiate(m_playerCopy);
                Player playerScr = player.GetComponent<Player>();
                playerScr.m_isAI = false;
                
                GameObject world = Instantiate(m_worldCopy);
                World worldScr = player.GetComponent<World>();
                worldScr.m_origin = new Vector2(0.0f, 0.0f);
                worldScr.m_levels = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                worldScr.m_player = Instantiate(m_playerCopy);

                m_worlds[0] = world;
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_EASY:
            {
                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                Vector2 origin = new Vector2(0.0f, 0.0f);
                
                for (int i = 0; i < m_populationSize; i++)
                {
                    GameObject player = Instantiate(m_playerCopy);
                    Player playerScr = player.GetComponent<Player>();
                    playerScr.m_isAI = true;
                    playerScr.m_populationIndex = i;

                    GameObject world = Instantiate(m_worldCopy);
                    World worldScr = player.GetComponent<World>();
                    worldScr.m_origin = origin;
                    worldScr.m_levels = new int[] { 1, 2, 3, 4, 5 };
                    worldScr.m_player = Instantiate(m_playerCopy);

                    m_worlds[i] = world;

                    origin.x += World.m_w + 10.0f;
                }
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            {
                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                Vector2 origin = new Vector2(0.0f, 0.0f);
                
                for (int i = 0; i < m_populationSize; i++)
                {
                    GameObject player = Instantiate(m_playerCopy);
                    Player playerScr = player.GetComponent<Player>();
                    playerScr.m_isAI = true;
                    playerScr.m_populationIndex = i;

                    GameObject world = Instantiate(m_worldCopy);
                    World worldScr = player.GetComponent<World>();
                    worldScr.m_origin = origin;
                    worldScr.m_levels = new int[] { 6, 7, 8, 9, 10, 11 };
                    worldScr.m_player = Instantiate(m_playerCopy);

                    m_worlds[i] = world;

                    origin.x += World.m_w + 10.0f;
                }
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_HARD:
            {
                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                Vector2 origin = new Vector2(0.0f, 0.0f);
                
                for (int i = 0; i < m_populationSize; i++)
                {
                    GameObject player = Instantiate(m_playerCopy);
                    Player playerScr = player.GetComponent<Player>();
                    playerScr.m_isAI = true;
                    playerScr.m_populationIndex = i;

                    GameObject world = Instantiate(m_worldCopy);
                    World worldScr = player.GetComponent<World>();
                    worldScr.m_origin = origin;
                    worldScr.m_levels = new int[] { 11, 12, 13, 14, 15 };
                    worldScr.m_player = Instantiate(m_playerCopy);

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

                    if (worldScr.IsOver() == false)
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
                        worldScr.LoadScene();
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

                DLL.DLL_PG_Quit();

                for (int i = 0; i < m_populationSize; i++)
                {
                    Destroy(m_worlds[i]);
                }

            break;

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
                
                DLL.DLL_PG_Quit();

                for (int i = 0; i < m_populationSize; i++)
                {
                    Destroy(m_worlds[i]);
                }

            break;

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
