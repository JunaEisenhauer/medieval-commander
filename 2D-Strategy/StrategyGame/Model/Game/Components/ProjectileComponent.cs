using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Model.Game.Components
{
    public class ProjectileComponent : IComponent
    {
        public IEntity Parent { get; }

        public IEntity Target { get; }

        public IEntity Shooter { get; }

        public float AttackDamage { get; }

        private readonly Dictionary<int, float> damageMultipliers;

        public ProjectileComponent(IEntity parent, IEntity target, IEntity shooter, float attackDamage, Dictionary<int, float> damageMultipliers)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Target = target;
            Shooter = shooter;
            AttackDamage = attackDamage;
            this.damageMultipliers = damageMultipliers;
        }

        public void Update()
        {
            var moveComponent = Parent.GetComponent<MoveComponent>();
            if (moveComponent.Moving)
            {
                moveComponent.Destination = Target.Position;
                return;
            }

            if (Target.GetComponent<OwnerComponent>() != Parent.GetComponent<OwnerComponent>())
            {
                var damage = AttackDamage;

                var targetMultiplierType = Target.GetComponent<HealthComponent>().ArmyType;
                if (damageMultipliers.ContainsKey(targetMultiplierType))
                    damage *= damageMultipliers[targetMultiplierType];

                Target.GetComponent<HealthComponent>().Damage(damage, Parent);
            }

            Parent.Destroy();
        }
    }
}
