using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// Game Event Architecture using Scriptable Objects of generic type, to pass Custom Data Types as Parameter
/// Allows Event without dependancy injection & reference between the scripts Calling and Listening
///
/// Implementation workflow of a new data type as parameter
///     1. Create a new script for:     
///         a) EventListener "NewType"EventListener
///         b) GameEvent "NewType"GameEvent
///         c) UnityEvent "NewType"UnityEvent
///               (follow int example)
///
///             //if not creating a new data type, skip step 1.
///             
///     2. Add Game Event
///         a) Create in Project a new "NewType"GameEventSO, named OnEventFired
///         b) Implement the GameEventSO in the script which starts the event callback for the GameEventListeners
///               [Header("Events")]
///               [SerializeField] private "DataType"GameEvent OnEventFired;
///
///               OnEventFired.RaiseEvent(DataType);
///         
///         c) Add GameEvent Reference in the inspector on the scene object with the script above that starts the callback and the GameEvent was added
///     3. Add Game Event Listeners
///         a) Add "NewType"GameEventListener component to the scripts which want to subscribe to the callback of the GameEvent
///         b) Set up corresponding event response
///      
/// </summary>
/// 
/// Keep separated into different scripts for each class and manage order in Folder structure.
///

public class GameEventListener_Void : BaseGameEventListener<Void, GameEvent_Void, UnityEvent_Void> { }

public class GameEventListener_Int : BaseGameEventListener<int, GameEvent_Int, UnityEvent_Int> { }
