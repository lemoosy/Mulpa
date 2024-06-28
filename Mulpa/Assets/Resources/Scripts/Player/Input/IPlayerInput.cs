public interface IPlayerInput
{
    public bool PressJump(PlayerAbstract player);

    public bool PressLeft(PlayerAbstract player);

    public bool PressRight(PlayerAbstract player);

    public void Update(PlayerAbstract player);
}