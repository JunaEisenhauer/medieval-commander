namespace StrategyGame.Model.IService
{
    public interface IServiceProvider
    {
        T GetService<T>()
            where T : IService;

        void Update();
    }
}
