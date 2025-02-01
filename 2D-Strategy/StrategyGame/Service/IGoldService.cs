using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Model.IService
{
    public interface IGoldService : IService
    {
        int GetGold(int owner);

        void AddGold(int owner, int amount);

        void RemoveGold(int owner, int amount);

        void Reset();
    }
}
