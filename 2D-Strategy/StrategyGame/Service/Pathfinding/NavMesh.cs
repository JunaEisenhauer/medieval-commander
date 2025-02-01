using System.Collections.Generic;
using System.Linq;
using OpenTK;
using StrategyGame.Model.IService.Pathfinding;

namespace StrategyGame.Service.Pathfinding
{
    /// <summary>
    /// Implementation of a Navigation Mesh (a collection of connected polygons).
    /// The Navigation Mesh is used to define walkable areas on the map as well as obstacles.
    /// </summary>
    public class NavMesh : IGraph
    {
        /// <summary>
        /// Private custom data type to store the edges between polygons / Nodes <see cref="Node.neighbors"/>.
        /// </summary>
        private class Edge
        {
            public int neighborID;
            public List<Vector2> connectionPoints;
        }

        /// <summary>
        /// Private custom data type to store a polygon of the NavMesh.
        /// </summary>
        private class Node
        {
            public int nodeID;
            public bool isObstacle;
            public List<Vector2> corners;
            public Vector2 center;
            public List<Edge> neighbors;
        }

        /// <summary>
        /// List with all polygons of the NavMesh which also function as the nodes of the graph.
        /// </summary>
        private readonly List<Node> nodes;

        /// <summary>
        /// Calculates the Points which connect one polygon with another.
        /// The two points are calculated by determining the corners that the two polyons share.
        /// </summary>
        /// <param name="nodeID1">Id of the polygon.</param>
        /// <param name="nodeID2">Id of the polygon that is connected to the polygon specified in the first parameter.</param>
        /// <returns>List of points that connect the two specified polygons.</returns>
        private List<Vector2> CalculateConnectionPoints(int nodeID1, int nodeID2)
        {
            Vector2 point = new Vector2(0, 0);
            List<Vector2> connectionPoints = new List<Vector2>();
            IEnumerable<Vector2> query = nodes[nodeID1].corners.Where(value => nodes[nodeID2].corners.Contains(value));

            foreach (Vector2 value in query)
            {
                connectionPoints.Add(value);
                point.X += value.X;
                point.Y += value.Y;
            }

            point.X /= query.Count();
            point.Y /= query.Count();
            connectionPoints.Add(point);

            if (connectionPoints.Count == 3)
            {
                return connectionPoints;
            }
            else
            {
                return new List<Vector2>();
            }
        }

        /// <summary>
        /// Adds an edge between two polygons.
        /// The edge is added in both directions.
        /// </summary>
        /// <param name="nodeID1">Id of the first node.</param>
        /// <param name="nodeID2">Id of the second node that should be connected to the first node.</param>
        /// <param name="connectionPoints">List of points which connect the two nodes.</param>
        private void AddEdge(int nodeID1, int nodeID2, List<Vector2> connectionPoints)
        {
            int edgeID = nodes[nodeID1].neighbors.Count;
            nodes[nodeID1].neighbors.Add(new Edge());
            nodes[nodeID1].neighbors[edgeID].neighborID = nodeID2;
            nodes[nodeID1].neighbors[edgeID].connectionPoints = connectionPoints;
            edgeID = nodes[nodeID2].neighbors.Count;
            nodes[nodeID2].neighbors.Add(new Edge());
            nodes[nodeID2].neighbors[edgeID].neighborID = nodeID1;
            nodes[nodeID2].neighbors[edgeID].connectionPoints = connectionPoints;
        }

