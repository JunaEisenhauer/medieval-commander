using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Model.Game.Components
{
    public class CollisionComponent : IComponent
    {
        public IEntity Parent { get; }

        public CollisionComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
        }

        public void Update()
        {
        }
    }
}
