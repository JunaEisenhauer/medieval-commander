using System.Collections.Generic;
using OpenTK;
using StrategyGame.Service.Pathfinding;

namespace StrategyGame.Model.IService.Pathfinding
{
    public interface IPathfinderService : IService
    {
        /// <summary>
        /// Adds an area / polygon to the mapdata for the pathfinding.
        /// </summary>
        /// <param name="corner">List of points that are the corners of the polygon.</param>
        /// <param name="isObstacle">true of the polygon should be treaded as an obstacle, otherwise false.</param>
        void AddArea(List<Vector2> corner, bool isObstacle);

        /// <summary>
        /// deletes the current map data to allow for the creation of a new map.
        /// </summary>
        void ClearAreas();

        /// <summary>
        /// Calculates the Path between the startpoint and the goal.
        /// </summary>
        /// <param name="start">point where the path starts.</param>
        /// <param name="goal">point where the path ends.</param>
        /// <returns>List of points to follow to get to the goal.</returns>
        List<Vector2> GetPath(Vector2 start, Vector2 goal);

        /// <summary>
        /// Tests if the specified point is inside of the map or not.
        /// </summary>
        /// <param name="p">point to test.</param>
        /// <returns>true if the point is inside the map, false otherwise.</returns>
        bool IsInNavMesh(Vector2 p);
    }
}
