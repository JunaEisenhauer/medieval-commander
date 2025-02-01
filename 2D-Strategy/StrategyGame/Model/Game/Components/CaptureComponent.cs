using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Model.Game.Components
{
    public class CaptureComponent : IComponent
    {
        public IEntity Parent { get; }

        public CaptureComponent(IEntity parent)
        {
            Parent = parent;
            parent?.AddComponent(this);
        }

        public void Update()
        {
        }
    }
}
