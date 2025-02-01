using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class GameStateService : IGameStateService
    {
        public bool Running { get; set; }

        public bool Won { get; set; }
    }
}
