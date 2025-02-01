using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Model.IService
{
    public interface IGameStateService : IService
    {
        bool Running { get; set; }

        bool Won { get; set; }
    }
}
