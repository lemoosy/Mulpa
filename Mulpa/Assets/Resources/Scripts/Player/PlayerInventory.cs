public class PlayerInventory
{
    private int coinCount = 0;

    private int diamondCount = 0;

    public void Reset()
    {
        coinCount = 0;
        diamondCount = 0;
    }

    public void AddCoin()
    {
        coinCount += 1;
    }

    public void AddDiamond()
    {
        diamondCount += 1;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public int GetDiamondCount()
    {
        return diamondCount;
    }
}