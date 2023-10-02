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

    public GameObject m_objSpawn;
    public GameObject m_objPlayerCopy;
    public GameObject m_objPlayer;

    // PG

    public bool m_activeAI;

    public int m_populationSize;
    public int m_selectionSize;
    public int m_childrenSize;
    public int m_mutationRate;

    public int m_populationCursor;

    public int m_generation;

    #endregion

    #region Fonctions

    void Start()
    {
        if (m_activeAI)
        {
            m_populationSize = 50;
            m_selectionSize = 5;
            m_childrenSize = 20;
            m_mutationRate = 10;

            m_populationCursor = 0;

            m_generation = 0;

            DLL_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);
        }
        else
        {
            m_objPlayer = Instantiate(m_objPlayerCopy, m_objSpawn.transform.position, m_objSpawn.transform.rotation);

            player scr = m_objPlayer.GetComponent<player>();
            scr.m_objSpawn = m_objSpawn;
            scr.m_isIA = false;
        }
    }

    void Update()
    {
        if (!m_activeAI) return;

        if (m_objPlayer)
        {
            player scr = m_objPlayer.GetComponent<player>();

            if (scr.m_isDead)
            {
                DLL_NN_SetScore(scr.m_nnIndex, scr.m_score);
                Destroy(m_objPlayer);
                m_objPlayer = null;
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            if (m_populationCursor == m_populationSize)
            {
                DLL_Population_Update();
                m_populationCursor = m_selectionSize;
            }
            else
            {
                int nnIndex = m_populationCursor;
                m_populationCursor++;

                m_objPlayer = Instantiate(m_objPlayerCopy, m_objSpawn.transform.position, m_objSpawn.transform.rotation);

                player scr = m_objPlayer.GetComponent<player>();
                scr.m_objSpawn = m_objSpawn;
                scr.m_isIA = true;
                scr.m_nnIndex = nnIndex;
            }
        }
    }

    void OnDestroy()
    {
        DLL_Quit();
    }

    #endregion
}
