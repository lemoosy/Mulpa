using UnityEngine;

public class PlayerStateIdle : IPlayerState
{
    public void Enter(Player player)
    {
        // Animation
    }

    public void Exit(Player player) { }

    public void Update(Player player)
    {
        player.GetInput().Update(player);
        player.GetSprite().Update(player);
        player.GetMovement().Update(player);




    }
}