using System.Collections.Generic;

namespace StrategyGame.Model.MapData.Pathfinding
{
    /// <summary>
    /// Implementation of a MinHeap to serve as a PriorityQueue
    /// </summary>
    public class MinHeap : IPriorityQueue
    {
        /// <summary>
        /// List of the Elements in the PriorityQueue and their respective keys
        /// </summary>
        private List<Element<int>> elements;

        /// <summary>
        /// Computes the position of the left child of an element
        /// </summary>
        /// <param name="position">Index of the current Element</param>
        /// <returns>Index of the left child</returns>
        private int Left(int position)
        {
            return (2 * position) + 1;
        }

        /// <summary>
        /// Computes the position of the right child of an element
        /// </summary>
        /// <param name="position">Index of the current Element</param>
        /// <returns>Index of the right child</returns>
        private int Right(int position)
        {
            return (2 * position) + 2;
        }

        /// <summary>
        /// Computes the position of the parent of an element
        /// </summary>
        /// <param name="position">Index of the current Element</param>
        /// <returns>Index of the Parent element</returns>
        private int Parent(int position)
        {
            return position / 2;
        }

        /// <summary>
        /// Enshures that the conditions for minheap are met for the specified position
        /// </summary>
        /// <param name="position">position to check the minheap condition on</param>
        private void MinHeapify(int position)
        {
            int min;

            if (Left(position) < elements.Count && elements[Left(position)] > elements[position])
                min = Left(position);
            else
                min = position;

            if (Right(position) < elements.Count && elements[Right(position)] > elements[position])
                min = Right(position);

            if (min != position)
            {
                Swap(position, min);
                MinHeapify(min);
            }
        }

        /// <summary>
        /// Swaps two Elements in the minheap
        /// </summary>
        /// <param name="p1">First Element to swap</param>
        /// <param name="p2">Second Element to swap</param>
        private void Swap(int p1, int p2)
        {
            Element<int> tmp = elements[p1];
            elements[p1] = elements[p2];
            elements[p2] = tmp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinHeap"/> class.
        /// </summary>
        public MinHeap()
        {
            elements = new List<Element<int>>();
        }

        /// <summary>
        /// Adds an Element to the MinHeap and moves it to the correct position
        /// </summary>
        /// <param name="id">Id or value of the element</param>
        /// <param name="key">Key of the element to sort it</param>
        public void Add(int id, double key)
        {
            elements.Add(new Element<int>(key, id));
            int elementIndex = elements.Count - 1;

            while (elements[elementIndex] < elements[Parent(elementIndex)])
            {
                Swap(elementIndex, Parent(elementIndex));
                elementIndex = Parent(elementIndex);
            }
        }

        /// <summary>
        /// Gets the smalles element of the Queue without removing it
        /// </summary>
        /// <returns>Id or value of the smalles element</returns>
        public int Peek()
        {
            return elements[0].Value;
        }

        /// <summary>
        /// Gets the smalles element of the Queue and removes it
        /// </summary>
        /// <returns>Id or value of the smalles element</returns>
        public int Poll()
        {
            int tmp = elements[0].Value;

            elements[0] = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);

            MinHeapify(0);

            return tmp;
        }

        /// <summary>
        /// Gets the amount of elements in the queue
        /// </summary>
        /// <returns>amount of Elements int the Queue</returns>
        public int Size()
        {
            return elements.Count;
        }

        /// <summary>
        /// checks if an element with the given id or value exists in the queue
        /// </summary>
        /// <param name="id">id or value of the element</param>
        /// <returns>true when an element with the id or value has been found</returns>
        public bool ContainsID(int id)
        {
            bool tmp = false;

            foreach (Element<int> next in elements)
            {
                if (next.Value == id)
                {
                    tmp = true;
                    break;
                }
            }

            return tmp;
        }

        /// <summary>
        /// checks if an element with the given key exists in the queue
        /// </summary>
        /// <param name="key">key of the element</param>
        /// <returns>true when an element with the key has been found</returns>
        public bool ContainsKey(double key)
        {
            bool tmp = false;

            foreach (Element<int> next in elements)
            {
                if (next.Key == key)
                {
                    tmp = true;
                    break;
                }
            }

            return tmp;
        }
    }
}
