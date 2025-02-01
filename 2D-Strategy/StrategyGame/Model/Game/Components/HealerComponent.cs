using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model.Game.Components
{
    public class HealerComponent : IComponent
    {
        public IEntity Parent { get; }

        public float Heal { get; }

        public float HealSpeed { get; }

        public float Range { get; }

        private float lastHeal;

        public HealerComponent(IEntity parent, float heal, float healSpeed, float range)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Heal = heal;
            HealSpeed = healSpeed;
            Range = range;
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            bool heal = true;
            var timeService = Parent.Scene.ServiceProvider.GetService<ITimeService>();
            if (lastHeal + HealSpeed > timeService.AbsoluteTime)
                return;
            lastHeal = timeService.AbsoluteTime;

            var target = FindNearestEntity();
            if (target == null)
                return;

            OwnerComponent ownerComponent = Parent.GetComponent<OwnerComponent>();

            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (!other.HasComponent<OwnerComponent>())
                    continue;

                OwnerComponent otherOwnerComponent = other.GetComponent<OwnerComponent>();

                if (ownerComponent.Owner == otherOwnerComponent.Owner)
                    continue;
                if (!HasInRange(other))
                    continue;

                heal = false;
            }

            if (heal)
            {
                target.GetComponent<HealthComponent>().Heal(Heal);
            }
        }

        private IEntity FindNearestEntity()
        {
            var entitiesInRange = new Dictionary<float, IEntity>();

            OwnerComponent ownerComponent = Parent.GetComponent<OwnerComponent>();
            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (!other.HasComponent<OwnerComponent>())
                    continue;
                if (other == Parent)
                    continue;
                OwnerComponent otherOwnerComponent = other.GetComponent<OwnerComponent>();
                if (ownerComponent.Owner != otherOwnerComponent.Owner)
                    continue;
                if (!other.HasComponent<HealthComponent>() || !other.HasComponent<HealComponent>())
                    continue;
                HealthComponent otherHealth = other.GetComponent<HealthComponent>();
                if (otherHealth.Dead)
                    continue;
                if (otherHealth.Health >= otherHealth.MaxHealth)
                    continue;
                if (!HasInRange(other))
                    continue;
                var range = GetRange(other);
                if (!entitiesInRange.ContainsKey(range))
                    entitiesInRange.Add(range, other);
            }

            var sortedEntities = entitiesInRange.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            return sortedEntities.Count == 0 ? null : sortedEntities.First().Value;
        }

        private bool HasInRange(IEntity other)
        {
            return (Parent.Position - other.Position).Length < Range + (Parent.Size.X / 2);
        }

        private float GetRange(IEntity other)
        {
            return (Parent.Position - other.Position).Length;
        }
    }
}
