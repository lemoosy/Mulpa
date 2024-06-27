using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerUser : MonoBehaviour
{
    public List<GameObject> m_levels = new List<GameObject>();

    [HideInInspector] public int m_cursor = 0;

    [HideInInspector] public GameObject m_current = null;

    public GameObject m_player = null;

    public List<GameObject> m_buttons = new List<GameObject>();

    private void Start()
    {
        Debug.Assert(m_levels.Count > 0);
        Debug.Assert(m_player);

        SetLevel(0);

        Player player = m_player.GetComponent<Player>();

        player.m_objectLevelEditor = m_current;
        
        player.gameObject.SetActive(true);
    }

    private void Update()
    {
        Player player = m_player.GetComponent<Player>();

        if (player.m_isDead)
        {
            bool res = SetLevel(m_cursor);

            if (res)
            {
                player.m_objectLevelEditor = m_current;
                player.ResetLevel();
                player.gameObject.SetActive(true);
            }
        }

        if (player.m_atExit)
        {
            bool res = SetLevel(m_cursor + 1);

            if (res)
            {
                player.m_objectLevelEditor = m_current;
                player.ResetLevel();
                player.gameObject.SetActive(true);
            }
        }

        // BUTTONS

        int index = 0;

        foreach (GameObject objectButton in m_buttons)
        {
            Button button = objectButton.GetComponent<Button>();

            if (button.m_pressed)
            {
                switch (index)
                {
                    case 0:

                        if (Time.timeScale == 0.0f)
                        {
                            Time.timeScale = 1.0f;
                        }
                        else
                        {
                            Time.timeScale = 0.0f;
                        }

                        break;

                    case 1:
                        Time.timeScale = 1.0f;
                        SceneManager.LoadScene(0);
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            index++;

            button.m_pressed = false;
        }
    }

    private bool SetLevel(int p_index)
    {
        int size = m_levels.Count;

        if ((p_index < 0) || (p_index >= size))
        {
            SceneManager.LoadScene(0);

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
