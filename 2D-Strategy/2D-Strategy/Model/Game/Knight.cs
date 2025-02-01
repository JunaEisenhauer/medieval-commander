using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Game
{
    public class Knight : IArmy
    {
        private readonly Health healthBar = new Health();

        private readonly float maxHealth = 100;

        private float timeSum;
        private bool attackAnimation;
        private bool attackState;

        public Knight(PlayerType type, Vector2 position)
        {
            if (type == PlayerType.Player)
                Id = EntityType.KnightPlayer;
            if (type == PlayerType.Enemy)
                Id = EntityType.KnightEnemy;
            Owner = type;
            Position = position;
            Radius = 0.055f;
            Range = 0.02f;
            Health = 100;
            AttackDamage = 10;
            AttackSpeed = 1f;
            Speed = 0.0005f;

            Path = new List<Vector2>();
        }

        public EntityType Id { get; }

        public Vector2 Position { get; private set; }

        public float Radius { get; }

        public Vector2 Offset { get; private set; }

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

        public bool IsHealing { get; set; }

        public float BarState { get; set; }

        public float DamageKnight { get; private set; }

        public float HealKnight { get; private set; }

        public float DamageAnimation { get; private set; }

        public List<IEntity> UpdateAttack(float time, List<IGameObject> others)
        {
            if (IsDead)
                return null;

            if (Target != null && HasInRange(Target) && !Target.IsDead)
            {
                UpdateAttack(time);
                return null;
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
                UpdateAttack(time);
                return null;
            }

            return null;
        }

        public List<IEntity> UpdateHealing(float time, List<IGameObject> others)
        {
            if (IsDead)
                return null;

            if (Target is Tower)
            {
                if (Target != null && HasInRange(Target) && !Target.IsDead)
                {
                    UpdateHealing(time);
                    return null;
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

            Console.WriteLine(Owner + " knight: " + Health);

            if (Health > 0)
            {
                if (Health <= 80 || Health <= 60 || Health <= 40 || Health <= 20)
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

                Console.WriteLine(Owner + " knight: " + Health);

                HealthChange = true;
                IsHealing = true;
            }

            return true;
        }

        public bool UpdateHealing(float time)
        {
            timeSum += time;

            if (timeSum < 1 / 0.4f)
                return false;

            timeSum -= 1 / 0.4f;

            Target.Healing(healthBar.FortressHealing);

            return true;
        }

        public bool HasInRange(IGameObject other)
        {
            return (Position - other.Position).Length < Range + other.Radius + Radius;
        }

        public float HealthLoss(float damage)
        {
            DamageKnight += damage;

            float bar = (0.095f / maxHealth) * 2;

            damage = DamageKnight * bar;

            return damage;
        }

        public float HealthWin(float heal)
        {
            HealKnight += heal;

            float bar = (0.095f / maxHealth) * 2;

            heal = -(HealKnight * bar);

            return heal;
        }

        public void Update(float time)
        {
            if (attackAnimation)
            {
                AttackAnimation(time);
            }
        }

        private bool UpdateAttack(float time)
        {
            timeSum += time;

            if (timeSum < 1 / AttackSpeed)
                return false;

            timeSum -= 1 / AttackSpeed;

            attackAnimation = true;

            return true;
        }

        private void AttackAnimation(float time)
        {
            if (Target == null)
            {
                Offset = new Vector2(0, 0);
                attackState = false;
                attackAnimation = false;
                return;
            }

            const float animationSpeed = 3f;

            Vector2 targetPos;
            targetPos.X = Target.Position.X + Target.Offset.X;
            targetPos.Y = Target.Position.Y + Target.Offset.Y;

            Vector2 pos;
            pos.X = Position.X + Offset.X;
            pos.Y = Position.Y + Offset.Y;

            Vector2 direction;
            direction.X = targetPos.X - pos.X;
            direction.Y = targetPos.Y - pos.Y;

            Vector2 offsetMove = direction * animationSpeed * time;

            if (!attackState)
            {
                if (direction.Length + offsetMove.Length < Radius + Target.Radius)
                {
                    Target.Damage(AttackDamage);
                    attackState = true;
                }
                else
                {
                    Offset += offsetMove;
                }
            }
            else
            {
                // TODO - check if vector of 'direction' has same direction as vector of 'Offset'
                if (true)
                {
                    Offset = new Vector2(0, 0);
                    attackState = false;
                    attackAnimation = false;
                }

/*                else
                {
                    Offset -= offsetMove;
                }*/
            }
        }
    }
}
