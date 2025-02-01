using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.MapData.MapComponents;

namespace StrategyGame.Model.MapData.Pathfinding
{
    /// <summary>
    /// Encapsulates functions for pathfinding algoriths
    /// </summary>
    public static class Pathfinder
    {
        /// <summary>
        /// Calculates a path on the graph
        /// If the path is optimal is dependent on the heuristic implemented in the graph
        /// </summary>
        /// <param name="graph">IGraph implementation to search the path on</param>
        /// <param name="start">node in IGraph where the algorithm should start</param>
        /// <param name="goal">node in IGraph where the path should lead</param>
        /// <returns>List of integers wich correspond with the node id's in the IGraph implementation</returns>
        public static List<int> AStarSearch(IGraph graph, int start, int goal)
        {
            if (start == -1)
                throw new System.ArgumentException("Parameter must be equal or larger than zero", "start");
            else if (goal == -1)
                throw new System.ArgumentException("Parameter must be equal or larger than zero", "goal");

            IPriorityQueue priorityQueue = new MinHeap();
            Dictionary<int, int> cameFrom = new Dictionary<int, int>();
            Dictionary<int, double> costSoFar = new Dictionary<int, double>();
            List<int> path = new List<int>();
            Neighbors component = new Neighbors();
            double newCost;
            double priority;
            int current;

            priorityQueue.Add(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;

            // as long as unvisited nodes are available
            do
            {
                // get element with the best value and remove it from the unvisited nodes
                current = priorityQueue.Poll();

                // test if the current node is the goal
                if (current == goal)
                    break;

                if (!graph.HasComponent<Neighbors>(current))
                {
                    continue;
                }

                component = graph.GetComponent<Neighbors>(current);

                // go through all nodes that are directly connected to the current one
                foreach (int next in component.GetNeighbors())
                {
                    // calculate cost for the current node
                    newCost = costSoFar[current] + component.GetCost(next);

                    // only add the node when it does not exist or when the cost is smaller than the current cost
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        // calculate priority and add node to frontier
                        costSoFar[next] = newCost;
                        priority = newCost + graph.Heuristic(next, goal);
                        priorityQueue.Add(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
            while (priorityQueue.Size() > 0);

            // reconstruct path by folloing cameFrom from the goal to the start
            path.Add(current);

            while (cameFrom[current] != start)
            {
                path.Add(cameFrom[current]);
                current = cameFrom[current];
            }

            path.Add(cameFrom[current]);
            path.Reverse();
            return path;
        }

        /// <summary>
        /// recalculates the points in the path to ensure a path without obstacles
        /// </summary>
        /// <param name="graph">IGrap implementation where the path is on</param>
        /// <param name="path">nodes that the path goes through</param>
        /// <param name="goal">Final goal of the path</param>
        /// <returns>points on the edge between the nodes</returns>
        public static List<Vector2> RefinePath(IGraph graph, List<int> path, Vector2 goal)
        {
            List<Vector2> refinedPath = new List<Vector2>();

            if (path.Count > 2 && path[0] != path[1])
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    refinedPath.Add(graph.GetComponent<Neighbors>(path[i]).GetConnectionPoints(path[i + 1])[0]);
                }
            }

            refinedPath.Add(goal);
            return refinedPath;
        }
    }
}
