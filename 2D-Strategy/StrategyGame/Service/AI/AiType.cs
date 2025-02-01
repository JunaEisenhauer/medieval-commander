using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Service.AI
{
    /// <summary>
    /// enum to tell the <see cref="AIService"/> how the entity should be treated.
    /// </summary>
    public enum AiType
    {
        FastArmy = 0,
        Army = 1,
        RangedArmy = 2,
        Tower = 3,
        Fortress = 4,
    }
}
