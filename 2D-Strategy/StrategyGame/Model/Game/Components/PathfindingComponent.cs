using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.Pathfinding;
using StrategyGame.Service;
using StrategyGame.Service.Pathfinding;
using StrategyGame.Service.Sound;

namespace StrategyGame.Model.Game.Components
{
    public class PathfindingComponent : IComponent
    {
        private static bool soundplayed = false;

        private List<Vector2> path;

        private int currentGoal;

        private IEntity destinationFlag;

        public IEntity Parent { get; }

        private readonly IServiceProvider serviceProvider;

        private int notMoved = 0;

        private void ShowFlag()
        {
            if (destinationFlag == null)
            {
                destinationFlag = ((IGameEntityFactory)Parent.Scene.EntityFactory).CreateDestiantionFlag(path[path.Count - 1]);
                Parent.AddChild(destinationFlag);
            }
        }

        private void RemoveFlag()
        {
            if (destinationFlag != null)
            {
                Parent.RemoveChild(destinationFlag);
                destinationFlag.Destroy();
                destinationFlag = null;
            }
        }

        public PathfindingComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
            serviceProvider = Parent.Scene.ServiceProvider;
            path = new List<Vector2>();
        }

        public void Pathfind(Vector2 point)
        {
            path = serviceProvider.GetService<IPathfinderService>().GetPath(Parent.Position, point);
            if (path.Count > 0)
            {
                currentGoal = 0;
                Parent.GetComponent<MoveComponent>().Destination = path[currentGoal];
            }
            else if (path.Count > 0 && Parent.Position == Parent.GetComponent<MoveComponent>().Destination)
            {
                currentGoal++;
                if (currentGoal < path.Count)
                {
                    Parent.GetComponent<MoveComponent>().Destination = path[currentGoal];
                }
            }
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            if (!Parent.GetComponent<MoveComponent>().Moving)
                notMoved++;
            else
                notMoved = 0;

            if (serviceProvider.GetService<IKeystateService>().IsButtonPressed(MouseButton.Right))
            {
                RemoveFlag();
                if (Parent.GetComponent<SelectComponent>().Selected && Parent.GetComponent<OwnerComponent>().Owner == 0)
                {
                    path = serviceProvider.GetService<IPathfinderService>().GetPath(Parent.Position, serviceProvider.GetService<IKeystateService>().MousePosition);
                    if (path.Count > 0)
                    {
                        currentGoal = 0;
                        Parent.GetComponent<MoveComponent>().Destination = path[currentGoal];
                        if (soundplayed == false)
                        {
                            ShowFlag();
                            serviceProvider.GetService<IAudioService>().PlaySound("command");
                            soundplayed = true;
                        }
                    }
                }
            }
            else if (path.Count > 0 && Parent.Position == Parent.GetComponent<MoveComponent>().Destination)
            {
                currentGoal++;
                if (currentGoal < path.Count)
                {
                    Parent.GetComponent<MoveComponent>().Destination = path[currentGoal];
                }
                else
                {
                    RemoveFlag();
                    path.Clear();
                }
            }

            if (serviceProvider.GetService<IKeystateService>().IsButtonReleased(MouseButton.Right))
            {
                soundplayed = false;
            }

            if (Parent.GetComponent<OwnerComponent>().Owner == 0)
            {
                if (Parent.GetComponent<SelectComponent>().Selected && path.Count > 0 && notMoved <= 2)
                {
                    ShowFlag();
                }
                else
                {
                    RemoveFlag();
                }
            }
        }

        public Vector2? Destination()
        {
            if (path.Count == 0)
                return null;
            return path[path.Count - 1];
        }

        public void UpdateFlag()
        {
            RemoveFlag();
            if (path.Count != 0)
                ShowFlag();
        }
    }
}
