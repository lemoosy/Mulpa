public class PlayerStateStart : IPlayerState
{
    public void Enter(Player player)
    {
        PlayerMovement movement = player.GetMovement();
        movement.ResetPosition(player);

        player.SetState(new PlayerStateIdle());
    }

    public void Exit(Player player) { }

    public void Update(Player player) { }
}