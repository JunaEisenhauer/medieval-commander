using System;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class RandomService : IRandomService
    {
        private readonly Random random;

        public RandomService()
        {
            random = new Random();
        }

        public int GetRandomInt(int maxValue)
        {
            return random.Next(maxValue);
        }

        public float GetRandomFloat(float maxValue)
        {
            return (float)random.NextDouble() * maxValue;
        }
    }
}
