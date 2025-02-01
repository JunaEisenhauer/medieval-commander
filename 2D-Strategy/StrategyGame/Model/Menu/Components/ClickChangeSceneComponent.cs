using System;
using OpenTK.Input;
using StrategyGame.Model.Game;
using StrategyGame.Model.IService;

namespace StrategyGame.Model.Menu.Components
{
    public class ClickChangeSceneComponent : IComponent
    {
        public IEntity Parent { get; }

        public string Scene { get; }

        public ClickChangeSceneComponent(IEntity parent, string scene)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Scene = scene;
        }

        public void Update()
        {
            IKeystateService keyService = Parent.Scene.ServiceProvider.GetService<IKeystateService>();
            if (!keyService.IsButtonPressed(MouseButton.Left))
                return;
            if ((keyService.MousePosition - Parent.Position).Length > (Parent.Size.X / 2))
                return;

            Parent.Scene.ServiceProvider.GetService<ISceneService>().ChangeScene(Scene);
        }
    }
}
