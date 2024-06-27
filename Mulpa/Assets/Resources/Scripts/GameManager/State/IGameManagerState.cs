public interface IGameManagerState
{
    public void Enter(GameManager state);

    public void Exit(GameManager state);

    public void Update(GameManager state);
}