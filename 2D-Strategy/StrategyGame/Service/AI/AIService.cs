using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.AI;

namespace StrategyGame.Service.AI
{
    /// <summary>
    /// Class to define and execute behaviors of the enemy units
    /// as well as the buying of new enemy units.
    /// </summary>
    public class AIService : IAIService
    {
        /// <summary>
        /// Customs data type to store the current behavior pattern of the enemy.
        /// Currently Unused.
        /// </summary>
        private enum State
        {
            Attacking = 0,
            Defending = 1,
        }

        /// <summary>
        /// List to store references to all entities that are relevant for the enemy to make decisions.
        /// </summary>
        private readonly List<IEntity> entities;

        /// <summary>
        /// Reference to the serviceProvider <see cref="IServiceProvider"/>.
        /// </summary>
        private readonly Model.IService.IServiceProvider serviceProvider;

        private Dictionary<int, float> requiredTowerBalance;
        private Dictionary<int, float> currentTowerBalance;

        private int starttime;

        /// <summary>
        /// Calculates the euclidian distance between two vectors.
        /// </summary>
        /// <param name="p1">first vector.</param>
        /// <param name="p2">second vector.</param>
        /// <returns>Euclidian Distance between the two vectors.</returns>
        private double EuclideanDistance(Vector2 p1, Vector2 p2)
        {
            return Vector2.Distance(p1, p2);
        }

