using UnityEngine;

public class PlayerMovement
{
    private Vector2 speed = new Vector2(10.0f, 12.0f);

    public void Update(Player player)
    {
        //animator.SetBool("IsJumping", false);
        //animator.SetBool("IsWalking", false);

        //Vector2 velocity = GetVelocity();

        //if (m_left)
        //{
        //    if (m_impulse)
        //    {
        //        velocity.x = -m_speed.x;
        //    }
        //    else
        //    {
        //        velocity.x += -m_speed.x * 0.15f;
        //    }
        //}

        //if (m_right)
        //{
        //    if (m_impulse)
        //    {
        //        velocity.x = +m_speed.x;
        //    }
        //    else
        //    {
        //        velocity.x += +m_speed.x * 0.15f;
        //    }
        //}

        //if (m_up && m_onGround)
        //{
        //    animator.SetBool("IsJumping", true);

        //    velocity.y = m_speed.y;
        //}

        //velocity.x *= 0.8f;

        //velocity.x = Mathf.Clamp(velocity.x, -m_speed.x, +m_speed.x);
        //velocity.y = Mathf.Clamp(velocity.y, -m_speed.y, +m_speed.y);

        //SetVelocity(velocity);

        //if (Mathf.Abs(velocity.x) > 1.0f)
        //{
        //    animator.SetBool("IsWalking", true);
        //}
    }

    public void SetPosition(Player player, Vector2 position)
    {
        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
        rigidbody2D.position = position;
    }

    public void ResetPosition(Player player)
    {
        Level level = player.GetLevel();
        SetPosition(player, level.GetPositionStart());
    }
}