using System;
using System.Drawing;
using StrategyGame.Model.Components;
using StrategyGame.Model.IService;
using StrategyGame.Service;
using StrategyGame.Service.Sound;

namespace StrategyGame.Model.Game.Components
{
    public class HealthComponent : IComponent
    {
        private readonly float animationTime = 0.15f;
        private readonly float fadeOutTime = 1.5f;

        public IEntity Parent { get; }

        private float health;

        private bool playedDeathSound;

        public float Health
        {
            get => health;
            private set
            {
                var absTime = Parent?.Scene?.ServiceProvider.GetService<ITimeService>().AbsoluteTime;
                if (absTime.HasValue)
                    LastHealthChange = absTime.Value;
                health = value;
            }
        }

        public float MaxHealth { get; }

        public float LastHealthChange { get; private set; }

        public bool Dead { get; private set; }

        public int ArmyType { get; }

        private IEntity damageAnimationEntity;
        private float animation;

        private float fadeOut = 1.5f;
        private byte startAlpha;

        public HealthComponent(IEntity parent, int health, int armyType)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Health = health;
            MaxHealth = health;
            LastHealthChange = float.MinValue;
            ArmyType = armyType;
            playedDeathSound = false;
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            if (animation > 0)
            {
                animation -= Parent.Scene.ServiceProvider.GetService<ITimeService>().DeltaTime;

                if (animation <= 0)
                {
                    damageAnimationEntity.Destroy();
                    damageAnimationEntity = null;
                }
            }

            if (Dead)
            {
                if (!playedDeathSound)
                {
                    Parent.Scene.ServiceProvider.GetService<IAudioService>().PlaySound("dead");
                    playedDeathSound = true;
                }

                fadeOut -= Parent.Scene.ServiceProvider.GetService<ITimeService>().DeltaTime;

                if (fadeOut <= 0)
                {
                    Parent.Destroy();
                }

                // Fade out
                if (Parent.HasComponent<DrawTextureComponent>())
                {
                    var texComp = Parent.GetComponent<DrawTextureComponent>();
                    var c = texComp.Color;
                    var newAlpha = (fadeOut / fadeOutTime) * startAlpha;
                    texComp.Color = Color.FromArgb((byte)newAlpha, c.R, c.G, c.B);
                }
            }
        }

        public void Damage(float damage, IEntity damager)
        {
            if (Dead)
                return;

            Health -= damage;

            if (damageAnimationEntity == null)
            {
                damageAnimationEntity = ((IGameEntityFactory)Parent?.Scene?.EntityFactory)?.CreateDamageAnimation(Parent);
                animation = animationTime;
            }

            if (Health <= 0)
            {
                Health = 0;
                OnDeath(damager);
            }

            if (Parent.GetComponent<OwnerComponent>().Owner != 0)
            {
                if (Parent.HasComponent<AttackComponent>() &&
                    Parent.GetComponent<AttackComponent>().NextTarget == null &&
                    Parent.GetComponent<AttackComponent>().Target == null)
                {
                    if (damager.HasComponent<HealComponent>())
                        Parent.GetComponent<AttackComponent>().NextTarget = damager;
                    else if (damager.HasComponent<ProjectileComponent>())
                        Parent.GetComponent<AttackComponent>().NextTarget = damager.GetComponent<ProjectileComponent>().Shooter;
                }
            }
        }

        public void Heal(float heal)
        {
            if (heal > MaxHealth)
            {
                health = MaxHealth;
            }
            else
            {
                Health += heal;
            }
        }

        private void OnDeath(IEntity killer)
        {
            if (Parent.HasComponent<CaptureGoldComponent>())
            {
                Parent.GetComponent<CaptureGoldComponent>().OnCapture(killer);
            }

            if (Parent.HasComponent<CaptureComponent>())
            {
                int owner = killer.GetComponent<OwnerComponent>().Owner;
                Parent.GetComponent<OwnerComponent>().Owner = owner;
                Health = MaxHealth;
                if (Parent.HasComponent<DrawTextureComponent>())
                {
                    Parent.GetComponent<DrawTextureComponent>().Texture = "Tower-" + owner;
                }
            }
            else
            {
                Dead = true;
                if (Parent.HasComponent<DrawTextureComponent>())
                {
                    startAlpha = Parent.GetComponent<DrawTextureComponent>().Color.A;
                }
            }
        }
    }
}