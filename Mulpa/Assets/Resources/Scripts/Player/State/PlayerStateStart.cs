public class PlayerStateStart : IPlayerState
{
    public void Enter(PlayerAbstract player)
    {
        PlayerMovement movement = player.GetMovement();
        movement.ResetPosition(player);

        player.SetState(new PlayerStateIdle());
    }

    public void Exit(PlayerAbstract player) { }

    public void Update(PlayerAbstract player) { }
}