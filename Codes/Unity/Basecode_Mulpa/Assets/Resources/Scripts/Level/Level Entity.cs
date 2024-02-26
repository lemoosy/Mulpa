using UnityEngine;

public class LevelEntity : MonoBehaviour
{
    [HideInInspector] public LevelMaker m_levelMaker = null;

    [HideInInspector] public GameObject m_objectLevelEditor = null;

    public GameObject m_objectPlayer = null;

    // Player.

    public PlayerAI GetPlayer()
    {
        PlayerAI player = m_objectPlayer.GetComponent<PlayerAI>();

        return player;
    }

    public float GetScore()
    {
        PlayerAI player = GetPlayer();

        float A = 3.0f * (float)player.m_SP;
        float B = 0.1f * (float)player.m_nCases;
        float C = 1.0f * (float)player.m_nSpades;
        float D = 1.0f * (float)player.m_nMonsters;

        return A - B - C - D;
    }

    public void PrintScore()
    {
        PlayerAI player = GetPlayer();

        int A = player.m_SP;
        int B = player.m_nCases;
        int C = player.m_nSpades;
        int D = player.m_nMonsters;

        print("SP = " + A.ToString() + " | nCases = " + B.ToString() + " | nSpades = " + C.ToString() + " | nMonsters = " + D.ToString());
    }

    // Instantiate.

    public void InstantiateLevelRandom(bool p_hasLever, Vector2Int p_positionSpawn, Vector2Int p_positionLever, Vector2Int p_positionExit)
    {
        // Level.

        m_levelMaker = new LevelMaker(p_hasLever, p_positionSpawn, p_positionLever, p_positionExit);

        m_levelMaker.Matrix_Init();

        m_objectLevelEditor = m_levelMaker.Matrix_ToUnity(transform);
        
        // Player.

        PlayerAI player = GetPlayer();

        player.m_objectLevelEditor = m_objectLevelEditor;

        player.gameObject.SetActive(true);
    }

    public void InstantiateLevelCrossover(GameObject p_objectLevelEntity1, GameObject p_objectLevelEntity2)
    {
        // Level.

        LevelEntity levelEntity1 = p_objectLevelEntity1.GetComponent<LevelEntity>();
        LevelEntity levelEntity2 = p_objectLevelEntity2.GetComponent<LevelEntity>();
        
        LevelMaker levelMaker1 = levelEntity1.m_levelMaker;
        LevelMaker levelMaker2 = levelEntity2.m_levelMaker;
        LevelMaker levelMaker3 = levelMaker1 + levelMaker2;
        
        m_levelMaker = levelMaker3;

        m_objectLevelEditor = m_levelMaker.Matrix_ToUnity(transform);

        // Player.

        PlayerAI player = GetPlayer();

        player.m_objectLevelEditor = m_objectLevelEditor;

        player.gameObject.SetActive(true);
    }
}
