using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heap array.
/// Source: Sebastian Lague.
/// </summary>
/// <see href="https://youtu.be/3Dw5d7PlcTM"/>
public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int itemCount;

    public Heap(int maxSize)
    {
        items = new T[maxSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = itemCount;
        items[itemCount] = item;
        SortUp(item);
        itemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        itemCount--;
        items[0] = items[itemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count => itemCount;

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int leftIndex = item.HeapIndex * 2 + 1;
            int rightIndex = item.HeapIndex * 2 + 2;
            int swapIndex;

            if (leftIndex < itemCount)
            {
                swapIndex = leftIndex;

                if (rightIndex < itemCount)
                {
                    if (items[leftIndex].CompareTo(items[rightIndex]) < 0)
                    {
                        swapIndex = rightIndex;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T a, T b)
    {
        items[a.HeapIndex] = b;
        items[b.HeapIndex] = a;

        a.HeapIndex += b.HeapIndex;
        b.HeapIndex = a.HeapIndex - b.HeapIndex;
        a.HeapIndex -= b.HeapIndex;
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}