namespace StrategyGame.Model.IService
{
    public interface IRandomService : IService
    {
        int GetRandomInt(int maxValue);

        float GetRandomFloat(float maxValue);
    }
}
