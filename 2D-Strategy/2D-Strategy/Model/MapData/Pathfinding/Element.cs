using System;

namespace StrategyGame.Model.MapData.Pathfinding
{
    /// <summary>
    /// Class to save and compare a data type based on a key
    /// </summary>
    /// <typeparam name="T">Data type to save</typeparam>
    public class Element<T> : IEquatable<Element<T>>
    {
        /// <summary>
        /// Gets or sets the Key of the element
        /// </summary>
        public double Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the element in the specified data type
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Element{T}"/> class.
        /// </summary>
        /// <param name="key">Key of the Element</param>
        /// <param name="value">Value to be saved with the Key</param>
        public Element(double key, T value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// larger or equal operator for the class
        /// </summary>
        /// <param name="e1">First object to compare</param>
        /// <param name="e2">Second object to compare</param>
        /// <returns>true if the key of e1 is larger or equal</returns>
        public static bool operator >=(Element<T> e1, Element<T> e2) 
        {
            return e1.Key >= e2.Key ? true : false;
        }

        /// <summary>
        /// smaller or equal operator for the class
        /// </summary>
        /// <param name="e1">First object to compare</param>
        /// <param name="e2">Second object to compare</param>
        /// <returns>true if the key of e1 is smaller or equal</returns>
        public static bool operator <=(Element<T> e1, Element<T> e2)
        {
            return e1.Key <= e2.Key ? true : false;
        }

        /// <summary>
        /// larger operator for the class
        /// </summary>
        /// <param name="e1">First object to compare</param>
        /// <param name="e2">Second object to compare</param>
        /// <returns>true if the key of e1 is larger</returns>
        public static bool operator >(Element<T> e1, Element<T> e2)
        {
            return e1.Key > e2.Key ? true : false;
        }

        /// <summary>
        /// smaller operator for the class
        /// </summary>
        /// <param name="e1">First object to compare</param>
        /// <param name="e2">Second object to compare</param>
        /// <returns>true if the key of e1 is smaller</returns>
        public static bool operator <(Element<T> e1, Element<T> e2)
        {
            return e1.Key < e2.Key ? true : false;
        }

        /// <summary>
        /// equals operator for the class
        /// </summary>
        /// <param name="e1">First object to compare</param>
        /// <param name="e2">Second object to compare</param>
        /// <returns>true if the key of e1 is equal to e2</returns>
        public static bool operator ==(Element<T> e1, Element<T> e2)
        {
            return e1.Key == e2.Key ? true : false;
        }

        /// <summary>
        /// not equal operator for the class
        /// </summary>
        /// <param name="e1">First object to compare</param>
        /// <param name="e2">Second object to compare</param>
        /// <returns>true if the key of e1 is not equal to e2</returns>
        public static bool operator !=(Element<T> e1, Element<T> e2)
        {
            return e1.Key != e2.Key ? true : false;
        }

        /// <summary>
        /// Compares instance of Element to an obj
        /// </summary>
        /// <param name="obj">obj the element should be compared to</param>
        /// <returns>true if the object is not null and has the same key</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Element<T>);
        }

        /// <summary>
        /// Compares two instances of the class Element
        /// </summary>
        /// <param name="other">instance of Element to compare to</param>
        /// <returns>true if the Element is not null and has the same key</returns>
        public bool Equals(Element<T> other)
        {
            return other != null &&
                   Key == other.Key;
        }

        /// <summary>
        /// generates a hashcode for an instance of Element
        /// </summary>
        /// <returns>hashcode as integer</returns>
        public override int GetHashCode()
        {
            var hashCode = 206514262;
            hashCode = (hashCode * -1521134295) + Key.GetHashCode();
            hashCode = (hashCode * -1521134295) + Value.GetHashCode();
            return hashCode;
        }
    }
}
