using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeps track of all the grid positions the placed object occupies
//store all the data related to the building object
public class PlacedObject : MonoBehaviour
{
    //creates an object and stores its data on the component
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlaceableSO.ItemDirection dir, PlaceableSO placedObjectTypeSO)
    {
        //create object in grid
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.pfPlacedObject, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    public PlaceableSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlaceableSO.ItemDirection dir;

    //get the grid positions that the object is occupying
    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    //destroy placed object prefab
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