        /// <summary>
        /// Searches the <see cref="entities"/> list and returns the Fortress with the specified owner.
        /// </summary>
        /// <param name="owner">owner <see cref="OwnerComponent"/> of the searched Fortress.</param>
        /// <returns>reference to the searched fortress as <see cref="IEntity"/>.</returns>
        private int GetFortress(int owner)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].GetComponent<AIComponent>().Type == AiType.Fortress && entities[i].GetComponent<OwnerComponent>().Owner == owner)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Checks if the enemy has enough money to buy an army and buys armys accordingly.
        /// All armies are bough at the fortress and are chosen randomly.
        /// Knights and archers have a 37.5% chance.
        /// </summary>
        private void BuyArmys()
        {
            int rand;
            int fortress = GetFortress(1);

            if (fortress == -1)
                return;

            if (serviceProvider.GetService<IGoldService>().GetGold(1) >= 100)
            {
                rand = serviceProvider.GetService<IRandomService>().GetRandomInt(6);
                if (rand < 3)
                {
                    entities[fortress].GetComponent<BuyArmyComponent>().BuyArmy(ArmyPrice.Knight);
                }
                else if (rand < 6)
                {
                    entities[fortress].GetComponent<BuyArmyComponent>().BuyArmy(ArmyPrice.Archer);
                }
            }
        }

        private bool IsArmy(int index)
        {
            AiType type = entities[index].GetComponent<AIComponent>().Type;

            switch (type)
            {
                case AiType.Army:
                case AiType.FastArmy:
                case AiType.RangedArmy:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsTower(int index)
        {
            AiType type = entities[index].GetComponent<AIComponent>().Type;

            switch (type)
            {
                case AiType.Tower:
                case AiType.Fortress:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Calculates the tower that is nearest to the given position.
        /// </summary>
        /// <param name="towerowner">tower owner.</param>
        /// <param name="position">position.</param>
        /// <returns>returns closest tower.</returns>
        private int NearestTower(int towerowner, Vector2 position)
        {
            bool isSet = false;
            int closestTower = -1;
            double closetDistance = 0;

            for (int i = 0; i < entities.Count; i++)
            {
                if (IsTower(i) && entities[i].GetComponent<OwnerComponent>().Owner == towerowner)
                {
                    if (!isSet)
                    {
                        isSet = true;
                        closestTower = i;
                        closetDistance = EuclideanDistance(position, entities[i].Position);
                    }
                    else
                    {
                        if (EuclideanDistance(position, entities[i].Position) < closetDistance)
                        {
                            closestTower = i;
                            closetDistance = EuclideanDistance(position, entities[i].Position);
                        }
                    }
                }
            }

            return closestTower;
        }

        /// <summary>
        /// Calculates the three towers that need to be defended the most and returns 
        /// a dictionary where the key is equal to the index in <see cref="entities"/>
        /// and the value the amount of pressure the tower is under.
        /// </summary>
        /// <returns>returns towers.</returns>
        private Dictionary<int, int> CalculateTowersToDefend()
        {
            Dictionary<int, int> towers = new Dictionary<int, int>();
            int nearestTower;
            Dictionary<int, int> towersToDefend = new Dictionary<int, int>();

            for (int i = 0; i < entities.Count; i++)
            {
                if (IsArmy(i) && entities[i].GetComponent<OwnerComponent>().Owner == 0)
                {
                    nearestTower = NearestTower(1, entities[i].Position);

                    if (towers.ContainsKey(nearestTower))
                    {
                        towers[nearestTower] += 1;
                    }
                    else
                    {
                        towers.Add(nearestTower, 1);
                    }
                }
            }

            return towers;
        }

        private Dictionary<int, int> CalculateArmyBalance()
        {
            Dictionary<int, int> towers = new Dictionary<int, int>();
            int tower;

            for (int i = 0; i < entities.Count; i++)
            {
                if (IsArmy(i) && entities[i].GetComponent<OwnerComponent>().Owner == 1)
                {
                    tower = entities[i].GetComponent<AIComponent>().TowerToDefend;

                    if (tower == -1)
                        continue;

                    if (towers.ContainsKey(tower))
                    {
                        towers[tower] += 1;
                    }
                    else
                    {
                        towers.Add(tower, 1);
                    }
                }
            }

            return towers;
        }

        private void UpdateArmy(int index, int tower)
        {
            entities[index].GetComponent<AIComponent>().TowerToDefend = tower;
            entities[index].GetComponent<PathfindingComponent>().Pathfind(entities[tower].Position);
        }

        private int GetArmyAmount(int owner)
        {
            int counter = 0;

            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].HasComponent<OwnerComponent>())
                {
                    if (entities[i].GetComponent<OwnerComponent>().Owner == owner)
                    {
                        if (IsArmy(i))
                        {
                            counter++;
                        }
                    }
                }
            }

            return counter;
        }

        private float GetPercentage(float max, float current)
        {
            return (current / max) * 100;
        }

        private void CalculateBalance()
        {
            requiredTowerBalance.Clear();
            currentTowerBalance.Clear();

            Dictionary<int, int> towerEnemyBalance = CalculateTowersToDefend();
            Dictionary<int, int> towerArmyBalance = CalculateArmyBalance();
            int armyEnemyCount = GetArmyAmount(1);
            int armyPlayerCount = GetArmyAmount(0);

            foreach (var tower in towerEnemyBalance)
            {
                requiredTowerBalance.Add(tower.Key, GetPercentage(armyPlayerCount, tower.Value));
            }

            foreach (var tower in towerArmyBalance)
            {
                currentTowerBalance.Add(tower.Key, GetPercentage(armyEnemyCount, tower.Value));
            }
        }

        private List<int> GetUnnessesaryArmys()
        {
            List<int> armys = new List<int>();
            float minnimum = GetPercentage(GetArmyAmount(1), 1);

            foreach (var tower in requiredTowerBalance)
            {
                if (currentTowerBalance.ContainsKey(tower.Key))
                {
                    if (currentTowerBalance[tower.Key] - tower.Value > minnimum)
                    {
                        for (int i = 0; i < entities.Count; i++)
                        {
                            if (IsArmy(i) && entities[i].GetComponent<OwnerComponent>().Owner == 1)
                            {
                                if (entities[i].GetComponent<AIComponent>().TowerToDefend == tower.Key)
                                {
                                    armys.Add(i);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        if (IsArmy(i) && entities[i].GetComponent<OwnerComponent>().Owner == 1)
                        {
                            if (entities[i].GetComponent<AIComponent>().TowerToDefend == tower.Key)
                            {
                                armys.Add(i);
                            }
                        }
                    }
                }
            }

            return armys;
        }

        private List<int> GetUnderdefendedTowers()
        {
            List<int> towers = new List<int>();
            float minnimum = GetPercentage(GetArmyAmount(1), 1);

            foreach (var tower in requiredTowerBalance)
            {
                if (currentTowerBalance.ContainsKey(tower.Key))
                {
                    if (tower.Value - currentTowerBalance[tower.Key] >= minnimum)
                    {
                        towers.Add(tower.Key);
                    }
                }
                else
                {
                    if (tower.Value > minnimum)
                    {
                        towers.Add(tower.Key);
                    }
                }
            }

            return towers;
        }

        private List<int> GetFreeArmys()
        {
            List<int> armys = new List<int>();

            for (int i = 0; i < entities.Count; i++)
            {
                if (IsArmy(i) && entities[i].GetComponent<OwnerComponent>().Owner == 1)
                {
                    if (entities[i].GetComponent<AIComponent>().TowerToDefend == -1)
                    {
                        armys.Add(i);
                    }
                }
            }

            return armys;
        }

        private void UpdateBalance()
        {
            List<int> freeArmys = GetFreeArmys();
            List<int> armysAtTower = GetUnnessesaryArmys();
            List<int> underdefendedTowers = GetUnderdefendedTowers();

            foreach (int i in underdefendedTowers)
            {
                if (freeArmys.Count > 0)
                {
                    UpdateArmy(freeArmys[freeArmys.Count - 1], i);
                }
                else if (armysAtTower.Count > 0)
                {
                    UpdateArmy(armysAtTower[armysAtTower.Count - 1], i);
                }
            }
        }

        private void UpdateReferences()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (IsArmy(i) && entities[i].GetComponent<OwnerComponent>().Owner == 1)
                {
                    if (entities.Count >= entities[i].GetComponent<AIComponent>().TowerToDefend && entities[i].GetComponent<AIComponent>().TowerToDefend != -1)
                    {
                        if (entities[entities[i].GetComponent<AIComponent>().TowerToDefend].GetComponent<OwnerComponent>().Owner == 0)
                        {
                            entities[i].GetComponent<AIComponent>().TowerToDefend = -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AIService"/> class.
        /// </summary>
        /// <param name="serviceProvider">Serviceprovider of the scene in wich the ai is supposed to operate in.</param>
        public AIService(Model.IService.IServiceProvider serviceProvider)
        {
            entities = new List<IEntity>();
            starttime = 0;
            this.serviceProvider = serviceProvider;
            requiredTowerBalance = new Dictionary<int, float>();
            currentTowerBalance = new Dictionary<int, float>();
        }

        /// <summary>
        /// Function to register an entity as relevant for the behavior of the enemy.
        /// </summary>
        /// <param name="entity">entity that is relevant.</param>
        public void Register(IEntity entity)
        {
            this.entities.Add(entity);
        }

        /// <summary>
        /// Function that is called each Frame and initiates all behaviors of the enemy.
        /// </summary>
        public void Update()
        {
            if (starttime < 180 && entities.Count > 0)
            {
                starttime++;
                entities.Clear();
                return;
            }
            else if (starttime >= 180)
            {
                BuyArmys();

                UpdateReferences();

                CalculateBalance();

                UpdateBalance();

                entities.Clear();
            }
        }
    }
}
