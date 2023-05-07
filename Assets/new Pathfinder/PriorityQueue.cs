using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<T> data;
    private Dictionary<T, int> priorities;

    public PriorityQueue()
    {
        data = new List<T>();
        priorities = new Dictionary<T, int>();
    }

    public void Enqueue(T item, int priority)
    {
        priorities[item] = priority;
        data.Add(item);
        data.Sort((x, y) => priorities[x].CompareTo(priorities[y]));
    }

    public T Dequeue()
    {
        T item = data[0];
        data.RemoveAt(0);
        priorities.Remove(item);
        return item;
    }

    public int Count
    {
        get { return data.Count; }
    }

    public bool Contains(T item)
    {
        return priorities.ContainsKey(item);
    }

    public void ChangePriority(T item, int newPriority)
    {
        priorities[item] = newPriority;
        data.Sort((x, y) => priorities[x].CompareTo(priorities[y]));
    }
}
