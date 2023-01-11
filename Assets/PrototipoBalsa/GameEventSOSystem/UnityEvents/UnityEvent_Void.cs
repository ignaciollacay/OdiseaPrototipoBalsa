using UnityEngine.Events;

/// <summary>
/// Void Unity Event, for not passing parameter types
/// </summary>
///
/// Keep separated into different scripts for each class and manage order in Folder structure.
/// Int Game Event Provided as example for new data type implementations


[System.Serializable]
public class UnityEvent_Void : UnityEvent <Void> { }

[System.Serializable]
public class UnityEvent_Int : UnityEvent<int> { }