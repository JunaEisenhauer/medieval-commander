using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace StrategyGame.Model.MapData.MapComponents
{
    public class Neighbors : IMapComponent
    {
        private class Edge
        {
            public readonly int neighborID;
            public readonly double cost;
            public readonly List<Vector2> connectionPoints;

            public Edge(int neighborID, double cost, List<Vector2> connectionPoints)
            {
                this.neighborID = neighborID;
                this.cost = cost;
                this.connectionPoints = connectionPoints;
            }
        }

        private List<Edge> neighbors;

        public Neighbors()
        {
            neighbors = new List<Edge>();
        }

        public void AddNeighbor(Node node, Node neighborNode, double cost)
        {
            neighbors.Add(new Edge(neighborNode.nodeID, cost, CalculateConnectionPoints(node.corners, neighborNode.corners)));
        }

        public void RemoveNeighbor(Node neighborNode)
        {
            var neighborIndex = from neighbor in neighbors where neighbor.neighborID == neighborNode.nodeID select neighbor;

            foreach (Edge neighbor in neighborIndex)
            {
                neighbors.Remove(neighbor);
            }
        }

        public List<int> GetNeighbors()
        {
            return neighbors.ConvertAll(e => e.neighborID);
        }

        public double GetCost(int neighborID)
        {
            int i = neighbors.FindIndex(x => x.neighborID == neighborID);
            return neighbors[i].cost;
        }

        public List<Vector2> CalculateConnectionPoints(List<Vector2> cornersNode1, List<Vector2> cornersNode2)
        {
            List<Vector2> connectionPoints = new List<Vector2>();
            Vector2 point = new Vector2(0, 0);

            var connections = (from corner in cornersNode1 where cornersNode2.Contains(corner) select corner).ToList();

            foreach (Vector2 corner in connections)
            {
                point.X += corner.X;
                point.Y += corner.Y;
            }
            
            point.X /= connections.Count;
            point.Y /= connections.Count;

            connectionPoints.Add(point);

            return connectionPoints;
        }

        public List<Vector2> GetConnectionPoints(int neighborID)
        {
            var vertexIndex = (from neighbor in neighbors where neighbor.neighborID == neighborID select neighbor).ToList();

            if (vertexIndex.Count > 0)
                return vertexIndex[0].connectionPoints;
            else
                return new List<Vector2>();
        }
    }
}
