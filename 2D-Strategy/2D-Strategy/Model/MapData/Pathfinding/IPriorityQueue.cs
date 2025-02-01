namespace StrategyGame.Model.MapData.Pathfinding
{
    public interface IPriorityQueue
    {
        /// <summary>
        /// inserts an element into the priorityqueue and moves it to the correct position
        /// </summary>
        /// <param name="id">id or value of the element</param>
        /// <param name="key">key of the element to  determin its position</param>
        void Add(int id, double key);

        /// <summary>
        /// return the smalles element without removing it
        /// </summary>
        /// <returns>id or value of the smalles element</returns>
        int Peek();

        /// <summary>
        /// return the smalles element and removes it from the queue
        /// </summary>
        /// <returns>id or value of the smalles element</returns>
        int Poll();

        /// <summary>
        /// Get amount of elements in the queue
        /// </summary>
        /// <returns>Amount of element in the queue</returns>
        int Size();

        /// <summary>
        /// checks if an element with the given id or value exists in the queue
        /// </summary>
        /// <param name="id">id or value of the element</param>
        /// <returns>true when an element with the id or value has been found</returns>
        bool ContainsID(int id);

        /// <summary>
        /// checks if an element with the given key exists in the queue
        /// </summary>
        /// <param name="key">key of the element</param>
        /// <returns>true when an element with the key has been found</returns>
        bool ContainsKey(double key);
    }
}
