using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class script_AI : MonoBehaviour
{
    #region DLL

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_Init(int p_populationSize, int p_selectionSize, int p_childrenSize, int p_mutationRate);

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_Quit();

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_NN_SetScore(int p_nnIndex, float p_score);

    [DllImport("Basecode_DLL.dll")]
    private static extern float DLL_NN_GetScore(int p_nnIndex);

    [DllImport("Basecode_DLL.dll")]
    private static extern bool DLL_Population_Update();

    #endregion

    #region Variables

    // Unity

    public GameObject m_objPlayerCopy;
    
    private GameObject m_objPlayer = null;

    private bool m_firstFrame = true;

    // PG

    public bool m_activeAI = false;

    public int m_populationSize = 50;
    public int m_selectionSize = 20;
    public int m_childrenSize = 10;
    public int m_mutationRate = 10;

    private int m_populationCursor = 0;

    public int m_generation = 0;

    #endregion

    #region Fonctions

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(1);

        if (m_activeAI)
        {
            DLL_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);
        }
    }

    void Update()
    {
        if (m_firstFrame)
        {
            m_firstFrame = false;
            return;
        }

        if (m_objPlayer == null)
        {
            if (m_activeAI)
            {
                if (m_populationCursor == m_populationSize)
                {
                    //bool res = DLL_Population_Update();

                    //if (res) print("ERROR - DLL_Population_Update()");

                    m_populationCursor = 0;
                    m_generation++;
                }
                else
                {
                    int nnIndex = m_populationCursor;
                    m_populationCursor++;

                    m_objPlayer = Instantiate(m_objPlayerCopy);

                    player scr = m_objPlayer.GetComponent<player>();
                    scr.m_isIA = true;
                    scr.m_nnIndex = nnIndex;
                }
            }
            else
            {
                m_objPlayer = Instantiate(m_objPlayerCopy);

                player scr = m_objPlayer.GetComponent<player>();
                scr.m_isIA = false;
            }
        }
        else
        {
            player scr = m_objPlayer.GetComponent<player>();

            if (scr.m_isDead)
            {
                DLL_NN_SetScore(scr.m_nnIndex, scr.m_score);
                Destroy(m_objPlayer);
                m_objPlayer = null;
                SceneManager.LoadScene(1);
            }
        }
    }

    void OnDestroy()
    {
        if (m_activeAI)
        {
            DLL_Quit();
        }
    }

    #endregion
}
