using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    private GridXZ<HeatMapGridObject> gridXZ;

    private void Start()
    {
        gridXZ = new GridXZ<HeatMapGridObject>(0, 4, 2, 10f, new Vector3(0, 0), (GridXZ<HeatMapGridObject> g, int x, int z) => new HeatMapGridObject(g, x, z));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObject = gridXZ.GetGridObject(position);
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(gridXZ.GetGridObject(UtilsClass.GetMouseWorldPosition()));
        }
    }
}

public class HeatMapGridObject
{
    private GridXZ<HeatMapGridObject> gridXZ;
    private int x;
    private int z;
    private int value;

    public const int MIN = 0;
    public const int MAX = 100;

    public HeatMapGridObject(GridXZ<HeatMapGridObject> gridXZ, int x, int z)
    {
        this.gridXZ = gridXZ;
        this.x = x;
        this.z = z;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        Mathf.Clamp(value, MIN, MAX);
        gridXZ.TriggerGridObjectChanged(x, z); //triggerea el evento creo. Pero no se updatea la visual
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString(); 
    }
}
