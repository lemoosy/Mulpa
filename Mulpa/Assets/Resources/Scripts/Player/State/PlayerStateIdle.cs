using UnityEngine;

public class PlayerStateIdle : IPlayerState
{
    public void Enter(PlayerAbstract player)
    {
        // Animation
    }

    public void Exit(PlayerAbstract player) { }

    public void Update(PlayerAbstract player)
    {
        player.GetInput().Update(player);
        player.GetSprite().Update(player);
        player.GetMovement().Update(player);




    }
}