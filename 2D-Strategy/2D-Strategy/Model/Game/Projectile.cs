using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Game
{
    public class Projectile : IEntity
    {
        private readonly float projectileSpeed;

        public Projectile(IGameObject shooter, IGameObject target, int damage)
        {
            Id = EntityType.Projectile;
            Position = shooter.Position;
            Shooter = shooter;
            Target = target;
            Damage = damage;
            projectileSpeed = 0.02f;
        }

        public EntityType Id { get; }

        public Vector2 Position { get; private set; }

        public IGameObject Shooter { get; }

        public IGameObject Target { get; }

        public int Damage { get; }

        public bool UpdatePosition()
        {
            Vector2 direction = Target.Position - Position;
            if ((direction.X * direction.X) + (direction.Y * direction.Y) < projectileSpeed * projectileSpeed)
            {
                Target.Damage(Damage);
                return false;
            }

            direction.Normalize();
            Position += direction * projectileSpeed;
            return true;
        }
    }
}
