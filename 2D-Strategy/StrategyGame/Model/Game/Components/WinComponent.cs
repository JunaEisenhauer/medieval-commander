using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model.IService;
using StrategyGame.Model.Menu;
using StrategyGame.Service;

namespace StrategyGame.Model.Game.Components
{
    public class WinComponent : IComponent
    {
        public IEntity Parent { get; }

        public bool CheckArmyCount { get; }

        public float Delay { get; }

        private bool winChecked;

        private bool cannotPlay;
        private float lostTime;

        public WinComponent(IEntity parent, bool checkArmyCount, float delay)
        {
            Parent = parent;
            parent?.AddComponent(this);
            CheckArmyCount = checkArmyCount;
            Delay = delay;
        }

        public void Update()
        {
            if (cannotPlay)
            {
                if (lostTime + Delay > Parent.Scene.ServiceProvider.GetService<ITimeService>().AbsoluteTime)
                    return;
                Parent.Scene.ServiceProvider.GetService<ISceneService>().ChangeScene("GameOver");
            }

            CheckFortress();
            if (CheckArmyCount)
                CanPlayOn();
        }

        private void CheckFortress()
        {
            if (!Parent.GetComponent<HealthComponent>().Dead)
                return;
            if (winChecked)
                return;
            winChecked = true;

            Console.WriteLine("Player " + Parent.GetComponent<OwnerComponent>().Owner + " has lost");
            if (Parent.GetComponent<OwnerComponent>().Owner == 0)
            {
                Parent.Scene.ServiceProvider.GetService<IGameStateService>().Won = true;
                Parent.Scene.ServiceProvider.GetService<ISceneService>().ChangeScene("GameOver");
            }

            IEntity winner = null;
            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (!other.HasComponent<WinComponent>())
                    continue;
                if (other.GetComponent<HealthComponent>().Dead)
                    continue;
                if (winner != null)
                    return;
                winner = other;
            }

            if (winner == null)
                return;

            Console.WriteLine("Player " + winner.GetComponent<OwnerComponent>().Owner + " has won the game");
            if (winner.GetComponent<OwnerComponent>().Owner == 0)
            {
                Parent.Scene.ServiceProvider.GetService<IGameStateService>().Won = true;
                Parent.Scene.ServiceProvider.GetService<ISceneService>().ChangeScene("GameOver");
            }
        }

        private void CanPlayOn()
        {
            if (!Parent.HasComponent<OwnerComponent>())
                return;
            var owner = Parent.GetComponent<OwnerComponent>().Owner;

            var gold = Parent.Scene.ServiceProvider.GetService<IGoldService>().GetGold(owner);
            var cheapest = (int)Enum.GetValues(typeof(ArmyPrice)).Cast<ArmyPrice>().Min();
            if (gold >= cheapest)
                return;

            foreach (var other in Parent.Scene.ServiceProvider.GetService<IGameObjectService>().GetEntities())
            {
                if (other.HasComponent<OwnerComponent>() && other.GetComponent<OwnerComponent>().Owner != owner)
                    continue;
                if (!other.HasComponent<HealthComponent>())
                    continue;
                if (other.GetComponent<HealthComponent>().ArmyType > 2)
                    continue;
                if (other.GetComponent<HealthComponent>().Dead)
                    continue;

                // Player still have an army
                return;
            }

            Console.WriteLine("Player " + owner + " has lost");
            if (owner == 0)
            {
                Parent.Scene.ServiceProvider.GetService<IGameStateService>().Won = false;
                lostTime = Parent.Scene.ServiceProvider.GetService<ITimeService>().AbsoluteTime;
                cannotPlay = true;
            }
        }
    }
}
