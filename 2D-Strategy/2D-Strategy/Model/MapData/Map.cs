using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using StrategyGame.Model.MapData.MapComponents;
using StrategyGame.Model.MapData.Pathfinding;

namespace StrategyGame.Model.MapData
{
    public class Map : IGraph
    {
        public List<Node> Areas { get; private set; }

        public Map()
        {
            Areas = new List<Node>();
        }

        public void CreateArea(List<Vector2> corners)
        {
            int cornerCount = corners.Count;
            int nodeID = Areas.Count;

            this.Areas.Add(new Node());

            this.Areas[nodeID].nodeID = nodeID;
            this.Areas[nodeID].corners = corners;
            this.Areas[nodeID].center = new Vector2(0, 0);

            foreach (Vector2 corner in corners)
            {
                this.Areas[nodeID].center.X += corner.X;
                this.Areas[nodeID].center.Y += corner.Y;
            }

            this.Areas[nodeID].center.X /= cornerCount;
            this.Areas[nodeID].center.Y /= cornerCount;
        }

        public Node GetNode(int nodeID)
        {
            return Areas[nodeID];
        }

        public void AddComponent(int nodeID, IMapComponent component)
        {
            this.Areas[nodeID].components[component.GetType()] = component;
        }

        public void RemoveComponent<T>(int nodeID)
            where T : IMapComponent
        {
            this.Areas[nodeID].components.Remove(typeof(T));
        }

        public T GetComponent<T>(int nodeID)
            where T : IMapComponent
        {
            return (T)this.Areas[nodeID].components[typeof(T)];
        }

        public bool HasComponent(int nodeID, Type type)
        {
            return this.Areas[nodeID].components.ContainsKey(type);
        }

        public bool HasComponent<T>(int nodeID)
            where T : IMapComponent
        {
            return this.Areas[nodeID].components.ContainsKey(typeof(T));
        }

        public double Heuristic(int nodeID1, int nodeID2)
        {
            return EuclideanDistance(Areas[nodeID1].center, Areas[nodeID2].center);
        }

        public double EuclideanDistance(Vector2 p1, Vector2 p2)
        {
            double dx = Math.Abs(p1.X - p2.X);
            double dy = Math.Abs(p1.Y - p2.Y);
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        public bool IsPointInNode(int nodeID, Vector2 point)
        {
            bool result = false;
            int j = Areas[nodeID].corners.Count - 1;

            for (int i = 0; i < Areas[nodeID].corners.Count; i++)
            {
                if ((Areas[nodeID].corners[i].Y <= point.Y && Areas[nodeID].corners[j].Y >= point.Y) || (Areas[nodeID].corners[j].Y <= point.Y && Areas[nodeID].corners[i].Y >= point.Y))
                {
                    if (Areas[nodeID].corners[i].X + ((point.Y - Areas[nodeID].corners[i].Y) / (Areas[nodeID].corners[j].Y - Areas[nodeID].corners[i].Y) * (Areas[nodeID].corners[j].X - Areas[nodeID].corners[i].X)) <= point.X)
                    {
                        result = !result;
                    }
                }

                j = i;
            }

            return result;
        }

        public int GetNodeWithPoint(Vector2 point)
        {
            var nodeWithPoint = (from area in Areas where IsPointInNode(area.nodeID, point) select area).ToList();

            if (nodeWithPoint.Count > 0)
                return nodeWithPoint[0].nodeID;
            else
                return -1;
        }
    }
}
