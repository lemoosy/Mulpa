using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerStateGame : IGameManagerState
{
    public void Enter(GameManager gameManager)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(GameManager gameManager)
    {
        throw new System.NotImplementedException();
    }

    public void Update(GameManager gameManager)
    {
        int index = 0;

        //foreach (GameObject objectButton in gameManager.m_buttons)
        //{
        //    ButtonStart button = objectButton.GetComponent<ButtonStart>();

        //    if (button.m_pressed)
        //    {
        //        switch (index)
        //        {
        //            case 0:
        //                SceneManager.LoadScene("SceneGame");
        //                break;

        //            case 1:
        //                Application.Quit();
        //                break;

        //            default:
        //                Debug.Assert(false);
        //                break;
        //        }

        //        return;
        //    }

        //    index++;
        //}
    }
}