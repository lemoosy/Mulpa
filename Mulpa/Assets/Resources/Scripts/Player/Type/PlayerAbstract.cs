using UnityEngine;

public abstract class PlayerAbstract : MonoBehaviour
{
    protected Level level = null;

    protected bool onGround = false;

    protected IPlayerInput input = null;

    protected PlayerSprite sprite = null;

    protected PlayerMovement movement = null;

    protected PlayerInventory inventory = null;

    protected IPlayerState state = null;

    public void SetLevel(Level level)
    {
        this.level = level;
    }

    public Level GetLevel()
    {
        return level;
    }

    public IPlayerInput GetInput()
    {
        return input;
    }

    public void SetInput(IPlayerInput input)
    {
        this.input = input;
    }

    public PlayerSprite GetSprite()
    {
        return sprite;
    }

    public PlayerMovement GetMovement()
    {
        return movement;
    }

    public PlayerInventory GetInventory()
    {
        return inventory;
    }

    public IPlayerState GetState()
    {
        return state;
    }

    public void SetState(IPlayerState state)
    {
        this.state = state;

        state.Enter(this);
    }
}