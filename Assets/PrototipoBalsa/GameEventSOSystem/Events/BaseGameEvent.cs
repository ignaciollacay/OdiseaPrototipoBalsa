using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game Event Scriptable Object, using a Generic Type to allow passing different data types as parameters
/// Base Class all Game Event SO will inherit from
/// </summary>
/// <typeparam name="T"> Data Type </typeparam>


public abstract class BaseGameEvent<T> : ScriptableObject
{
    private readonly List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();

    public void RaiseEvent(T item)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(item);
            //Debug.Log("Event Raised by " + eventListeners[i]);
        }
    }

    public void RegisterListener(IGameEventListener<T> listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(IGameEventListener<T> listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}