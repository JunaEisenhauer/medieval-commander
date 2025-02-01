using OpenTK;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class TimeService : ITimeService
    {
        public float DeltaTime { get; private set; }

        public float AbsoluteTime { get; private set; }

        public float TimeScale { get; set; }

        public TimeService(GameWindow gameWindow)
        {
            DeltaTime = 0;
            AbsoluteTime = 0;
            TimeScale = 1;

            gameWindow.UpdateFrame += (s, e) =>
            {
                DeltaTime = (float)e.Time * TimeScale;
                AbsoluteTime += (float)e.Time * TimeScale;
            };
        }
    }
}
