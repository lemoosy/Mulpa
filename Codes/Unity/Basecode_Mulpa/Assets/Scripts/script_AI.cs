using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class script_AI : MonoBehaviour
{
    #region DLL

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_Init();

    [DllImport("Basecode_DLL.dll")]
    private static extern void DLL_Quit();

    [DllImport("Basecode_DLL.dll")]
    private static extern int NN_Create();

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_Destroy(int p_nnID);

    [DllImport("Basecode_DLL.dll")]
    private static extern float NN_GetScore(int p_nnID);

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_SetScore(int p_nnID, float p_score);

    [DllImport("Basecode_DLL.dll")]
    private static extern int NN_Crossover(int p_nnID_1, int p_nnID_2);

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_Mutation(int p_nnID, int p_rate);

    #endregion

    #region Variables

    public GameObject m_playerInit;
    public GameObject m_playerCurr;

    public int m_populationSize = 50;
    public int m_populationCursor = 0;
    public int[] m_population;

    public int m_selectionSize = 5;
    public int[] m_selection;

    public int m_childrenSize = 20;
    public int[] m_children;

    public int m_mutationRate = 3;

    public int generation = 0;

    #endregion

    #region Fonctions -> Unity

    void Start()
    {
        DLL_Init();

        m_playerCurr = null;

        m_population = new int[m_populationSize];

        for (int i = 0; i < m_populationSize; i++)
        {
            m_population[i] = -1;
        }

        m_selection = new int[m_selectionSize];

        m_children = new int[m_childrenSize];
    }

    void Update()
    {
        // Si le joueur est vivant, on vérifie s'il est mort.
        if (m_playerCurr)
        {
            player scr = m_playerCurr.GetComponent<player>();

            if (scr.m_isDead)
            {
                NN_SetScore(scr.m_nnID, scr.m_score);
                Destroy(m_playerCurr);
                m_playerCurr = null;
            }
        }

        // Si le joueur n'est pas vivant, on crée un autre joueur OU nouvelle génération.
        else
        {
            // Si la population est pleine, nouvelle génération.
            if (Population_IsOver())
            {
                Selection_Fill();
                Population_Destroy();
                Children_Fill();
                Selection_ToPopulation();
                Children_ToPopulation();
            }

            // Si la population n'est pas pleine, on crée un nouveau joueur.
            else
            {
                int nnID = NN_Create();

                m_playerCurr = Instantiate(m_playerInit, m_playerInit.transform.position, m_playerInit.transform.rotation);

                player scr = m_playerCurr.GetComponent<player>();
                scr.m_nnID = nnID;

                m_population[m_populationCursor] = nnID;
                m_populationCursor++;
            }
        }
    }

    public void OnDestroy()
    {
        DLL_Quit();
    }

    #endregion

    #region PG

    // Population.

    void Population_Fill()
    {
        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] == -1)
            {
                m_population[i] = NN_Create();
            }
        }
    }

    void Population_Insert(int p_nnID)
    {
        Debug.Assert(m_populationCursor < m_populationSize, "ERROR - Population_Insert()");

        m_population[m_populationCursor] = p_nnID;
        m_populationCursor++;
    }

    bool Population_IsOver()
    {
        return (m_populationSize == m_populationCursor);
    }

    void Population_Destroy()
    {
        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] != -1)
            {
                NN_Destroy(m_population[i]);
            }
        }

        m_populationCursor = 0;
    }

    int Population_RemoveBest()
    {
        int indexMin = -1;

        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] == null) continue;

            if (indexMin == -1)
            {
                indexMin = i;
                continue;
            }

            player scriptMinimum = m_population[indexMin].GetComponent<player>();
            player scriptCurrent = m_population[i].GetComponent<player>();

            //if (scriptCurrent.GetScore() < scriptMinimum.GetScore())
            {
                indexMin = i;
            }
        }

        GameObject objectMin = m_population[indexMin];
        m_population[indexMin] = null;

        return objectMin;
    }

    // Selection.

    void Selection_Fill()
    {
        for (int i = 0; i < m_selectionSize; i++)
        {
            int nnID = Population_RemoveBest();
            m_selection[i] = nnID;
        }
    }

    void Selection_ToPopulation()
    {
        for (int i = 0; i < m_selectionSize; i++)
        {
            Population_Insert(m_selection[i]);
            m_selection[i] = -1;
        }
    }

    // Children.

    void Children_Fill()
    {
        for (int i = 0; i < m_childrenSize; i++)
        {
            int r1 = Random.Range(0, m_selectionSize - 1);
            int r2 = Random.Range(0, m_selectionSize - 1);

            if (r1 == r2) continue;

            int nnID1 = m_selection[r1];
            int nnID2 = m_selection[r2];

            m_children[i] = NN_Crossover(nnID1, nnID2);
            NN_Mutation(m_children[i], m_mutationRate);
        }
    }

    void Children_ToPopulation()
    {
        for (int i = 0; i < m_childrenSize; i++)
        {
            if (m_children[i] == -1) continue;

            Population_Insert(m_children[i]);
            m_children[i] = -1;
        }
    }

    #endregion
}
