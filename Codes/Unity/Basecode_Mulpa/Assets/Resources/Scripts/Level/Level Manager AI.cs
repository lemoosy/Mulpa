using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerAI : MonoBehaviour
{
    public List<GameObject> m_levels = new List<GameObject>();

    [HideInInspector] public int m_cursor = 0;

    [HideInInspector] public GameObject m_current = null;

    public GameObject m_player = null;

    public int m_levelReset = 0;

    private void Start()
    {
        Debug.Assert(m_levels.Count > 0);
        Debug.Assert(m_player);

        SetLevel(m_levelReset);

        PlayerAI player = m_player.GetComponent<PlayerAI>();

        player.m_objectLevelEditor = m_current;
        
        player.gameObject.SetActive(true);
    }

    private void Update()
    {
        PlayerAI player = m_player.GetComponent<PlayerAI>();

        if (player.m_isDead)
        {
            bool res = SetLevel(0);

            if (res)
            {
                player.m_objectLevelEditor = m_current;
                player.EndEpisode();
            }
            else
            {
                Destroy(player.gameObject);
                Destroy(gameObject);
            }
        }

        if (player.m_atExit)
        {
            bool res = SetLevel(m_cursor + 1);

            if (res)
            {
                player.m_objectLevelEditor = m_current;
                player.ResetLevel();
            }
            else
            {
                Destroy(player.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private bool SetLevel(int p_index)
    {
        int size = m_levels.Count;

        if ((p_index < 0) || (p_index >= size))
        {
            return false;
        }

        if (m_current)
        {
            Destroy(m_current);
        }

        m_cursor = p_index;

        m_current = Instantiate(m_levels[m_cursor], transform);

        return true;
    }
}
