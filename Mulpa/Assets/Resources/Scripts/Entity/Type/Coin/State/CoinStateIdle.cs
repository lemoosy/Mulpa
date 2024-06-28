public class CoinStateIdle : ICoinState
{
    public void Enter(Coin coin)
    {
        coin.SetMovement(new CoinMovementIdle(coin));
    }

    public void Exit(Coin coin) { }

    public void Update(Coin coin)
    {
        coin.GetMovement().Update(coin);
    }
}