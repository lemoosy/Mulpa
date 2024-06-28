using UnityEngine;

public abstract class EntityAbstract : MonoBehaviour, IColliderManager
{
    protected IEntityMovement movement = null;



    public abstract void CollisionWithEntity(EntityAbstract entity);
}