using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Game
{
    public class Archer : IArmy
    {
        private readonly float maxHealth = 50f;

        private float timeSum;

        private Health healthBar = new Health();

        public Archer(PlayerType type, Vector2 position)
        {
            if (type == PlayerType.Player)
                Id = EntityType.ArcherPlayer;
            if (type == PlayerType.Enemy)
                Id = EntityType.ArcherEnemy;
            Owner = type;
            Position = position;
            Radius = 0.05f;
            Range = 0.3f;
            Health = 50;
            AttackDamage = 8;
            AttackSpeed = 1f;
            Speed = 0.0007f;

            Path = new List<Vector2>();
        }

        public EntityType Id { get; }

        public Vector2 Position { get; private set; }

        public float Radius { get; }

        public Vector2 Offset { get; }

        public PlayerType Owner { get; }

        public float Range { get; }

        public float Health { get; set; }

        public int AttackDamage { get; }

        public float AttackSpeed { get; }

        public bool Selected { get; set; }

        public IGameObject Target { get; private set; }

        public bool IsDead { get; private set; }

        public List<Vector2> Path { get; set; }

        public float Speed { get; }

        public bool HealthChange { get; private set; }

        public bool IsAttack { get; private set; }

        public bool IsHealing { get; private set; }

        public float BarState { get; set; }

        public float DamageArcher { get; private set; }

        public float HealArcher { get; private set; }

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
                Path.Clear();
                return UpdateAttack(time) ? new List<IEntity> { new Projectile(this, Target, AttackDamage) } : null;
            }

            return null;
        }

        public List<IEntity> UpdateHealing(float time, List<IGameObject> others)
        {
            if (IsDead)
                return null;

            if (Target is Tower)
            {
                if (IsDead)
                    return null;

                if (Target != null && HasInRange(Target) && !Target.IsDead)
                {
                    return UpdateAttack(time) ? new List<IEntity> { new Projectile(this, Target, AttackDamage) } : null;
                }

                Target = null;
            }

            // find new target
            foreach (IGameObject other in others)
            {
                if (other.IsDead || Owner != other.Owner)
                    continue;

                if (!HasInRange(other))
                    continue;

                Target = other;
                Path.Clear();
                UpdateHealing(time);
                return null;
            }

            return null;
        }

        public void Damage(int damage)
        {
            Health -= damage;

            BarState = HealthLoss(damage);

            DamageAnimation = DamageAnimation + 1;

            Console.WriteLine(Owner + " archer: " + Health);

            if (Health > 0)
            {
                if (Health <= 40 || Health <= 30 || Health <= 20 || Health <= 10)
                {
                    Console.WriteLine(Owner + " one soldier died");
                }

                HealthChange = true;
                IsAttack = true;
                return;
            }

            IsDead = true;
        }

        public void UpdatePosition()
        {
            if (IsDead)
                return;

            if (Path.Count <= 0)
                return;

            Vector2 direction;
            direction.X = Path[0].X - Position.X;
            direction.Y = Path[0].Y - Position.Y;

            if (Math.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y)) < Speed)
            {
                Position = Path[0];
                Path.RemoveAt(0);
            }
            else
            {
                direction.Normalize();
                Position += direction * Speed;
            }
        }

        public bool Healing(float heal)
        {
            if (Health <= maxHealth)
            {
                Health += heal;

                BarState = HealthWin(heal);

                Console.WriteLine(Owner + " archer: " + Health);

                HealthChange = true;
                IsHealing = true;
            }

            return true;
        }

        public bool HasInRange(IGameObject other)
        {
            return (Position - other.Position).Length < Range + other.Radius;
        }

        public float HealthLoss(float damage)
        {
            DamageArcher += damage;

            float bar = (0.095f / maxHealth) * 2;

            damage = DamageArcher * bar;

            return damage;
        }

        public float HealthWin(float heal)
        {
            HealArcher += heal;

            float bar = (0.095f / maxHealth) * 2;

            heal = -(HealArcher * bar);

            return heal;
        }

        public void Update(float time)
        {
        }

        private bool UpdateHealing(float time)
        {
            timeSum += time;

            if (timeSum < 1 / 0.4f)
                return false;

            timeSum -= 1 / 0.4f;

            return true;
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
