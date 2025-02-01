using System;
using System.Collections.Generic;

namespace StrategyGame.Service.Pathfinding
{
    /// <summary>
    /// Implementation of a priority queue with a min heap as underlying data structure.
    /// </summary>
    /// <typeparam name="T">Data type to create a priority queue for.</typeparam>
    public class MinHeap<T> : IPriorityQueue<T>
    {
        private class Node
        {
            public T data;
            public double key;
        }

        /// <summary>
        /// List that stores all current values in the priority queue.
        /// </summary>
        private List<Node> nodes;

        // Functions to calculate the indexes of the child and parent elements
        private int GetLeftChildIndex(int nodeIndex) => (2 * nodeIndex) + 1;

        private int GetRightChildIndex(int nodeIndex) => (2 * nodeIndex) + 2;

        private int GetParentIndex(int nodeIndex) => (nodeIndex - 1) / 2;

        // Functions to check whether a node has a child or parent
        private bool HasLeftChild(int nodeIndex) => GetLeftChildIndex(nodeIndex) < nodes.Count;

        private bool HasRightChild(int nodeIndex) => GetRightChildIndex(nodeIndex) < nodes.Count;

        private bool IsRoot(int nodeIndex) => nodeIndex == 0;

        // Functions to get a child or parent element of a node
        private Node GetLeftChild(int nodeIndex) => nodes[GetLeftChildIndex(nodeIndex)];

        private Node GetRightChild(int nodeIndex) => nodes[GetRightChildIndex(nodeIndex)];

        private Node GetParent(int nodeIndex) => nodes[GetParentIndex(nodeIndex)];

        // Function to swap to elements in the array nodes
        private void Swap(int firstIndex, int secondIndex)
        {
            Node temp = nodes[firstIndex];
            nodes[firstIndex] = nodes[secondIndex];
            nodes[secondIndex] = temp;
        }

        /// <summary>
        /// Restores the min heap after a element has been removed.
        /// </summary>
        /// <param name="nodeIndex">index of the removed element.</param>
        private void MinHeapify(int nodeIndex)
        {
            int minIndex;

            if (HasLeftChild(nodeIndex) && GetLeftChild(nodeIndex).key < nodes[nodeIndex].key)
                minIndex = GetLeftChildIndex(nodeIndex);
            else
                minIndex = nodeIndex;

            if (HasRightChild(nodeIndex) && GetRightChild(nodeIndex).key < nodes[minIndex].key)
                minIndex = GetRightChildIndex(nodeIndex);

            if (nodeIndex != minIndex)
            {
                Swap(nodeIndex, minIndex);
                MinHeapify(minIndex);
            }
        }

        /// <summary>
        /// Compares the  element in the specified index with its parent and swaps until the parent is smaller than the element.
        /// </summary>
        /// <param name="nodeIndex">index of the element to integrate into the queue.</param>
        private void RestoreHeap(int nodeIndex)
        {
            while (nodes[nodeIndex].key > GetParent(nodeIndex).key)
            {
                Swap(nodeIndex, GetParentIndex(nodeIndex));
                nodeIndex = GetParentIndex(nodeIndex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinHeap{T}"/> class.
        /// </summary>
        public MinHeap()
        {
            nodes = new List<Node>();
        }

        /// <summary>
        /// Checks if the queue is currently empty or not.
        /// </summary>
        /// <returns>true if the queue does not contain any elements, false if it does contain elements.</returns>
        public bool IsEmpty()
        {
            return nodes.Count == 0 ? true : false;
        }

        /// <summary>
        /// Gets the amount of elements in the queue.
        /// </summary>
        /// <returns>Amount of element in the queue.</returns>
        public int Count()
        {
            return nodes.Count;
        }

        /// <summary>
        /// Adds a value into the priority queue and puts it in the correct position to maintain the min heap.
        /// </summary>
        /// <param name="key">value after which the element is positioned in the min heap.</param>
        /// <param name="data">data that is suppossed to be added to the queue.</param>
        public void Add(double key, T data)
        {
            int index = nodes.Count;

            nodes.Add(new Node());

            nodes[index].key = key;
            nodes[index].data = data;

            RestoreHeap(index);
        }

        /// <summary>
        /// Gets the first / smallest element in the priority queue without removing it.
        /// </summary>
        /// <returns>first / smallest element in the queue.</returns>
        public T Peek()
        {
            if (IsEmpty())
                throw new IndexOutOfRangeException();

            return nodes[0].data;
        }

        /// <summary>
        /// Gets the first / smallest element in the priority queue and removes it.
        /// After the element has been removed the conditions for a min heap is restored.
        /// </summary>
        /// <returns>first / smallest element in the queue.</returns>
        public T Pop()
        {
            T node = nodes[0].data;
            if (IsEmpty())
                throw new IndexOutOfRangeException();

            nodes[0] = nodes[nodes.Count - 1];
            nodes.RemoveAt(nodes.Count - 1);

            MinHeapify(0);

            return node;
        }
    }
}
