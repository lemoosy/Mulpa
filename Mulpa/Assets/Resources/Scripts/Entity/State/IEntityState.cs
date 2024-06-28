public interface IEntityState
{
    public void Enter(PlayerUser player);

    public void Exit(PlayerUser player);

    public void Update(PlayerUser player);
}