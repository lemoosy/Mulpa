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
using _Game;
using _Settings;

public class GameSettings : MonoBehaviour
{



    #region Variables



    // Mode de jeu.
    public Settings.ModeID m_mode = Settings.ModeID.MODE_SOLO;


    
    // Taille de la population.
    public int m_populationSize = 50;

    // Taille de la sélection.
    public int m_selectionSize = 20;

    // Nombre d'enfants.
    public int m_childrenSize = 10;

    // Nombre de fois qu'un enfant est muté.
    public int m_mutationRate = 10;

    // Curseur pour la liste de population.
    public int m_populationCursor = 0;

    // Nombre de générations.
    public int m_generation = 0;



    // Joueur.
    public GameObject m_player = null;

    // Jeux.
    public Game[] m_game = null;



    #endregion



    #region Fonctions



    void Start()
    {
        Debug.Assert(m_player, "ERROR - GameSettings::Start()");

        Player playerScript = m_player.GetComponent<Player>();

        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:
            {
                playerScript.m_isAI = false;
                
                m_game = new Game[1];
                
                int[] worlds = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                
                m_game[0] = new Game(Vector2.zero, worlds, m_player);
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_EASY:
            {
                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                playerScript.m_isAI = true;

                int[] worlds = new int[] { 1, 2, 3, 4, 5 };

                m_game = new Game[m_populationSize];

                for (int i = 0; i < m_populationSize; i++)
                {
                    playerScript.m_nnIndex = i;

                    m_game[i] = new Game(Vector2.zero, worlds, m_player); // TODO
                }
                
                break;
            }

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            {
                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                playerScript.m_isAI = true;

                int[] worlds = new int[] { 6, 7, 8, 9, 10 };

                m_game = new Game[m_populationSize];

                for (int i = 0; i < m_populationSize; i++)
                {
                    playerScript.m_nnIndex = i;

                    m_game[i] = new Game(Vector2.zero, worlds, m_player); // TODO
                }

                break;
            }

            case Settings.ModeID.MODE_TRAINING_HARD:
            {
                DLL.DLL_PG_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);

                playerScript.m_isAI = true;

                int[] worlds = new int[] { 11, 12, 13, 14, 15 };

                m_game = new Game[m_populationSize];

                for (int i = 0; i < m_populationSize; i++)
                {
                    playerScript.m_nnIndex = i;

                    m_game[i] = new Game(Vector2.zero, worlds, m_player); // TODO
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
            break;

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
            break;

            case Settings.ModeID.MODE_TRAINING_HARD:
            break;
        }
    }

    void OnDestroy()
    {
        switch (m_mode)
        {
            case Settings.ModeID.MODE_SOLO:
            break;

            case Settings.ModeID.MODE_TRAINING_EASY:
                DLL.DLL_PG_Quit();
            break;

            case Settings.ModeID.MODE_TRAINING_MEDIUM:
                DLL.DLL_PG_Quit();
            break;

            case Settings.ModeID.MODE_TRAINING_HARD:
                DLL.DLL_PG_Quit();
            break;
        }
    }



    #endregion



}
