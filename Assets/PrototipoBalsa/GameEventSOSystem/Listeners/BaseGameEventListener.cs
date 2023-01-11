using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Game Event Listener for Scriptable Object Game Event Subscription, using a Generic Type to allow passing different data types as parameters
/// Base Class all Game Event Listeners will inherit from
/// </summary>
/// <typeparam name="T"> Game Event SO </typeparam> 
/// <typeparam name="E">  Game Event Listener </typeparam> 
/// <typeparam name="UER"> Unity Event </typeparam>

public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
{
    [SerializeField] private E gameEvent;
    public E GameEvent { get { return gameEvent; } set { gameEvent = value; } }

    [SerializeField] private UER unityEventResponse;

    private void OnEnable()
    {
        if (gameEvent == null)
        {
            return;
        }
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
        {
            return;
        }
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        if (unityEventResponse != null)
        {
            unityEventResponse.Invoke(item);
        }
    }
}