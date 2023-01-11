using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }  //used in building ghost

    [Header("Grid Common Settings")]
    [SerializeField] private float cellSize = 0.4f;
    //[SerializeField] private GameObject[] gridZone = new GameObject[3];
    private GridXZ<GridObject>[] grids = new GridXZ<GridObject>[8];

    [Header("Grid Horizontal")]
    [SerializeField] private int gridH_Width = 10;
    [SerializeField] private int gridH_Height = 1;
    [SerializeField] private Vector3 gridH1_originPosition = Vector3.zero;
    [SerializeField] private Vector3 gridH2_originPosition = new Vector3(0, 0, 0.4f);
    [SerializeField] private Vector3 gridH3_originPosition = new Vector3(0, 0, 0.8f);
    [SerializeField] private Vector3 gridH4_originPosition = new Vector3(0, 0, 1.2f);
    [SerializeField] private Vector3 gridH5_originPosition = new Vector3(0, 0, 1.6f);
    private GridXZ<GridObject> gridH1;
    private GridXZ<GridObject> gridH2;
    private GridXZ<GridObject> gridH3;
    private GridXZ<GridObject> gridH4;
    private GridXZ<GridObject> gridH5;

    [Header("Grid Perpendicular")]
    [SerializeField] private int gridP_Width = 6;
    [SerializeField] private int gridP_Height = 1;
    [SerializeField] private Vector3 gridP1_originPosition = new Vector3(0.4f, 0, 3);
    [SerializeField] private Vector3 gridP2_originPosition = new Vector3(3.2f, 0, 3);
    private GridXZ<GridObject> gridP1;
    private GridXZ<GridObject> gridP2;

    [Header("Grid Vertical")]
    [SerializeField] private int gridV_Width = 8;
    [SerializeField] private int gridV_Height = 1;
    [SerializeField] private Vector3 gridV_originPosition = new Vector3(1.8f, 0, 3);
    private GridXZ<GridObject> gridV;


    [Header("Log Grid Perpendicular")]
    //[SerializeField] private int logGridP_Width = 6;
    //[SerializeField] private int logGridP_Height = 1;
    [SerializeField] private Vector3 logGridP1_originPosition = new Vector3(0.4f, 0, -0.2f);
    [SerializeField] private Vector3 logGridP2_originPosition = new Vector3(3.2f, 0, -0.2f);
    private GridXZ<GridObject> logGridP1;
    private GridXZ<GridObject> logGridP2;

    [Header("Log Grid Vertical")]
    [SerializeField] private int logGridV_Width = 1;
    //[SerializeField] private int logGridV_Height = 1;
    [SerializeField] private Vector3 logGridV_originPosition = new Vector3(2.6f, 0, 0.8f);
    private GridXZ<GridObject> logGridV;

    [Header("Placement Logs")]
    [SerializeField] private Transform raftPlacementLogs; //Parent GameObject to hold the instantiated raftPlacementLogs (Horizontal) on woodBuilt(), to allow their Destroy on RaftBuilt();
    [SerializeField] private PlacementLog[] woodLogs;
    [SerializeField] private PlacementLog[] raftLogs;
    private PlacementLog[] logs;

    [Header("Raft")]
    [SerializeField] GameObject raftPrefab;

    [Header("Event")]
    [SerializeField] private GameEvent_Item OnItemPlaced;
    [SerializeField] private GameEvent_Item OnItemRemoved;
    public event EventHandler OnSelectedChanged; //used in building ghost. TODO Reemplazar con PlacedObject.GameEvent_OnPlaceableEquipped
    //Events for log
    public delegate void OnPlacedObject(GameObject placedObject);
    public event OnPlacedObject OnObjectPlaced;
    public delegate void OnRemovedObject(GameObject placedObject);
    public event OnRemovedObject OnObjectRemoved;

    [Header("Rotation")]
    public PlaceableSO.ItemDirection dir = PlaceableSO.ItemDirection.Left;


    private GridXZ<GridObject> gridActive;
    private int activeIndex;

    private PlaceableSO placeableSOType;
    private RayCast raycast;


    private void Awake()
    {
        //singleton instance -- Used in Ghost (& TriggerGridZone)
        Instance = this; //used in building ghost

        //create grids
        CreateWoodGrids();
        SetWoodLogs();

        //set starting placeableSOType
        placeableSOType = null;
    }

    public void SetActiveGrid(int triggeredGridZone)
    {
        gridActive = grids[triggeredGridZone];
        activeIndex = triggeredGridZone;
    }

    private void Start()
    {
        raycast = PlayerManager.Instance.raycast;
        //grid = grids[1];
        //Debug.Log("Active grid object origin pos is " + gridActive.originPosition);
        //Debug.Log("Active grid object grid array is " + gridActive.height + ", " + gridActive.width);
    }

    public void PlaceObject()
    {
        //Debug.Log("Active grid object origin pos is " + gridActive.originPosition);
        //Debug.Log("Active grid object grid array is " + gridActive.height + ", " + gridActive.width);
        //get object placement grid positions
        gridActive.GetXZ(raycast.mouseWorldPosition, out int x, out int z);


        List<Vector2Int> gridPositionList = placeableSOType.GetGridPositionList(new Vector2Int(x, z), dir);

        //check if there is an object in grid object positions
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {

            //Debug.Log("ActiveGrid" + gridActive);
            //Debug.Log("GetGridObject " + gridActive.GetGridObject(gridPosition.x, gridPosition.y));
            //Debug.Log("Can Build? " + gridActive.GetGridObject(gridPosition.x, gridPosition.y).CanBuild());
            if (!gridActive.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            //handle rotation offset
            Vector2Int rotationOffset = placeableSOType.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = gridActive.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridActive.GetCellSize();

            //create object in grid
            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placeableSOType);

            //cover all grid positions in which the grid object is placed
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                gridActive.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            //Raise event: Item placed --> Remove from inventory (player + equipment)
            //Debug.Log("Item Placed. Calling remove event");

            //remove the placedobject from the inventory
            OnItemPlaced.RaiseEvent(placeableSOType.data);


            logs[activeIndex].AddPlacedObject(placedObject.gameObject);
            //OnObjectPlaced?.Invoke(placedObject.gameObject); //using for log (TriggerGridZone) TODOING
            //Debug.Log("Created placed object is " + placedObject.gameObject.name, placedObject.gameObject.gameObject);
        }
        else
        {
            //Debug.Log("Can't place object, grid position holds another object");
            RemoveObject();
        }
    }
    public void RemoveObject()
    {
        GridObject gridObject = gridActive.GetGridObject(raycast.mouseWorldPosition);
        PlacedObject placedObject = gridObject.GetPlacedObject();
        if (placedObject != null)
        {
            //destroy placed object prefab
            placedObject.DestroySelf();

            //clear all the placed object's grid positions
            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                gridActive.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }

            //Debug.Log("Removed from grid other object of type " + placedObject.placedObjectTypeSO);

            //Add the placed object back to the inventory
            OnItemRemoved?.RaiseEvent(placedObject.placedObjectTypeSO.data);

            //Remove the placed object from the log
            logs[activeIndex].RemovePlacedObject(placedObject.gameObject);
        }
    }
    public void RotateItem(InputDirection inputDirection)
    {
        dir = PlaceableSO.GetDir(inputDirection);
    }

    public void SelectObjectType(GameObject equippedItem) //UpdatePlaceableSOType
    {
        //Debug.Log("Select object type called " + equippedItem);
        if ((equippedItem != null) && (equippedItem.TryGetComponent(out ItemPlaceable itemPlaceable)))
        {
            placeableSOType = itemPlaceable.placeableSOType;
            //Debug.Log("New selected object type is: " + placeableSOType);
            RefreshSelectedObjectType(); //repeated, move below
        }
        else
        {
            //Debug.Log("No object type selected. Deselecting. " + placeableSOType);
            //deselect object type
            placeableSOType = null;
            RefreshSelectedObjectType(); //repeated, move below
        }
        //RefreshSelectedObjectType
    }

    //deselect object type
    public void DeselectObjectType()
    {
        //Debug.Log("Deselecting object type");
        placeableSOType = null;
        RefreshSelectedObjectType();
    }

    //referenced for ghost -- event callback
    private void RefreshSelectedObjectType() //changes ghost visual
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    //referenced for ghost
    public Vector3 GetMouseWorldSnappedPosition() //changes ghost position
    {
        //necesito controlar que esté dentro de los limites de la grilla
        Vector3 mouseWorldPosition = raycast.mouseWorldPosition;
        gridActive.GetXZ(mouseWorldPosition, out int x, out int z);

        if (placeableSOType != null)
        {
            Vector2Int rotationOffset = placeableSOType.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = gridActive.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * gridActive.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mouseWorldPosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placeableSOType != null)
        {
            return Quaternion.Euler(0, placeableSOType.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlaceableSO GetPlaceableSOType()
    {
        return placeableSOType;
    }

    //Check if all logs are completed
    public bool LogsBuiltCheck()
    {
        foreach (PlacementLog log in logs)
        {
            if (!log.LogBuilt())
            {
                Debug.Log("Log not built " + log.name, log.gameObject.gameObject);
                return false;
            }
        }
        return true;
    }
    
    //Replace woods with logs
    public void LogsBuilt()
    {
        if (!LogsBuiltCheck())
        {
            //Debug.Log("All logs are not built yet");
            return;
        }
        else
        {
            Debug.Log("All logs are built");

            for (int i = 0; i < logs.Length; i++)
            {
                foreach (var wood in logs[i].placedObjects)
                {
                    Destroy(wood);
                }
                if (i >= 5)
                {
                    Instantiate(logs[i].placeableSOType.pickableWorldItem, logs[i].transform.position, logs[i].transform.rotation);
                }
                else
                {
                    Instantiate(logs[i].placeableSOType.visual, logs[i].transform.position, logs[i].transform.rotation, raftPlacementLogs);
                    //guardar referenica raftPlacementLog para destruirlos al instancear el raft
                    
                }
                Destroy(logs[i].gameObject);
                logs[i] = null;
            }
            //Create Grids for log placement
            CreateLogGrids();

            //Enable new log placement positions
            foreach (var log in raftLogs)
            {
                log.gameObject.SetActive(true);
            }

            //Set Raft Logs Array (for RaftBuiltCheck and TriggerGridZone Placement)      -- to check if all Logs have been placed in the raft
            SetRaftLogs();
        }
    }

    public void RaftBuilt()
    {
        //Destory logs & Instantiate Raft prefab?
        if (!LogsBuiltCheck())
        {
            //Debug.Log("All logs are not built yet");
            return;
        }
        else
        {
            Debug.Log("All logs are built");

            //replace logs with raft
            for (int i = 0; i < logs.Length; i++)
            {
                //Destroy PlacedObjects (logs)
                foreach (GameObject placedLog in logs[i].placedObjects)
                {
                    Destroy(placedLog);
                }

                //destroy raftPlacementLogs (P & V)
                Destroy(logs[i].gameObject);
                logs[i] = null;
            }

            //Destroy raftPlacementLogs (H)
            Destroy(raftPlacementLogs.gameObject);

            //create raft
            Instantiate(raftPrefab, gridH1_originPosition, Quaternion.identity);
        }
    }

    private void CreateWoodGrids()
    {
        //Horizontal
        gridH1 = new GridXZ<GridObject>(0, gridH_Width, gridH_Height, cellSize, gridH1_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        gridH2 = new GridXZ<GridObject>(1, gridH_Width, gridH_Height, cellSize, gridH2_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        gridH3 = new GridXZ<GridObject>(2, gridH_Width, gridH_Height, cellSize, gridH3_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        gridH4 = new GridXZ<GridObject>(3, gridH_Width, gridH_Height, cellSize, gridH4_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        gridH5 = new GridXZ<GridObject>(4, gridH_Width, gridH_Height, cellSize, gridH5_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        //Perpendicular
        gridP1 = new GridXZ<GridObject>(5, gridP_Height, gridP_Width, cellSize, gridP1_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        gridP2 = new GridXZ<GridObject>(6, gridP_Height, gridP_Width, cellSize, gridP2_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        //Vertical
        gridV = new GridXZ<GridObject>(7, gridV_Height, gridV_Width, cellSize, gridV_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

        //add grids to GridArray for GridZone detection
        grids[0] = gridH1;
        grids[1] = gridH2;
        grids[2] = gridH3;
        grids[3] = gridH4;
        grids[4] = gridH5;
        grids[5] = gridP1;
        grids[6] = gridP2;
        grids[7] = gridV;

        //set starting active grid
        gridActive = gridH1;
    }

    private void CreateLogGrids()
    {
        //create a new list of grids for logs, replacing the one for woods
        grids = new GridXZ<GridObject>[3];

        //Create Grids
        //Perpendicular
        logGridP1 = new GridXZ<GridObject>(0, gridP_Height, gridP_Width, cellSize, logGridP1_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        logGridP2 = new GridXZ<GridObject>(1, gridP_Height, gridP_Width, cellSize, logGridP2_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        //Vertical
        logGridV = new GridXZ<GridObject>(2, gridV_Height, logGridV_Width, cellSize, logGridV_originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

        //add grids to GridArray for GridZone detection
        grids[0] = logGridP1;
        grids[1] = logGridP2;
        grids[2] = logGridV;

        //set starting active grid
        gridActive = logGridP1;
    }

    private void SetWoodLogs()
    {
        logs = new PlacementLog[8];
        logs[0] = woodLogs[0];
        logs[1] = woodLogs[1];
        logs[2] = woodLogs[2];
        logs[3] = woodLogs[3];
        logs[4] = woodLogs[4];
        logs[5] = woodLogs[5];
        logs[6] = woodLogs[6];
        logs[7] = woodLogs[7];

        activeIndex = 0;
    }

    private void SetRaftLogs()
    {
        logs = new PlacementLog[3];
        logs[0] = raftLogs[0];
        logs[1] = raftLogs[1];
        logs[2] = raftLogs[2];

        activeIndex = 0;
    }
}




    
/*
 * backup 
 * before trying multiple grid
 * 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }  //used in building ghost

    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float cellSize = 0.4f;
    [SerializeField] private Vector3 originPosition = Vector3.zero;

    [Header("Event")]
    [SerializeField] private GameEvent_Item OnItemPlaced;
    [SerializeField] private GameEvent_Item OnItemRemoved;
    public event EventHandler OnSelectedChanged; //used in building ghost. TODO Reemplazar con PlacedObject.GameEvent_OnPlaceableEquipped
    public event EventHandler OnObjectPlaced; //used in building ghost? TODO no references. Delete?

    public PlaceableSO.ItemDirection dir = PlaceableSO.ItemDirection.Down;

    private GridXZ<GridObject> grid;
    private PlaceableSO placeableSOType; //ITEM VARIABLE
    private RayCast raycast;

    private void Awake()
    {
        Instance = this; //used in building ghost

        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, originPosition, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

        placeableSOType = null;


    }
    private void Start()
    {
        raycast = PlayerManager.Instance.raycast;
    }
    public void PlaceObject()
    {
        //get object placement grid positions
        grid.GetXZ(raycast.mouseWorldPosition, out int x, out int z);
        List<Vector2Int> gridPositionList = placeableSOType.GetGridPositionList(new Vector2Int(x, z), dir);

        //check if there is an object in grid object positions
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            //handle rotation offset
            Vector2Int rotationOffset = placeableSOType.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            //create object in grid
            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placeableSOType);

            //cover all grid positions in which the grid object is placed
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }

            //invoke event Placed Object
            OnObjectPlaced?.Invoke(this, EventArgs.Empty); //TODO No references

            //Raise event: Item placed --> Remove from inventory (player + equipment)
            Debug.Log("Item Placed. Calling remove event");
            OnItemPlaced.RaiseEvent(placeableSOType.data);
        }
        else
        {
            Debug.Log("Can't place object, grid position holds another object");
            RemoveObject();
        }
    }
    public void RemoveObject()
    {
        GridObject gridObject = grid.GetGridObject(raycast.mouseWorldPosition);
        PlacedObject placedObject = gridObject.GetPlacedObject();
        if (placedObject != null)
        {
            //destroy placed object prefab
            placedObject.DestroySelf();

            //clear all the placed object's grid positions
            List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            }

            Debug.Log("Removed from grid other object of type " + placedObject.placedObjectTypeSO);
            OnItemRemoved?.RaiseEvent(placedObject.placedObjectTypeSO.data);
        }
    }
    public void RotateItem(InputDirection inputDirection)
    {
        dir = PlaceableSO.GetDir(inputDirection);
    }

    public void SelectObjectType(GameObject equippedItem) //UpdatePlaceableSOType
    {
        Debug.LogWarning("Select object type called " + equippedItem);
        if ((equippedItem != null) && (equippedItem.TryGetComponent(out ItemPlaceable itemPlaceable)))
        {
            //placeableSOType = equippedItem.GetComponent<ItemPlaceable>().placeableSOType;
            placeableSOType = itemPlaceable.placeableSOType;
            Debug.Log("New selected object type is: " + placeableSOType);
            RefreshSelectedObjectType(); //repeated, move below
        }
        else
        {
            Debug.Log("No object type selected. Deselecting. " + placeableSOType);
            //deselect object type
            placeableSOType = null;
            RefreshSelectedObjectType(); //repeated, move below
        }
        //RefreshSelectedObjectType
    }

    //deselect object type
    public void DeselectObjectType()
    {
        Debug.LogWarning("Deselecting object type");
        placeableSOType = null;
        RefreshSelectedObjectType();
    }

    //referenced for ghost -- event callback
    private void RefreshSelectedObjectType() //changes ghost visual
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    //referenced for ghost
    public Vector3 GetMouseWorldSnappedPosition() //changes ghost position
    {
        //necesito controlar que esté dentro de los limites de la grilla
        Vector3 mouseWorldPosition = raycast.mouseWorldPosition;
        grid.GetXZ(mouseWorldPosition, out int x, out int z);

        if (placeableSOType != null)
        {
            Vector2Int rotationOffset = placeableSOType.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mouseWorldPosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placeableSOType != null)
        {
            return Quaternion.Euler(0, placeableSOType.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public PlaceableSO GetPlaceableSOType()
    {
        return placeableSOType;
    }

    public class GridObject
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, z);
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }
        public override string ToString()
        {
            return x + ", " + z + "\n" + placedObject;
        }
    }
}

*/