public class PlayerSprite
{
    public void Update(Player player)
    {
        PlayerInput input = player.GetInput();

        if (input.left && (player.transform.localScale.x > 0.0f))
        {

        }
    }
}