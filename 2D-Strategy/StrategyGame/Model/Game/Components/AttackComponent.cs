using System;
using System.Collections.Generic;
using System.ComponentModel;
using OpenTK;
using OpenTK.Input;
using StrategyGame.Model.Components;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model.Game.Components
{
    public class AttackComponent : IComponent
    {
        public IEntity Parent { get; }

        public IEntity Target { get; set; }

        public IEntity NextTarget { get; set; }

        public float AttackDamage { get; }

        public float AttackSpeed { get; }

        public float Range { get; }

        public AttackType Type { get; }

        public Dictionary<int, float> DamageMultipliers { get; }

        private float AttackDelay { get; }

        private float lastAttack;

        private bool attackAnimation;

        private DrawTextureComponent orginalTexture;

        private float lastAnimation;
        private bool damaged;

        public AttackComponent(IEntity parent, float attackDamage, float attackSpeed, float range, float attackDelay, AttackType type, Dictionary<int, float> damageMultipliers)
        {
            Parent = parent;
            parent?.AddComponent(this);
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
            Range = range;
            AttackDelay = attackDelay;
            Type = type;
            DamageMultipliers = damageMultipliers;
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            if (Parent.HasComponent<MoveComponent>())
            {
                IKeystateService keyService = Parent.Scene.ServiceProvider.GetService<IKeystateService>();
                if (keyService.IsButtonPressed(MouseButton.Right) && Parent.GetComponent<OwnerComponent>().Owner == 0 && Parent.GetComponent<SelectComponent>().Selected)
                {
                    Target = null;
                    FindClickTarget();
                }

                if (NextTarget != null && Target == null)
                {
                    if (Parent.HasComponent<PathfindingComponent>() && Parent.GetComponent<PathfindingComponent>().Destination() != NextTarget.Position)
                    {
                        Parent.GetComponent<PathfindingComponent>().Pathfind(NextTarget.Position);
                        Parent.GetComponent<PathfindingComponent>().UpdateFlag();
                    }

                    if (HasInRange(NextTarget))
                    {
                        Parent.GetComponent<MoveComponent>().Moving = false;
                        Target = NextTarget;
                        NextTarget = null;
                    }
                }
            }

            UpdateAnimation();
            if (Parent.HasComponent<MoveComponent>() && Parent.GetComponent<MoveComponent>().Moving)
            {
                return;
            }

            HealthComponent healthComponent = Parent.GetComponent<HealthComponent>();
            if (healthComponent.Dead)
            {
                return;
            }

            if (Target == null)
            {
                if (!FindNewTarget())
                    return;
            }

            HealthComponent targetHealthComponent = Target.GetComponent<HealthComponent>();
            if (targetHealthComponent.Dead)
            {
                Target = null;
                return;
            }

            if (!HasInRange(Target))
            {
                Target = null;
                return;
            }

            if (Parent.GetComponent<OwnerComponent>().Owner == Target.GetComponent<OwnerComponent>().Owner)
            {
                Target = null;
                return;
            }

            ITimeService timeService = Parent.Scene.ServiceProvider.GetService<ITimeService>();
            if (attackAnimation)
                return;
            if (lastAttack + AttackSpeed > timeService.AbsoluteTime)
                return;
            lastAttack = timeService.AbsoluteTime;

            Attack();
        }

        private bool FindNewTarget()
        {
            OwnerComponent ownerComponent = Parent.GetComponent<OwnerComponent>();
            var targets = new List<IEntity>();
            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (!other.HasComponent<OwnerComponent>())
                    continue;
                OwnerComponent otherOwnerComponent = other.GetComponent<OwnerComponent>();
                if (ownerComponent.Owner == otherOwnerComponent.Owner)
                    continue;
                if (!other.HasComponent<HealthComponent>())
                    continue;
                if (!HasInRange(other))
                    continue;

                targets.Add(other);
            }

            if (targets.Count == 0)
                return false;

            targets.Sort((obj1, obj2) => (int)((Parent.Position - obj1.Position).Length - (Parent.Position - obj2.Position).Length));
            Target = targets[0];
            NextTarget = null;
            return true;
        }

        private bool FindClickTarget()
        {
            NextTarget = null;

            IKeystateService keyService = Parent.Scene.ServiceProvider.GetService<IKeystateService>();
            var clicked = new List<IEntity>();
            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (other.HasComponent<OwnerComponent>() && other.GetComponent<OwnerComponent>().Owner == Parent.GetComponent<OwnerComponent>().Owner)
                    continue;
                if (!other.HasComponent<HealthComponent>())
                    continue;
                if ((keyService.MousePosition - other.Position).Length > (other.Size.X / 2))
                    continue;

                clicked.Add(other);
            }

            if (clicked.Count == 0)
                return false;
            clicked.Sort((obj1, obj2) => (int)((keyService.MousePosition - obj1.Position).Length - (keyService.MousePosition - obj2.Position).Length));
            NextTarget = clicked[0];
            return true;
        }

        private bool HasInRange(IEntity other)
        {
            if (other.HasComponent<HealthComponent>())
            {
                var targetMultiplierType = other.GetComponent<HealthComponent>().ArmyType;
                if (DamageMultipliers.ContainsKey(targetMultiplierType))
                {
                    if (DamageMultipliers[targetMultiplierType] == 0)
                        return false;
                }
            }

            return (Parent.Position - other.Position).Length < Range + (Parent.Size.X / 2);
        }

        private void Attack()
        {
            attackAnimation = true;
            damaged = false;
            lastAnimation = Parent.Scene.ServiceProvider.GetService<ITimeService>().AbsoluteTime;
            orginalTexture = Parent.GetComponent<DrawTextureComponent>();
            Parent.RemoveComponent<DrawTextureComponent>();

            int owner = Parent.GetComponent<OwnerComponent>().Owner;

            string[] tex = new[] { orginalTexture.Texture };
            if (orginalTexture.Texture.StartsWith("Archer"))
            {
                tex = new string[16];
                for (var i = 0; i < tex.Length; i++)
                {
                    tex[i] = "Archer-Animation" + i + "-" + owner;
                }
            }
            else if (orginalTexture.Texture.StartsWith("Knight"))
            {
                tex = new string[18];
                for (var i = 0; i < tex.Length; i++)
                {
                    tex[i] = "Knight-Animation" + i + "-" + owner;
                }
            }

            new DrawAnimationComponent(Parent, orginalTexture.Layer, tex, 10f)
            {
                Repeat = false,
                LoopFinished = CancelAnimation,
            };
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (!attackAnimation)
                return;
            if (Target == null)
            {
                CancelAnimation();
                return;
            }

            if (damaged)
                return;

            if (lastAnimation + AttackDelay > Parent.Scene.ServiceProvider.GetService<ITimeService>().AbsoluteTime)
                return;

            DamageTarget();
        }

        private void CancelAnimation()
        {
            attackAnimation = false;
            Parent.RemoveComponent<DrawAnimationComponent>();
            Parent.AddComponent(orginalTexture);

            if (!damaged)
                DamageTarget();
        }

        private void DamageTarget()
        {
            if (Target == null)
                return;

            if (Target.HasComponent<OwnerComponent>() && Parent.HasComponent<OwnerComponent>() && Target.GetComponent<OwnerComponent>() != Parent.GetComponent<OwnerComponent>())
            {
                if (Type == AttackType.Far)
                {
                    var type = Parent.GetComponent<HealthComponent>().ArmyType;
                    ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateProjectile(Parent, Target, new Vector2(type == 2 ? 0.4f : 0.25f), AttackDamage);
                }
                else if (Type == AttackType.Melee)
                {
                    var damage = AttackDamage;

                    var targetMultiplierType = Target.GetComponent<HealthComponent>().ArmyType;
                    if (DamageMultipliers.ContainsKey(targetMultiplierType))
                        damage *= DamageMultipliers[targetMultiplierType];

                    Target.GetComponent<HealthComponent>().Damage(damage, Parent);
                }
            }

            damaged = true;
        }

        public enum AttackType
        {
            Melee,
            Far,
        }
    }
}
