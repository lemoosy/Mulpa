public interface ILevelState
{
    public void Enter(Level level);

    public void Exit(Level level);

    public void Update(Level level);
}