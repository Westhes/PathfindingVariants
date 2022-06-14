using System;
using System.Collections;
using System.Collections.Generic;

public class ArrayIterator<T> : ICollection, IList, IEnumerable<T>
{
    private T[] innerCollection;

    private int iterator = 0;

    public int Count => ((ICollection)innerCollection).Count;

    public int Length => innerCollection.Length;

    public bool IsSynchronized => innerCollection.IsSynchronized;

    public object SyncRoot => innerCollection.SyncRoot;

    public bool IsFixedSize => innerCollection.IsFixedSize;

    public bool IsReadOnly => innerCollection.IsReadOnly;

    object IList.this[int index] { get => ((IList)innerCollection)[index]; set => ((IList)innerCollection)[index] = value; }

    public ArrayIterator(int size)
    {
        innerCollection = new T[size];
    }

    public ArrayIterator(T[] collection)
    {
        innerCollection = collection;
    }

    public T this[int index] { get => innerCollection[index]; set => innerCollection[index] = value; }

    /// <summary> Tries to find the next item in the list that matches the predicate criteria. Updates iterator. </summary>
    /// <returns> Boolean indicating whether iterator fetch was within bounds. (Note: if an item in the array is null, it will still return true) </returns>
    public bool TryFindNext(out T item, Predicate<T> predicate)
    {
        item = default;
        for (int index = iterator; index < innerCollection.Length; index++)
        {
            var obj = innerCollection[index];
            if (predicate(obj))
            {
                iterator = index + 1;
                item = obj;
                return true;
            }
        }
        return false;
    }

    /// <summary> Tries to find the next item in the list that matches the predicate criteria. Updates iterator. </summary>
    /// <returns> Boolean indicating whether iterator fetch was within bounds. (Note: if an item in the array is null, it will still return true) </returns>
    public bool TryFindRefNext(ref T item, Predicate<T> predicate)
    {
        for (int index = iterator; index < innerCollection.Length; index++)
        {
            var obj = innerCollection[index];
            if (predicate(obj))
            {
                iterator = index + 1;
                item = obj;
                return true;
            }
        }
        return false;
    }

    /// <summary> Returns the first  </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public T FindNext(Predicate<T> predicate)
    {
        for (int index = iterator; index < innerCollection.Length; index++)
        {
            if (predicate(innerCollection[index]))
            {
                iterator = index + 1;
                return innerCollection[index];
            }
        }
        return default;
    }

    /// <summary> Gets the next item from the list and returns it. </summary>
    /// <returns> Boolean indicating whether iterator fetch was within bounds. (Note: if an item in the array is null, it will still return true) </returns>
    public bool TryGetNext(out T item)
    {
        item = default;
        if (iterator < 0 || iterator >= innerCollection.Length)
            return false;
        item = innerCollection[iterator++];
        return true;
    }

    /// <summary> Resets the iterator back to 0. </summary>
    public void ResetIterator() => iterator = 0;

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)innerCollection).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return innerCollection.GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        innerCollection.CopyTo(array, index);
    }

    public int Add(object value)
    {
        return ((IList)innerCollection).Add(value);
    }

    public void Clear()
    {
        ((IList)innerCollection).Clear();
    }

    public bool Contains(object value)
    {
        return ((IList)innerCollection).Contains(value);
    }

    public int IndexOf(object value)
    {
        return ((IList)innerCollection).IndexOf(value);
    }

    public void Insert(int index, object value)
    {
        ((IList)innerCollection).Insert(index, value);
    }

    public void Remove(object value)
    {
        ((IList)innerCollection).Remove(value);
    }

    public void RemoveAt(int index)
    {
        ((IList)innerCollection).RemoveAt(index);
    }
}