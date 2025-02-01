namespace StrategyGame.Model.IService
{
    public interface ITimeService : IService
    {
        float DeltaTime { get; }

        float AbsoluteTime { get; }

        float TimeScale { get; set; }
    }
}
