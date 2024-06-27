using UnityEngine;

public class PlayerInputKeyboard : IPlayerInput
{
    public bool PressJump(Player player)
    {
        return Input.GetKey(KeyCode.LeftArrow);
    }

    public bool PressLeft(Player player)
    {
        return Input.GetKey(KeyCode.RightArrow);
    }

    public bool PressRight(Player player)
    {
        return Input.GetKey(KeyCode.UpArrow) && (player.GetState() is PlayerStateIdle);
    }
}