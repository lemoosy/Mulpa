using UnityEngine;

public class GameManagerObject : MonoBehaviour
{
    public static GameObject self = null;

    private GameManager gameManager = null;

    public void Start()
    {
        if (self == null)
        {
            self = gameObject;

            DontDestroyOnLoad(gameObject);

            gameManager = new GameManager();
        }
        else
        {
            Destroy(self);
        }
    }

    public GameManager GetGameManager()
    {
        return gameManager;
    }
}