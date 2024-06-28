public interface IPlayerInput
{
    public bool PressJump(Player player);

    public bool PressLeft(Player player);

    public bool PressRight(Player player);

    public void Update(Player player);
}