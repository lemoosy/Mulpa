using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IGameManagerState state = null;

    private void Start()
    {
        state = new GameManagerStateHome();
    }

    private void Update()
    {
        state.Update(this);
    }

    public IGameManagerState GetState()
    {
        return state;
    }

    public void SetState(IGameManagerState state)
    {
        if (state != null)
        {
            state.Exit(this);
        }

        this.state = state;

        state.Enter(this);
    }
}