using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelAI : MonoBehaviour
{
    public int m_populationSize = 100;
    public int m_selectionSize = 20;
    public int m_childrenSize = 40;

    [HideInInspector] public GameObject[] m_population = null;
    [HideInInspector] public GameObject[] m_selection = null;

    [HideInInspector] public int m_generation = 0;

    public bool m_printScores = true;

    public int m_nCases = 10;
    public int m_nSpades = 5;
    public int m_nMonsters = 5;
    
    public bool m_hasLever = false;
    
    public Vector2Int m_positionSpawn = new Vector2Int(1, 7);
    public Vector2Int m_positionLever = new Vector2Int(14, 7);
    public Vector2Int m_positionExit = new Vector2Int(22, 7);

    public bool m_end = false;

    public void Start()
    {
        Resource.Init();

        Debug.Assert(m_selectionSize + m_childrenSize < m_populationSize);

        m_population = new GameObject[m_populationSize];
        m_selection = new GameObject[m_selectionSize];

        Population_CreateLevels();
    }

    public void Update()
    {
        if (Population_IsOver())
        {
            GameObject objectLevelEntityBest = IsOver();

            if (m_end || objectLevelEntityBest)
            {
                if (!objectLevelEntityBest)
                {
                    int index = Population_GetBest(true);

                    objectLevelEntityBest = m_population[index];
                }

                LevelEntity levelEntity = objectLevelEntityBest.GetComponent<LevelEntity>();

                levelEntity.m_levelMaker.Matrix_Completion();

                Export(levelEntity.m_levelMaker.Matrix_ToUnity(null));

                Destroy(gameObject);

                return;
            }

            Population_DestroyLevels();

            Selection_InitList();

            m_generation++;
            
            if (m_printScores)
            {
                print("----- Generation (" + m_generation + ") -----");

                int index = Population_GetBest(true);

                m_population[index].GetComponent<LevelEntity>().PrintScore();

                Selection_Print();
            }

            Children_CreateLevels();
            
            Population_CreateLevels();
        }
    }

    public GameObject IsOver()
    {
        int index = Population_GetBest(true);

        GameObject objectLevelEntity = m_population[index];

        LevelEntity levelEntity = objectLevelEntity.GetComponent<LevelEntity>();

        PlayerAI player = levelEntity.GetPlayer();

        if (
            (player.m_SP == 0) &&
            (player.m_nCases >= m_nCases) &&
            (player.m_nSpades >= m_nSpades) &&
            (player.m_nMonsters >= m_nMonsters)
        )
        {
            return m_population[index];
        }

        return null;
    }

    public void Export(GameObject p_objectLevelEditor)
    {
        // POUR EXPORTER EN EXE, COMMENTEZ LE RESTE

        string root = "Assets/Resources/Levels/Levels Generation/";

        int k = 0;

        while (true)
        {
            string path = root + "Level G-" + k.ToString() + ".prefab";

            if (!AssetDatabase.LoadAssetAtPath<GameObject>(path))
            {
                PrefabUtility.SaveAsPrefabAsset(p_objectLevelEditor, path);
                break;
            }

            k++;
        }
    }

    // Population.

    public int Population_GetBest(bool isMin)
    {
        int index = -1;

        for (int k = 0; k < m_populationSize; k++)
        {
            if (!m_population[k])
            {
                continue;
            }

            if (index == -1)
            {
                index = k;

                continue;
            }

            LevelEntity levelEntity = m_population[k].GetComponent<LevelEntity>();
            LevelEntity levelEntityBest = m_population[index].GetComponent<LevelEntity>();

            if (isMin)
            {
                if (levelEntity.GetScore() < levelEntityBest.GetScore())
                {
                    index = k;
                }
            }
            else
            {
                if (levelEntity.GetScore() > levelEntityBest.GetScore())
                {
                    index = k;
                }
            }
        }

        Debug.Assert(index != -1);

        return index;
    }

    public void Population_CreateLevels()
    {
        for (int k = 0; k < m_populationSize; k++)
        {
            if (m_population[k])
            {
                continue;
            }

            // Instantiate.

            m_population[k] = Instantiate(Resource.s_prefabLevelEntity, transform);

            LevelEntity levelEntity = m_population[k].GetComponent<LevelEntity>();

            levelEntity.InstantiateLevelRandom(m_hasLever, m_positionSpawn, m_positionLever, m_positionExit);

            // Position.

            int x = k * (LevelInformation.s_matrixSize.x + 2);
            int y = 0;
            
            Vector2 position = new Vector2((float)x, (float)y);
            
            m_population[k].transform.position = position;
        }
    }

    public bool Population_IsOver()
    {
        foreach (GameObject objectLevelEntity in m_population)
        {
            if (!objectLevelEntity)
            {
                continue;
            }

            LevelEntity levelEntity = objectLevelEntity.GetComponent<LevelEntity>();

            PlayerAI player = levelEntity.GetPlayer();

            if (player.gameObject.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    public void Population_DestroyLevels()
    {
        int size = m_populationSize - m_selectionSize;

        // TODO : O(n^2)

        for (int k = 0; k < size; k++)
        {
            int index = Population_GetBest(false);

            Destroy(m_population[index]);

            m_population[index] = null;
        }
    }

    // Selection.

    public void Selection_Clear()
    {
        for (int k = 0; k < m_selectionSize; k++)
        {
            m_selection[k] = null;
        }
    }

    public void Selection_InitList()
    {
        Selection_Clear();

        int k = 0;

        foreach (GameObject objectLevelEntity in m_population)
        {
            if (!objectLevelEntity)
            {
                continue;
            }

            m_selection[k++] = objectLevelEntity;
        }

        Debug.Assert(k == m_selectionSize);
    }

    public void Selection_Print()
    {
        List<float> scores = new List<float>();

        foreach (GameObject objectLevelEntity in m_selection)
        {
            LevelEntity levelEntity = objectLevelEntity.GetComponent<LevelEntity>();

            scores.Add(levelEntity.GetScore());
        }

        scores.Sort();

        for (int i = 0; i < 4; i++)
        {
            print(scores[i]);
        }
    }

    // Children.

    public void Children_CreateLevels()
    {
        int childCount = 0;

        for (int k = 0; k < m_populationSize; k++)
        {
            if (m_population[k])
            {
                continue;
            }

            int r1 = Utils.Int_Random(0, m_selectionSize - 1);
            int r2 = Utils.Int_Random(0, m_selectionSize - 1);

            GameObject objectLevelEntity1 = m_selection[r1];
            GameObject objectLevelEntity2 = m_selection[r2];

            // Instantiate.

            m_population[k] = Instantiate(Resource.s_prefabLevelEntity, transform);
            
            LevelEntity levelEntity = m_population[k].GetComponent<LevelEntity>();

            levelEntity.InstantiateLevelCrossover(objectLevelEntity1, objectLevelEntity2);

            // Position.

            int x = k * (LevelInformation.s_matrixSize.x + 2);
            int y = 0;

            Vector2 position = new Vector2((float)x, (float)y);

            m_population[k].transform.position = position;

            // Count.

            childCount++;
            
            if (childCount == m_childrenSize)
            {
                break;
            }
        }
    }
} 
