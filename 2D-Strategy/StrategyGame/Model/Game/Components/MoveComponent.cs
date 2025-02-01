using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model.Game.Components
{
    public class MoveComponent : IComponent
    {
        public IEntity Parent { get; }

        public float Speed { get; set; }

        private Vector2? destination;

        public Vector2? Destination
        {
            get => destination;
            set
            {
                if (value == null)
                {
                    destination = null;
                    Moving = false;
                    return;
                }

                var dest = value.Value;

                EntityDestination? obstacle = null;
                float lastLength = (Parent.Position - dest).Length;
                do
                {
                    obstacle = FindTargetOnPosition(dest);
                    if (obstacle != null)
                    {
                        var direction = obstacle.Value.destination - dest;
                        if (direction == Vector2.Zero)
                            direction = dest - Parent.Position;
                        if (direction == Vector2.Zero)
                        {
                            destination = dest;
                            Moving = false;
                            return;
                        }

                        dest = dest - ((direction.Normalized() * (obstacle.Value.entity.Size.X / 2)) * 0.7f);
                        var length = (Parent.Position - dest).Length;
                        if (length >= lastLength)
                            break;
                        lastLength = length;
                    }
                }
                while (obstacle != null);

                destination = dest;
                Moving = true;
            }
        }

        public bool Moving { get; set; }

        public MoveComponent(IEntity parent, float speed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Speed = speed;
        }

        public void Update()
        {
            if (!Moving)
                return;
            if (!Destination.HasValue)
                return;

            if (Destination.Value.X < 0 || Destination.Value.Y < 0)
            {
                Destination = null;
                return;
            }

            var mapService = Parent.Scene.ServiceProvider.GetService<IMapService>();
            if (Destination.Value.X > mapService.Width || Destination.Value.Y > mapService.Height)
            {
                Destination = null;
                return;
            }

            if (Parent.HasComponent<HealthComponent>() && Parent.GetComponent<HealthComponent>().Dead)
            {
                Moving = false;
                return;
            }

            if (Destination == Parent.Position)
            {
                Moving = false;
                return;
            }

            var movingDestination = (Destination.Value - Parent.Position).Normalized() * Speed *
                                    Parent.Scene.ServiceProvider.GetService<ITimeService>().DeltaTime;

            if (movingDestination.LengthSquared >
                (Destination.Value - Parent.Position).LengthSquared)
            {
                Parent.Position = Destination.Value;
                Moving = false;
                return;
            }

            Parent.Position += movingDestination;
        }

        private EntityDestination? FindTargetOnPosition(Vector2 position)
        {
            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (!other.HasComponent<CollisionComponent>())
                    continue;
                if (other.HasComponent<OwnerComponent>() && Parent.HasComponent<OwnerComponent>() && other.GetComponent<OwnerComponent>().Owner != Parent.GetComponent<OwnerComponent>().Owner)
                    continue;

                if (other.HasComponent<MoveComponent>() && other.GetComponent<MoveComponent>().Moving)
                {
                    if ((other.GetComponent<MoveComponent>().Destination.Value - position).Length < (other.Size.X / 2) * 0.7f)
                    {
                        return new EntityDestination { entity = other, destination = other.GetComponent<MoveComponent>().Destination.Value };
                    }
                }
                else
                {
                    if ((other.Position - position).Length < (other.Size.X / 2) * 0.7f)
                    {
                        return new EntityDestination { entity = other, destination = other.Position };
                    }
                }
            }

            return null;
        }

        private struct EntityDestination
        {
            public IEntity entity;
            public Vector2 destination;
        }
    }
}
