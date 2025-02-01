using System;
using StrategyGame.Model.MapData.MapComponents;

namespace StrategyGame.Model.MapData.Pathfinding
{
    public interface IGraph
    {
        double Heuristic(int nodeID1, int nodeID2);

        T GetComponent<T>(int nodeID)
            where T : IMapComponent;

        bool HasComponent(int nodeID, Type type);

        bool HasComponent<T>(int nodeID)
            where T : IMapComponent;
    }
}
