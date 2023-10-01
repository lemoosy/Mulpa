using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

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
    private static extern int NN_Crossover(int p_id_1, int p_id_2);

    [DllImport("Basecode_DLL.dll")]
    private static extern void NN_Mutation(int p_id, int rate);

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
    }

    void Update()
    {
        // Si le joueur est vivant, on vérifie qu'il n'est pas mort.
        if (m_playerCurr)
        {
            player scr = m_playerCurr.GetComponent<player>();

            if (scr.m_isDead)
            {
                Destroy(m_playerCurr);
                m_playerCurr = null;
            }
        }

        // Si le joueur est mort, on en crée un autre OU on fait la PG.
        else
        {
            // Si la population est pleine -> PG.
            if (PopulationIsFull())
            {

            }

            // Sinon, on crée un nouveau joueur.
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

        if (PopulationIsFinished())
        {
            for (int i = 0; i < m_selectionSize; i++)
            {
                GameObject obj = PopulationPopMinimum();
                SelectionInsert(obj);
            }

            PopulationDestroy();

            for (int i = 0; i < m_childrenSize; i++)
            {
                int r1 = Random.Range(0, m_selectionSize);
                int r2 = Random.Range(0, m_selectionSize);

                if (r1 == r2) continue;

                GameObject obj1 = m_selection[r1];
                GameObject obj2 = m_selection[r2];

                player scr1 = m_selection[r1].GetComponent<player>();
                player scr2 = m_selection[r2].GetComponent<player>();

                GameObject obj3 =

                player scr3 = obj3.GetComponent<player>();

                scr3.m_id = NN_Crossover(scr1.m_id, scr2.m_id);

                m_children[i] = obj3;
            }

            Selection_ToPopulation();
            Children_ToPopulation();

            PopulationFill();
        }
    }

    public void OnDestroy()
    {
        if (m_activeAI)
        {
            DLL_Quit();
        }
    }

    #endregion


    #region Fonctions -> Population

    void PopulationFill()
    {
        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] == null)
            {
                m_population[i] = Instantiate(m_playerINIT, m_playerINIT.transform.position, m_playerINIT.transform.rotation);

                player scr = m_population[i].GetComponent<player>();
                scr.m_id = NN_Create();
            }
        }
    }

    void PopulationInsert(GameObject p_obj)
    {
        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] == null)
            {
                m_population[i] = p_obj;
                return;
            }
        }
    }

    void PopulationDestroy()
    {
        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] != null)
            {
                Destroy(m_population[i]);
            }
        }
    }

    bool PopulationIsFinished()
    {
        for (int i = 0; i < m_populationSize; i++)
        {
            if (m_population[i] != null)
            {
                player scr = m_population[i].GetComponent<player>();

                if (scr.m_isDead == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool PopulationIsFull()
    {
        return false;
    }

    GameObject PopulationPopMinimum()
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

    #endregion

    #region Fonctions -> Selection

    void SelectionInsert(GameObject p_object)
    {
        for (int i = 0; i < m_selectionSize; i++)
        {
            if (m_selection[i] == null) continue;

            m_selection[i] = p_object;

            return;
        }
    }

    void Selection_ToPopulation()
    {
        for (int i = 0; i < m_selectionSize; i++)
        {
            if (m_selection[i] == null) continue;
            PopulationInsert(m_selection[i]);
            m_selection[i] = null;
        }
    }

    #endregion

    #region Fonctions -> Children

    void Children_ToPopulation()
    {
        for (int i = 0; i < m_childrenSize; i++)
        {
            if (m_children[i] == null) continue;

            PopulationInsert(m_children[i]);
            m_children[i] = null;
        }
    }

    #endregion

}
