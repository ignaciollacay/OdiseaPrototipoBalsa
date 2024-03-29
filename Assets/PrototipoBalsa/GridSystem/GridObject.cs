using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
