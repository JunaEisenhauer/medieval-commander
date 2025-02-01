using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using StrategyGame.Model.IService;

namespace StrategyGame.Model.Components
{
    public class ButtonComponent : IComponent
    {
        public IEntity Parent { get; }

        private readonly Action callback;

        public ButtonComponent(IEntity parent, Action callback)
        {
            Parent = parent;
            parent?.AddComponent(this);
            this.callback = callback;
        }

        public void Update()
        {
            IKeystateService keyService = Parent.Scene.ServiceProvider.GetService<IKeystateService>();

            if (!keyService.IsButtonPressed(MouseButton.Left))
                return;

            var x = keyService.MousePosition.X;
            var y = keyService.MousePosition.Y;

            if (x > Parent.Position.X + (Parent.Size.X / 2))
                return;
            if (x < Parent.Position.X - (Parent.Size.X / 2))
                return;
            if (y > Parent.Position.Y + (Parent.Size.Y / 2))
                return;
            if (y < Parent.Position.Y - (Parent.Size.Y / 2))
                return;

            callback.Invoke();
        }
    }
}
