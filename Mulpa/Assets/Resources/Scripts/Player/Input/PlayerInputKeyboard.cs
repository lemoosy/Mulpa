using UnityEngine;

public class PlayerInputKeyboard : IPlayerInput
{
    private bool jump = false;
    private bool left = false;
    private bool right = false;

    public bool PressJump(Player player)
    {
        return jump;
    }

    public bool PressLeft(Player player)
    {
        return left;
    }

    public bool PressRight(Player player)
    {
        return right;
    }

    public void Update(Player player)
    {
        jump = Input.GetKey(KeyCode.UpArrow) && (player.GetState() is PlayerStateIdle);
        left = Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.RightArrow);
    }
}