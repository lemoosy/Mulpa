using UnityEngine;

public class Coin : EntityAbstract
{
    
    private ICoinState state = null;

    void Start()
    {
        Debug.Assert(movement != null);
        
        Debug.Assert(state != null);
    }

    void Update()
    {
        state.Update(this);
    }

    public void SetMovement(ICoinMovement movement)
    {
        this.movement = movement;
    }

    public ICoinMovement GetMovement()
    {
        return movement;
    }

    public void SetState(ICoinState state)
    {
        if (state != null)
        {
            this.state.Exit(this);
        }

        this.state = state;

        this.state.Enter(this);
    }

    public Vector2 GetPosition()
    {
        return transform.localPosition;
    }

    public void override CollisionWithEntity(EntityAbstract entity)
    {
        if (entity is Player)
        {
            SetState(new CoinStateFromPlayer());
        }
    }
}