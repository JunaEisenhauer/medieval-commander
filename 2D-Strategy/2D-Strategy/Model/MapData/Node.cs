using System;
using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.MapData.MapComponents;

namespace StrategyGame.Model.MapData
{
    public class Node
    {
        public int nodeID;

        public List<Vector2> corners;
        public Vector2 center;

        public Dictionary<Type, IMapComponent> components;

        public Node()
        {
            corners = new List<Vector2>();
            components = new Dictionary<Type, IMapComponent>();
        }
    }
}
