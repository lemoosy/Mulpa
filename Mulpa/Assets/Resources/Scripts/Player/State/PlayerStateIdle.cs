public class PlayerStateIdle : IPlayerState
{
    public void Enter(Player player) { }

    public void Exit(Player player) { }

    public void Update(Player player)
    {
        PlayerInput playerInput = new PlayerInput();
        playerInput.Update(player);



    }
}