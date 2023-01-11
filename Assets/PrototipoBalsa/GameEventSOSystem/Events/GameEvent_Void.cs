using UnityEngine;

/// <summary>
/// Void Game Event, for not passing parameter types
/// </summary>
///
/// Keep separated into different scripts for each class and manage order in Folder structure.
/// Int Game Event Provided as example for new data type implementations


[CreateAssetMenu (fileName = "New Void Event", menuName = "Game Event/Void Event")]
public class GameEvent_Void : BaseGameEvent<Void>
{
    public void RaiseEvent() => RaiseEvent(new Void());
}

[CreateAssetMenu(fileName = "New Int Event", menuName = "Game Event/Int Event")]
public class GameEvent_Int : BaseGameEvent<int> { }
