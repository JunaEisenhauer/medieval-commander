using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.IService.Pathfinding;

namespace StrategyGame.Service.Pathfinding
{
    /// <summary>
    /// Interface that the Pathfinding algorithm relies on to get the data of the map.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Function to evaluate polygons for the pathfinding algorithm <see cref="IPathfinderService"/>.
        /// It uses the euclidian distance of the polygon centers.
        /// </summary>
        /// <param name="firstNodeID">Id of the first node.</param>
        /// <param name="secondNodeID">Id of the second node.</param>
        /// <returns>A value that represents the distance between the two polygons.</returns>
        double Heuristic(int firstNodeID, int secondNodeID);

        /// <summary>
        /// /// Function to evaluate polygons for the pathfinding algorithm <see cref="IPathfinderService"/>.
        /// It uses the euclidian distance of the polygon center and the specified point.
        /// </summary>
        /// <param name="nodeID">Id of the node.</param>
        /// <param name="p">point to test for.</param>
        /// <returns>A value that represents the distance between the node and the point.</returns>
        double Heuristic(int nodeID, Vector2 p);

        /// <summary>
        /// Gets the ids of the polygons that neighbor the specified polygon.
        /// </summary>
        /// <param name="nodeID">Id of the node.</param>
        /// <returns>List of nodeIds of the neighboring polygons.</returns>
        List<int> GetNeighbors(int nodeID);

        /// <summary>
        /// Gets the cost of the edge between two polygons.
        /// Currently not used.
        /// </summary>
        /// <param name="firstNodeID">Id of the first node.</param>
        /// <param name="secondNodeID">Id of the second node.</param>
        /// <returns>Cost of the edge between the two polygons.</returns>
        double GetEdgeCost(int firstNodeID, int secondNodeID);

        /// <summary>
        /// Gets the points that connect two polygons / nodes.
        /// </summary>
        /// <param name="firstNodeID">Id of the first node.</param>
        /// <param name="secondNodeID">Id of the second node.</param>
        /// <returns>List of points that connect the two polygons.</returns>
        List<Vector2> GetConnectionPoints(int firstNodeID, int secondNodeID);

        /// <summary>
        /// Function to check whether a obstacle is between the points p and q or not.
        /// </summary>
        /// <param name="p">startpoint of the segment.</param>
        /// <param name="q">endpoint of the segment.</param>
        /// <returns>true when no obstacle is between the two points, otherwise false.</returns>
        bool HasLineOfSight(Vector2 p, Vector2 q);

        /// <summary>
        /// Gets the node to wich the specified point has the smallest distance.
        /// </summary>
        /// <param name="point">point to get the nearest node for.</param>
        /// <returns>If of the nearest node.</returns>
        int GetNearestNode(Vector2 point);

        /// <summary>
        /// Gets the id of the node in wich the specified point is.
        /// </summary>
        /// <param name="point">point to get the corresponding node for.</param>
        /// <returns>
        /// -1 if the point is not inside of a polygon,
        /// otherwise the id of the node in wich the specified point is.
        /// </returns>
        int GetNodeWithPoint(Vector2 point);

        /// <summary>
        /// Adds a polygon / node to the NavMesh.
        /// </summary>
        /// <param name="corners">List of points that are the corners of the polygon.</param>
        /// <param name="isObstacle">true of the polygon should be treaded as an obstacle, otherwise false.</param>
        void AddArea(List<Vector2> corners, bool isObstacle);
    }
}
