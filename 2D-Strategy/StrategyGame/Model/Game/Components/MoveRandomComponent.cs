using System;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.Pathfinding;

namespace StrategyGame.Model.Game.Components
{
    public class MoveRandomComponent : IComponent
    {
        public IEntity Parent { get; }

        public MoveRandomComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
        }

        public void Update()
        {
            Vector2 destination;

            if (Parent.HasComponent<MoveComponent>())
            {
                for (int i = 0; i < 10; i++)
                {
                    var randomService = Parent.Scene.ServiceProvider.GetService<IRandomService>();
                    var x = (randomService.GetRandomFloat(Parent.Size.X) * 2) - Parent.Size.X;
                    var y = (randomService.GetRandomFloat(Parent.Size.Y) * 2) - Parent.Size.Y;

                    destination = Parent.Position + new Vector2(x, y);
                    if (Parent.Scene.ServiceProvider.GetService<IPathfinderService>().IsInNavMesh(destination))
                    {
                        Parent.GetComponent<MoveComponent>().Destination = Parent.Position + new Vector2(x, y);
                        break;
                    }

                    if (i == 9)
                        destination = Parent.Position;
                }
            }

            Parent.RemoveComponent<MoveRandomComponent>();
        }
    }
}
