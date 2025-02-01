using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model.IService;

namespace StrategyGame.Model.Game.Components
{
    public class CaptureGoldComponent : IComponent
    {
        public IEntity Parent { get; }

        public int GoldValue { get; set; }

        public CaptureGoldComponent(IEntity parent, int goldValue)
        {
            Parent = parent;
            parent?.AddComponent(this);
            GoldValue = goldValue;
        }

        public void Update()
        {
        }

        public void OnCapture(IEntity killer)
        {
            var owner = killer.GetComponent<OwnerComponent>().Owner;
            Parent.Scene.ServiceProvider.GetService<IGoldService>().AddGold(owner, GoldValue);

            if (Parent.HasComponent<HealerComponent>())
            {
                Parent.Scene.ServiceProvider.GetService<IGoldService>().AddGold(1, 150);
            }
        }
    }
}
