using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Model
{
    public class CGameLayer
    {
        public Layer Layer { get; }

        public CGameLayer(Layer layer)
        {
            Layer = layer;
        }
    }

    public enum Layer
    {
        Ground,
        GameObjects,
        Ui,
    }
}
