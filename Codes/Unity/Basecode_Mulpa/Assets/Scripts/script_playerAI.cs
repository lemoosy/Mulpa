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
    private static extern void DLL_NNSetScore(int p_nnIndex, float p_score);

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_PopulationUpdate();

    #endregion

    #region Variables

    // Unity

    public GameObject m_objSpawn;
    public GameObject m_objPlayer = null;

    // PG

    public bool m_activeAI = false;

    public int m_populationSize = 50;
    public int m_selectionSize = 5;
    public int m_childrenSize = 20;
    public int m_mutationRate = 10;

    public int m_populationCursor = 0;

    public int m_generation = 0;

    #endregion

    #region Fonctions

    void Start()
    {
        if (m_activeAI)
        {
            DLL_Init(m_populationSize, m_selectionSize, m_childrenSize, m_mutationRate);
        }
        else
        {
            Instantiate(m_objPlayer, m_objSpawn.transform.position, m_objSpawn.transform.rotation);
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
                DLL_NNSetScore(scr.m_nnIndex, scr.m_score);
                Destroy(m_objPlayer);
                m_objPlayer = null;
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            if (m_populationCursor == m_populationSize)
            {
                DLL_PopulationUpdate();
                m_populationCursor = m_selectionSize;
            }
            else
            {
                int nnIndex = m_populationCursor;
                m_populationCursor++;

                m_objPlayer = Instantiate(m_objPlayer, m_objSpawn.transform.position, m_objSpawn.transform.rotation);

                player scr = m_objPlayer.GetComponent<player>();
                scr.m_isIA = true;
                scr.m_nnIndex = nnIndex;
            }
        }
    }

    public void OnDestroy()
    {
        DLL_Quit();
    }

    #endregion
}