        /// <summary>
        /// Calculates the nodes wich are connected to the specified node
        /// and adds the corresponging edges to the data stucture <see cref="Node"/>.
        /// To calculate the neighbors the corners of the polygona are compared to the corners of the other
        /// polygons. When two corners are similar, a connection between the two nodes is added.
        /// </summary>
        /// <param name="nodeID">Id of the node with unknown neighbors.</param>
        private void CalculateNeighbors(int nodeID)
        {
            List<Vector2> connectionPoints;

            if (!nodes[nodeID].isObstacle)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (i == nodeID)
                        continue;

                    if (!nodes[i].isObstacle)
                    {
                        connectionPoints = CalculateConnectionPoints(nodeID, i);
                        if (connectionPoints.Count == 3)
                        {
                            AddEdge(nodeID, i, connectionPoints);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the orientation in wich the parameters are arranged.
        /// The points are arranged colinear if a line can be drawn where p, q and r are on.
        /// The points are arranged clockwise if route from p to q to r is clockwise.
        /// The points are arranged counter clockwise if the route from p to q to r is counter clockwise.
        /// </summary>
        /// <param name="p">first point.</param>
        /// <param name="q">second point.</param>
        /// <param name="r">third point.</param>
        /// <returns>0 if the points are colinear, 1 if they are clockwise, 2 if they are counter clockwise.</returns>
        private int Orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float result = ((q.X - p.X) * (r.Y - p.Y)) - ((r.X - p.X) * (q.Y - p.Y));

            if (result == 0f)
            {
                return 0; // colinear
            }

            return (result > 0) ? 1 : 2; // clock or counterclock wise
        }

        /// <summary>
        /// Determins whether q is on the segment p1,p2 or not.
        /// </summary>
        /// <param name="p1">startpoint of the segment.</param>
        /// <param name="p2">endpoint of the segment.</param>
        /// <param name="q">point to test.</param>
        /// <returns>true if q is on segment p1,p2, false if not.</returns>
        private bool OnSegment(Vector2 p1, Vector2 p2, Vector2 q)
        {
            if (q.X <= Max(p1.X, p2.X) && q.X >= Min(p1.X, p2.X) && q.Y <= Max(p1.Y, p2.Y) && p2.Y >= Min(p1.Y, p2.Y))
                return true;
            return false;
        }

        /// <summary>
        /// Determins and returns the float that is smaller then the other float.
        /// </summary>
        /// <param name="x">first float to compare.</param>
        /// <param name="y">second float to compare.</param>
        /// <returns>The smaller float of the two floats.</returns>
        private float Min(float x, float y)
        {
            return x < y ? x : y;
        }

        /// <summary>
        /// Determins and returns the float that is larger than the other float.
        /// </summary>
        /// <param name="x">first float to compare.</param>
        /// <param name="y">second float to comapre.</param>
        /// <returns>The larger float of the two floats.</returns>
        private float Max(float x, float y)
        {
            return x > y ? x : y;
        }

        /// <summary>
        /// Determins if the segment p1,q1 intersects the segment p2,q2.
        /// </summary>
        /// <param name="p1">starpoint of the first segment.</param>
        /// <param name="q1">endpoint of the first segment.</param>
        /// <param name="p2">startpoint of the second segment.</param>
        /// <param name="q2">endpoint of the second segment.</param>
        /// <returns>
        /// -1 if the start or endpoint of the first segment is equal to the start or endpoint of the second segment,
        /// 1 if the two segments intersect.
        /// 0 if the two segments do not intersect.
        /// </returns>
        private int Intersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
        {
            if (p1 == p2 || p1 == q2)
            {
                if (q1 != p2 && q1 != q2)
                    return -1;
            }
            else if (q1 == p2 || q1 == q2)
            {
                if (p1 != p2 && p1 != q2)
                    return -1;
            }           

            // calculate orientations
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // general case, orientations are opposite of each other
            if (o1 != o2 && o3 != o4)
                return 1;
            
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, q1, p2))
                return 1;
            
            // p1, q1 and q2 are colinear and q1 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q1, q2))
                return 1;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, q2, p1))
                return 1;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(p2, q2, q1))
                return 1;
                
            return 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavMesh"/> class.
        /// </summary>
        public NavMesh()
        {
            nodes = new List<Node>();
        }

        /// <summary>
        /// Adds a polygon / node to the NavMesh.
        /// </summary>
        /// <param name="corners">List of points that are the corners of the polygon.</param>
        /// <param name="isObstacle">true of the polygon should be treaded as an obstacle, otherwise false.</param>
        public void AddArea(List<Vector2> corners, bool isObstacle)
        {
            int cornerCount = corners.Count;
            int nodeID = nodes.Count;

            this.nodes.Add(new Node());

            this.nodes[nodeID].nodeID = nodeID;
            this.nodes[nodeID].isObstacle = isObstacle;
            this.nodes[nodeID].corners = corners;
            this.nodes[nodeID].center = new Vector2(0, 0);
            this.nodes[nodeID].neighbors = new List<Edge>();

            foreach (Vector2 corner in corners)
            {
                this.nodes[nodeID].center.X += corner.X;
                this.nodes[nodeID].center.Y += corner.Y;
            }

            this.nodes[nodeID].center.X /= cornerCount;
            this.nodes[nodeID].center.Y /= cornerCount;

            CalculateNeighbors(nodeID);
        }

        /// <summary>
        /// Gets the points that connect two polygons / nodes.
        /// </summary>
        /// <param name="firstNodeID">Id of the first node.</param>
        /// <param name="secondNodeID">Id of the second node.</param>
        /// <returns>List of points that connect the two polygons.</returns>
        public List<Vector2> GetConnectionPoints(int firstNodeID, int secondNodeID)
        {
            var vertexIndex = (from neighbor in nodes[firstNodeID].neighbors where neighbor.neighborID == secondNodeID select neighbor).ToList();

            if (vertexIndex.Count > 0)
                return vertexIndex[0].connectionPoints;
            else
                return new List<Vector2>();
        }

        /// <summary>
        /// Gets the cost of the edge between two polygons.
        /// Currently not used.
        /// </summary>
        /// <param name="firstNodeID">Id of the first node.</param>
        /// <param name="secondNodeID">Id of the second node.</param>
        /// <returns>Cost of the edge between the two polygons.</returns>
        public double GetEdgeCost(int firstNodeID, int secondNodeID)
        {
            return 0;
        }

        /// <summary>
        /// Gets the ids of the polygons that neighbor the specified polygon.
        /// </summary>
        /// <param name="nodeID">Id of the node.</param>
        /// <returns>List of nodeIds of the neighboring polygons.</returns>
        public List<int> GetNeighbors(int nodeID)
        {
            return nodes[nodeID].neighbors.ConvertAll(e => e.neighborID);
        }

        /// <summary>
        /// Function to check whether a obstacle is between the points p and q or not.
        /// </summary>
        /// <param name="p">startpoint of the segment.</param>
        /// <param name="q">endpoint of the segment.</param>
        /// <returns>true when no obstacle is between the two points, otherwise false.</returns>
        public bool HasLineOfSight(Vector2 p, Vector2 q)
        {
            int doesIntersect;
            int collisionCounter = 0;

            if (p == q)
                return true;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].isObstacle)
                {
                    for (int j = 0; j < nodes[i].corners.Count - 1; j++)
                    {
                        doesIntersect = Intersect(p, q, nodes[i].corners[j], nodes[i].corners[j + 1]);
                        if (doesIntersect == 1)
                            return false;
                        else if (doesIntersect == -1)
                            collisionCounter++;
                    }

                    doesIntersect = Intersect(p, q, nodes[i].corners[0], nodes[i].corners[nodes[i].corners.Count - 1]);
                    if (doesIntersect == 1)
                        return false;
                    else if (doesIntersect == -1)
                        collisionCounter++;
                }
            }

            if (collisionCounter > 2)
                return false;

            return true;
        }

        /// <summary>
        /// Function to evaluate polygons for the pathfinding algorithm <see cref="IPathfinderService"/>.
        /// It uses the euclidian distance of the polygon centers.
        /// </summary>
        /// <param name="firstNodeID">Id of the first node.</param>
        /// <param name="secondNodeID">Id of the second node.</param>
        /// <returns>A value that represents the distance between the two polygons.</returns>
        public double Heuristic(int firstNodeID, int secondNodeID)
        {
            return EuclideanDistance(nodes[firstNodeID].center, nodes[secondNodeID].center);
        }

        /// <summary>
        /// /// Function to evaluate polygons for the pathfinding algorithm <see cref="IPathfinderService"/>.
        /// It uses the euclidian distance of the polygon center and the specified point.
        /// </summary>
        /// <param name="nodeID">Id of the node.</param>
        /// <param name="p">point to test for.</param>
        /// <returns>A value that represents the distance between the node and the point.</returns>
        public double Heuristic(int nodeID, Vector2 p)
        {
            return EuclideanDistance(nodes[nodeID].center, p);
        }

        /// <summary>
        /// Calculates the euclidian distance between two points.
        /// </summary>
        /// <param name="p1">first point.</param>
        /// <param name="p2">second point.</param>
        /// <returns>Distance between the two points.</returns>
        public double EuclideanDistance(Vector2 p1, Vector2 p2)
        {
            return Vector2.Distance(p1, p2);
        }

        /// <summary>
        /// Test if a point is in a specified node.
        /// </summary>
        /// <param name="nodeID">If of the node to test.</param>
        /// <param name="point">point to test.</param>
        /// <returns>true if the point is inside the specified node, otherwise false.</returns>
        public bool IsPointInNode(int nodeID, Vector2 point)
        {
            int collisions = 0;

            if (nodes[nodeID].isObstacle)
            {
                return false;
            }

            for (int i = 0; i < nodes[nodeID].corners.Count - 1; i++)
            {
                if (Intersect(point, new Vector2(point.X + 30000, point.Y), nodes[nodeID].corners[i], nodes[nodeID].corners[i + 1]) == 1)
                {
                    collisions++;
                }
            }

            if (Intersect(point, new Vector2(point.X + 30000, point.Y), nodes[nodeID].corners[0], nodes[nodeID].corners[nodes[nodeID].corners.Count - 1]) == 1)
            {
                collisions++;
            }

            if (collisions % 2 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the node to wich the specified point has the smallest distance.
        /// </summary>
        /// <param name="point">point to get the nearest node for.</param>
        /// <returns>If of the nearest node.</returns>
        public int GetNearestNode(Vector2 point)
        {
            int bestNode = 0;
            int i = 0;
            double temp, temp2 = 10000;

            foreach (Node node in nodes)
            {
                if (node.isObstacle)
                    continue;

                temp = EuclideanDistance(point, node.center);

                if (temp < temp2)
                {
                    bestNode = i;
                    temp2 = temp;
                }

                i++;
            }

            return bestNode;    
        }

        /// <summary>
        /// Gets the id of the node in wich the specified point is.
        /// </summary>
        /// <param name="point">point to get the corresponding node for.</param>
        /// <returns>
        /// -1 if the point is not inside of a polygon,
        /// otherwise the id of the node in wich the specified point is.
        /// </returns>
        public int GetNodeWithPoint(Vector2 point)
        {
            var nodeWithPoint = nodes.FirstOrDefault(node => IsPointInNode(node.nodeID, point));

            if (nodeWithPoint == null)
            {
                return -1;
            }

            return nodeWithPoint.nodeID;
        }
    }
}
