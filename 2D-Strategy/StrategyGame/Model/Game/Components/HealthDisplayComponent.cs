using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model.Game.Components
{
    public class HealthDisplayComponent : IComponent
    {
        public IEntity Parent { get; }

        private readonly float healthDisplayDuration = 5f;

        private IEntity healthDisplay;
        private IEntity healthBackground;

        public HealthDisplayComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            var health = Parent.GetComponent<HealthComponent>();
            if (health.Dead)
            {
                RemoveHealthDisplay();
            }
            else
            {
                if (health.Health < health.MaxHealth || (Parent.HasComponent<SelectComponent>() && Parent.GetComponent<SelectComponent>().Selected))
                {
                    UpdateHealthDisplay();
                }
                else if (health.LastHealthChange + healthDisplayDuration > Parent.Scene.ServiceProvider.GetService<ITimeService>().AbsoluteTime)
                {
                    UpdateHealthDisplay();
                }
                else
                {
                    RemoveHealthDisplay();
                }
            }
        }

        private void ShowHealthDisplay()
        {
            if (healthDisplay == null)
                healthDisplay = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateHealthDisplay(Parent);
            if (healthBackground == null)
                healthBackground = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateHealthBackground(Parent);
        }

        private void RemoveHealthDisplay()
        {
            if (healthDisplay != null)
            {
                healthDisplay.Destroy();
                healthDisplay = null;
            }

            if (healthBackground != null)
            {
                healthBackground.Destroy();
                healthBackground = null;
            }
        }

        private void UpdateHealthDisplay()
        {
            var health = Parent.GetComponent<HealthComponent>();

            if (healthDisplay == null || healthBackground == null)
            {
                ShowHealthDisplay();
            }

            var percentage = (float)health.Health / health.MaxHealth;
            const float barWidth = 1.4f;
            healthDisplay.Size = new Vector2(percentage * barWidth, healthDisplay.Size.Y);
        }
    }
}
