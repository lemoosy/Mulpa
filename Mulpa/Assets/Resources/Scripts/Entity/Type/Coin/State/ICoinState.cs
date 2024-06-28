public interface ICoinState
{
    public void Enter(Coin coin);

    public void Exit(Coin coin);

    public void Update(Coin coin);
}