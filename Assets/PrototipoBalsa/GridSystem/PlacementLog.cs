using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlacementLog : MonoBehaviour
{
    public PlaceableSO placeableSOType;
    public int logGridIndex;

    [SerializeField] private bool woodLog;
    [SerializeField] private bool raftLog;

    public List<GameObject> placedObjects;
    private int logWidth;
    private int currentWidth;

    //Testing variable for debugging
#if UNITY_EDITOR
    [SerializeField] bool startBuilt;
#endif

    private void Awake()
    {
        logWidth = placeableSOType.width;
        //Debug.Log("logWidth is " + logWidth + "    " + this.gameObject.name, this.gameObject.gameObject);
    }

#if UNITY_EDITOR
    private void Start()
        //Helper functions for Editor
    {
        if (startBuilt)
        {
            currentWidth = logWidth;
            Debug.LogWarning("Log built on start " + this.gameObject.name, this.gameObject.gameObject);
        }
        if (woodLog && raftLog)
        {
            Debug.LogError("Both wood and raft logs are set. Only one should be set");
        }
    }
#endif

    public void AddPlacedObject(GameObject _placedObject)
    {
        int woodWidth = _placedObject.GetComponent<PlacedObject>().placedObjectTypeSO.width;

        placedObjects.Add(_placedObject);

        currentWidth += woodWidth;

        Debug.Log("Added wood width " + woodWidth + " to log width " + logWidth + ". Current width = " + currentWidth + ". Remaining width = " + (logWidth - currentWidth));

        if (LogBuilt())
        {
            //Debug.Log("Log built! Checking if other logs are also built");
            if (woodLog)
            {
                GridBuildingSystem.Instance.LogsBuilt();
            }
            if (raftLog)
            {
                GridBuildingSystem.Instance.RaftBuilt();
            }
        }
    }
    //Es bastante parecida al Add. Se podría refactorizar y sintetizar. TODO
    public void RemovePlacedObject(GameObject _placedObject)
    {
        int woodWidth = _placedObject.GetComponent<PlacedObject>().placedObjectTypeSO.width;

        placedObjects.Remove(_placedObject);

        currentWidth -= woodWidth;

        Debug.Log("Removed wood width " + woodWidth + " to log width " + logWidth + ". Current width = " + currentWidth + ". Remaining width = " + (logWidth - currentWidth));
    }

    public bool LogBuilt()
    {
        if (logWidth == currentWidth)
        {
            //Debug.Log("Log Built - " + gameObject.name, gameObject.gameObject);
            return true;
        }
        else
        {
            //Debug.Log("Log not built yet - " + gameObject.name, gameObject.gameObject);
            return false;
        }
    }

    //used for raycast
    public void GetActiveGrid()
    {
        GridBuildingSystem.Instance.SetActiveGrid(logGridIndex);
        //Debug.Log("Player triggered grid zone " + this.gameObject);
    }
}
