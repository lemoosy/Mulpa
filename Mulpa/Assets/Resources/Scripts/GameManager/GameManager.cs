using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> m_buttons = new List<GameObject>();

    private IGameManagerState state = null;

    void Start()
    {
        Resource.Init();

        state = new GameManagerStateHome();
    }

    private void Update()
    {
        state.Update(this);
    }
}
