using UnityEngine;

public class PlayerMovement
{
    private Vector2 velocityMaximum = new Vector2(7.5f, 10.0f);

    private Vector2 speed = new Vector2(1.0f, 2.0f);

    public void Update(PlayerAbstract player)
    {
        UpdateX(player);
        UpdateY(player);

        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();

        Vector2 velocity = rigidbody2D.velocity;

        velocity.x = Mathf.Clamp(velocity.x, -velocityMaximum.x, +velocityMaximum.x);
        velocity.y = Mathf.Clamp(velocity.y, -velocityMaximum.y, +velocityMaximum.y);

        rigidbody2D.velocity = velocity;
    }

    private void UpdateX(PlayerAbstract player)
    {
        IPlayerInput input = player.GetInput();

        Vector2 acceleration = new Vector2();

        if (input.PressLeft(player))
        {
            acceleration.x = -speed.x;
        }

        if (input.PressRight(player))
        {
            acceleration.x = +speed.x;
        }

        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();

        rigidbody2D.AddRelativeForce(acceleration);
    }

    private void UpdateY(PlayerAbstract player)
    {
        IPlayerInput input = player.GetInput();

        Vector2 acceleration = new Vector2();

        if (input.PressJump(player))
        {
            acceleration.y = +speed.y;
        }

        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();

        rigidbody2D.AddRelativeForce(acceleration, ForceMode2D.Impulse);
    }

    public void SetPosition(PlayerAbstract player, Vector2 position)
    {
        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();

        rigidbody2D.position = position;
    }

    public void ResetPosition(PlayerAbstract player)
    {
        //Level level = player.GetLevel();
        //SetPosition(player, level.GetPositionStart());
    }
}