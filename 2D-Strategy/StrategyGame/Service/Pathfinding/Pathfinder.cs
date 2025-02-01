using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using StrategyGame.Model.IService.Pathfinding;

namespace StrategyGame.Service.Pathfinding
{
    /// <summary>
    /// Class to provider pathfinding algorithms.
    /// </summary>
    public class Pathfinder : IPathfinderService
    {
        /// <summary>
        /// Representation of the Map which contains data relevent for the pathfinding algorithms.
        /// </summary>
        private IGraph graph;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pathfinder"/> class.
        /// </summary>
        public Pathfinder()
        {
            graph = new NavMesh();
        }

        /// <summary>
        /// Adds an area / polygon to the mapdata in <see cref="graph"/>.
        /// It is saved according to <see cref="IGraph.AddArea(List{Vector2}, bool)"/>.
        /// </summary>
        /// <param name="corner">List of points that are the corners of the polygon.</param>
        /// <param name="isObstacle">true of the polygon should be treaded as an obstacle, otherwise false.</param>
        public void AddArea(List<Vector2> corner, bool isObstacle)
        {
            graph.AddArea(corner, isObstacle);
        }

        /// <summary>
        /// deletes the current data in <see cref="graph"/> to allow for the creation of a new map.
        /// </summary>
        public void ClearAreas()
        {
            graph = new NavMesh();
        }

        /// <summary>
        /// Calculates the Path between the startpoint and the goal.
        /// </summary>
        /// <param name="start">point where the path starts.</param>
        /// <param name="goal">point where the path ends.</param>
        /// <returns>List of points to follow to get to the goal.</returns>
        public List<Vector2> GetPath(Vector2 start, Vector2 goal)
        {
            int node1 = graph.GetNodeWithPoint(goal);
            int node2 = graph.GetNodeWithPoint(start);
            if (node1 == -1)
            {
                return new List<Vector2>();
            }
            else if (node1 == node2)
            {
                return new List<Vector2>() { goal };
            }
            else
            {
                if (node2 == -1)
                {
                    node2 = graph.GetNearestNode(start);
                }

                return RefinePath(AStarSearch(node1, node2, goal), goal, start);
            }
        }

        /// <summary>
        /// Tests if the specified point is inside of the map or not.
        /// </summary>
        /// <param name="p">point to test.</param>
        /// <returns>true if the point is inside the map, false otherwise.</returns>
        public bool IsInNavMesh(Vector2 p)
        {
            if (graph.GetNodeWithPoint(p) == -1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Implementation of the A* Algorithms to find a path from the start to the goal node.
        /// </summary>
        /// <param name="start">id of the startnode.</param>
        /// <param name="goal">id of the endnode.</param>
        /// <param name="p">goal of the path.</param>
        /// <returns>List of ids of nodes to follow to get to the goal node.</returns>
        public List<int> AStarSearch(int start, int goal, Vector2 p)
        {
            IPriorityQueue<int> priorityQueue = new MinHeap<int>();

            Dictionary<int, int> cameFrom = new Dictionary<int, int>();
            Dictionary<int, double> costSoFar = new Dictionary<int, double>();

            List<int> path = new List<int>();

            double newCost;
            double priority;
            int current;

            cameFrom[start] = start;
            costSoFar[start] = 0;

            priorityQueue.Add(0, start);

            do
            {
                current = priorityQueue.Pop();

                if (current == goal)
                    break;

                foreach (int next in graph.GetNeighbors(current))
                {
                    newCost = costSoFar[current] + graph.GetEdgeCost(current, next);

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        priority = newCost + graph.Heuristic(next, p);
                        priorityQueue.Add(priority, next);
                        cameFrom[next] = current;
                    }
                }
            }
            while (priorityQueue.Count() > 0);

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
        /// Calculates the euclidean distance between two points.
        /// </summary>
        /// <param name="firstPoint">first point.</param>
        /// <param name="secondPoint">second point.</param>
        /// <returns>euclidean distance bewteen the two points.</returns>
        public double EuclideanDistance(Vector2 firstPoint, Vector2 secondPoint)
        {
            double dx = Math.Abs(firstPoint.X - secondPoint.X);
            double dy = Math.Abs(firstPoint.Y - secondPoint.Y);
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        /// <summary>
        /// Calculates the points to follow to get to the goal from a list of nodes that leads to the node in which the goal is.
        /// If a point does not need to be walked to in order to get to the goal without running into an obstacle it is removed.
        /// </summary>
        /// <param name="path">List of nodes that leads to the node in which the goal is.</param>
        /// <param name="goal">point where the path should end.</param>
        /// <param name="start">point where the path should start.</param>
        /// <returns>List of points to follow to get to the goal without running into an obstacle.</returns>
        public List<Vector2> RefinePath(List<int> path, Vector2 goal, Vector2 start)
        {
            List<Vector2> refinedPath = new List<Vector2>();
            Vector2? bestPoint = null;

            path.Reverse();

            if (path.Count > 2 && path[0] != path[1])
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    foreach (Vector2 point in graph.GetConnectionPoints(path[i], path[i + 1]))
                    {
                        if (!bestPoint.HasValue)
                            bestPoint = point;
                        else if (EuclideanDistance(point, goal) < EuclideanDistance((Vector2)bestPoint, goal))
                            bestPoint = point;
                    }

                    refinedPath.Add((Vector2)bestPoint);
                    bestPoint = null;
                }
            }

            refinedPath.Add(goal);

            refinedPath = refinedPath.Distinct().ToList();

            return RemoveUnnecessaryPoints(refinedPath, start);
        }

        /// <summary>
        /// Calculates which points can be left out of a path without running into an obstacle.
        /// </summary>
        /// <param name="path">path to simplify.</param>
        /// <param name="start">point where the path starts.</param>
        /// <returns>minimal List of points to follow to get to the goal without running into an obstacle.</returns>
        public List<Vector2> RemoveUnnecessaryPoints(List<Vector2> path, Vector2 start)
        {
            List<Vector2> optimizedPath = new List<Vector2>();

            int currentPoint;
            Vector2 pathPoint = RemovePoints(path, start);
            if (pathPoint == path[path.Count - 1])
            {
                optimizedPath.Add(pathPoint);
                return optimizedPath;
            }
            else if (pathPoint == start)
            {
                optimizedPath.Add(path[0]);
            }
            else
            {
                optimizedPath.Add(pathPoint);
            }

            while (true)
            {
                currentPoint = path.FindIndex(x => x == optimizedPath[optimizedPath.Count - 1]);

                pathPoint = RemovePoints(path, path[currentPoint]);

                if (pathPoint == path[path.Count - 1])
                {
                    optimizedPath.Add(pathPoint);
                    break;
                }
                else if (pathPoint == path[currentPoint])
                {
                    currentPoint++;
                    optimizedPath.Add(path[currentPoint]);
                }
                else
                {
                    optimizedPath.Add(pathPoint);
                }
            }

            return optimizedPath;
        }

        /// <summary>
        /// Calculates the furthest point of the path that can be reached from the start without running into an obstacle.
        /// </summary>
        /// <param name="path">path to test.</param>
        /// <param name="start">point to test.</param>
        /// <returns>point with the largest distance from the start that can be reached without obstacles.</returns>
        public Vector2 RemovePoints(List<Vector2> path, Vector2 start)
        {
            for (int i = path.Count - 1; i >= 0; i--)
            {
                if (graph.HasLineOfSight(start, path[i]))
                {
                    return path[i];
                }
            }

            return start;
        }
    }
}
