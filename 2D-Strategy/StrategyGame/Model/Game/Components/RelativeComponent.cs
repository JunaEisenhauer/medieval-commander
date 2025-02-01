using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Game.Components
{
    public class RelativeComponent : IComponent
    {
        public IEntity Parent { get; }

        public IEntity Follow { get; }

        public Vector2 Offset { get; }

        public RelativeComponent(IEntity parent, IEntity follow, Vector2 offset)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Follow = follow;
            Offset = offset;
        }

        public void Update()
        {
            Parent.Position = Follow.Position + Offset;
        }
    }
}
