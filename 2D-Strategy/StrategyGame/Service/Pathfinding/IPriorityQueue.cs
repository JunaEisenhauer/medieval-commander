using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGame.Service.Pathfinding
{
    /// <summary>
    /// Priority Queue wich sorts the elements in ascending order.
    /// </summary>
    /// <typeparam name="T">Data type after which the elements should be sorted.</typeparam>
    public interface IPriorityQueue<T>
    {
        /// <summary>
        /// Checks if the queue is currently empty or not.
        /// </summary>
        /// <returns>true if the queue does not contain any elements, false if it does contain elements.</returns>
        bool IsEmpty();

        /// <summary>
        /// Gets the amount of elements in the queue.
        /// </summary>
        /// <returns>Amount of element in the queue.</returns>
        int Count();

        /// <summary>
        /// Adds a value into the priority queue and puts it in the correct position to maintain the min heap.
        /// </summary>
        /// <param name="key">value after which the element is positioned in the min heap.</param>
        /// <param name="data">data that is suppossed to be added to the queue.</param>
        void Add(double key, T data);

        /// <summary>
        /// Gets the first / smallest element in the priority queue without removing it.
        /// </summary>
        /// <returns>first / smallest element in the queue.</returns>
        T Peek();

        /// <summary>
        /// Gets the first / smallest element in the priority queue and removes it.
        /// After the element has been removed the conditions for a min heap is restored.
        /// </summary>
        /// <returns>first / smallest element in the queue.</returns>
        T Pop();
    }
}
