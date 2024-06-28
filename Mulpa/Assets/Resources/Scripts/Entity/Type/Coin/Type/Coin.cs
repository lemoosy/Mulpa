using UnityEngine;

public class Coin : EntityAbstract
{
    void Start()
    {
        Debug.Assert(movement != null);
        
        //Debug.Assert(state != null);
    }

    void Update()
    {
        //state.Update(this);
    }

    public void SetMovement(IEntityMovement movement)
    {
        this.movement = movement;
    }

    public IEntityMovement GetMovement()
    {
        return movement;
    }

    public void SetState(IEntityState state)
    {
        //if (state != null)
        //{
        //    this.state.Exit(this);
        //}

        //this.state = state;

        //this.state.Enter(this);
    }

    public Vector2 GetPosition()
    {
        return transform.localPosition;
    }

    public override void CollisionWithEntity(EntityAbstract entity)
    {
        throw new System.NotImplementedException();
    }
}