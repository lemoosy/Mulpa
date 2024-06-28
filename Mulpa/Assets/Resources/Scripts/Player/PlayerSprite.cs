public class PlayerSprite
{
    public void Update(Player player)
    {
        IPlayerInput input = player.GetInput();

        if (input.PressLeft(player) && (player.transform.localScale.x > 0.0f))
        {

        }
    }
}