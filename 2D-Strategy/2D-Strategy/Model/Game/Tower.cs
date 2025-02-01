using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using StrategyGame.Model.Game;

namespace StrategyGame.Model
{
    /// <summary>
    /// A tower on the map
    /// </summary>
    public class Tower : IGameObject
    {
        private readonly float maxHealthFortress = 200f;

        private readonly float maxHealthTower = 150f;

        public Tower(PlayerType type, Vector2 position, float radius, float range, int health, int attackDamage, float attackSpeed)
        {
            if (type == PlayerType.Player)
                Id = EntityType.TowerPlayer;
            if (type == PlayerType.Enemy)
                Id = EntityType.TowerEnemy;
            Owner = type;
            Position = position;
            Radius = radius;
            Range = range;
            Health = health;
            AttackDamage = attackDamage;
            AttackSpeed = attackSpeed;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tower is the main tower (fortress)
        /// </summary>
        public bool IsMain { get; set; }

        public EntityType Id { get; }

        public Vector2 Position { get; set; }

        public float Radius { get; }

        public Vector2 Offset { get; }

        public PlayerType Owner { get; }

        public float Range { get; }

        public float Health { get; set; }

        public int AttackDamage { get; }

        public float AttackSpeed { get; }

        public bool Selected { get; set; }

        public IGameObject Target { get; private set; }

        private float timeSum;

        public bool IsDead { get; private set; }

        public bool HealthChange { get; private set; }

        public bool IsAttack { get; private set; }

        public bool IsHealing { get; }

        public float BarState { get; set; }

        public float DamageTower { get; private set; }

        public float DamageAnimation { get; private set; }

        public List<IEntity> UpdateAttack(float time, List<IGameObject> others)
        {
            if (IsDead)
                return null;

            if (Target != null && HasInRange(Target) && !Target.IsDead)
            {
                return UpdateAttack(time) ? new List<IEntity> { new Projectile(this, Target, AttackDamage) } : null;
            }

            Target = null;

            // find new target
            foreach (IGameObject other in others)
            {
                if (other.IsDead || Owner == other.Owner)
                    continue;

                if (!HasInRange(other))
                    continue;

                Target = other;
                return UpdateAttack(time) ? new List<IEntity> { new Projectile(this, Target, AttackDamage) } : null;
            }

            return null;
        }

        public List<IEntity> UpdateHealing(float time, List<IGameObject> others)
        {
            return null;
        }

        public void Damage(int damage)
        {
            Health -= damage;

            BarState = HealthLoss(damage);

            DamageAnimation = DamageAnimation + 1;

            Console.WriteLine(Owner + " tower: " + Health);

            if (Health > 0)
            {
                HealthChange = true;
                IsAttack = true;
                return;
            }

            IsDead = true;
            if (IsMain && Owner == PlayerType.Player)
                Console.WriteLine("Player lost");
            if (IsMain && Owner == PlayerType.Enemy)
                Console.WriteLine("Player won");
        }

        public bool HasInRange(IGameObject other)
        {
            return (Position - other.Position).Length < Range + other.Radius;
        }

        public float HealthLoss(float damage)
        {
            DamageTower += damage;

            if (IsMain)
            {
                float barFortress = (0.095f / maxHealthFortress) * 2;
                damage = DamageTower * barFortress;
                return damage;
            }

            float barTower = (0.095f / maxHealthTower) * 2;
            damage = DamageTower * barTower;
            return damage;
        }

        public bool Healing(float heal)
        {
            return true;
        }

        public void UpdateHealth(IGameObject other, float time)
        {
        }

        public void Update(float time)
        {
        }

        private bool UpdateAttack(float time)
        {
            timeSum += time;

            if (timeSum < 1 / AttackSpeed)
                return false;

            timeSum -= 1 / AttackSpeed;

            return true;
        }
    }
}
