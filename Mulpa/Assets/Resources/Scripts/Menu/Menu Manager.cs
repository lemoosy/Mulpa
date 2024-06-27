using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> m_buttons = new List<GameObject>();

    void Start()
    {
        Resource.Init();
    }

    private void Update()
    {
        int index = 0;

        foreach (GameObject objectButton in m_buttons)
        {
            Button button = objectButton.GetComponent<Button>();

            if (button.m_pressed)
            {
                switch (index)
                {
                    case 0:
                        SceneManager.LoadScene(1);
                        break;

                    case 1:
                        Application.Quit();
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }

                return;
            }

            index++;
        }
    }
}
