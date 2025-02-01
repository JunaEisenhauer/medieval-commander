using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model.IService;

namespace StrategyGame.Model.Game.Components
{
    public class ProduceGoldComponent : IComponent
    {
        public IEntity Parent { get; }

        public int Amount { get; }

        public float EarnSpeed { get; }

        private float lastEarn;

        public ProduceGoldComponent(IEntity parent, int amount, float earnSpeed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Amount = amount;
            EarnSpeed = earnSpeed;
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            ITimeService timeService = Parent.Scene.ServiceProvider.GetService<ITimeService>();
            if (lastEarn + EarnSpeed > timeService.AbsoluteTime)
                return;
            lastEarn = timeService.AbsoluteTime;

            IGoldService goldService = Parent.Scene.ServiceProvider.GetService<IGoldService>();
            int owner = Parent.GetComponent<OwnerComponent>().Owner;
            goldService.AddGold(owner, Amount);
        }
    }
}
