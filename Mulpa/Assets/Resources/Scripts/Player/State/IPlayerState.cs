public interface IPlayerState
{
    public void Enter(IPlayerState state);

    public void Exit(IPlayerState state);

    public void Update(IPlayerState state);
}