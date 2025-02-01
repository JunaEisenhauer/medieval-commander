using StrategyGame.Model.IService;

namespace StrategyGame.Service.Sound
{
    public interface IAudioService : IService
    {
        void AddSound(string name);

        void PlaySound(string name);
    }
}
