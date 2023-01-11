/// <summary>
///  Game Event Listener of Generic type
/// </summary>
/// <typeparam name="T"> Data Type</typeparam>
public interface IGameEventListener<T>
{
    void OnEventRaised(T item);
}