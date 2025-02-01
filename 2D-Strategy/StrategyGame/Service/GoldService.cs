using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class GoldService : IGoldService
    {
        private Dictionary<int, int> gold;

        public GoldService()
        {
            gold = new Dictionary<int, int>();
        }

        public int GetGold(int owner)
        {
            return !gold.ContainsKey(owner) ? 0 : (int)gold[owner];
        }

        public void AddGold(int owner, int amount)
        {
            if (!gold.ContainsKey(owner))
                gold.Add(owner, 0);

            gold[owner] += amount;
        }

        public void RemoveGold(int owner, int amount)
        {
            if (!gold.ContainsKey(owner))
                gold.Add(owner, 0);

            gold[owner] -= amount;

            if (gold[owner] < 0)
                gold[owner] = 0;
        }

        public void Reset()
        {
            gold = new Dictionary<int, int>();
        }
    }
}
