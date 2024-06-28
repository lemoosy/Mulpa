using UnityEngine;

public class PlayerInputKeyboard : IPlayerInput
{
    private bool jump = false;
    private bool left = false;
    private bool right = false;

    public bool PressJump(PlayerAbstract player)
    {
        return jump;
    }

    public bool PressLeft(PlayerAbstract player)
    {
        return left;
    }

    public bool PressRight(PlayerAbstract player)
    {
        return right;
    }

    public void Update(PlayerAbstract player)
    {
        jump = Input.GetKey(KeyCode.UpArrow) && (player.GetState() is PlayerStateIdle);
        left = Input.GetKey(KeyCode.LeftArrow);
        right = Input.GetKey(KeyCode.RightArrow);
    }
}