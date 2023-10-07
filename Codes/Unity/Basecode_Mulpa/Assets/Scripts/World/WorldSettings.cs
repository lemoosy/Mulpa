using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _World;

public class WorldSettings : MonoBehaviour
{



    #region Variables



    // Objet représentant le joueur.
    public GameObject m_objPlayer = null;

    // Permet de savoir si le joueur est une IA ou non.
    public bool m_modeAI = false;

    // Index du réseau de neurones du joueur.
    public int m_nnIndex = -1;

    // Difficulté de l'IA.
    public int m_difficulty = 0;

    // Score de l'IA.
    public float m_score = 0.0f;

    // Permet de savoir si le joueur est mort.
    public bool m_end = false;

    // Permet de savoir si le joueur a fini les niveaux.
    public bool m_win = false;



    #endregion



    #region Functions



    // Unity.



    void Start()
    {
        Debug.Assert(m_objPlayer != null, "ERROR (1) - WorldSettings::Start()");
        
        if (m_modeAI)
        {
            Debug.Assert(m_nnIndex != -1 && m_difficulty != 0, "ERROR (2) - WorldSettings::Start()");
        }

        LoadWorld();
        LoadPlayer();
        LoadMonsters();
    }

    void Update()
    {
        UpdateWorld();
        UpdatePlayer();
    }



    // Load.



    void LoadWorld()
    {
        // Si le joueur est une IA, on charge les mondes par rapport à la difficulté.
        if (m_modeAI)
        {
            switch (m_difficulty)
            {
                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    break;
            }
        }

        // Si le joueur n'est pas une IA, on charge le premier niveau.
        else
        {

        }
    }

    void LoadPlayer()
    {
        GameObject obj = Instantiate(m_objPlayer);

        Player scr = obj.GetComponent<Player>();

        if (m_modeAI)
        {
            scr.m_isAI = true;
            scr.m_nnIndex = m_nnIndex;
        }
        else
        {

        }
    }

    void LoadMonsters()
    {

    }



    // Update.



    void UpdateWorld()
    {

    }

    void UpdatePlayer()
    {

    }



    #endregion



}
