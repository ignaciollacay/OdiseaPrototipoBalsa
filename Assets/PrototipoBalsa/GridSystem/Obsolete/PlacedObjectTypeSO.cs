using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Construction Item")]
public class PlacedObjectTypeSO : ScriptableObject
{
    //are directions/rotation needed for prototype? Or is it according to view + grid positions?
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:      return Dir.Left;
            case Dir.Left:      return Dir.Up;
            case Dir.Up:        return Dir.Right;
            case Dir.Right:     return Dir.Down;
        }
    }
    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height; //length

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:      return 0;
            case Dir.Left:      return 90;
            case Dir.Up:        return 180;
            case Dir.Right:     return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:      return new Vector2Int(0, 0);
            case Dir.Left:      return new Vector2Int(0, width);
            case Dir.Up:        return new Vector2Int(width, height);
            case Dir.Right:     return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}


/* ////UPDATE. CODIGO DE ABAJO NO SIRVE. FUERON PRUEBAS QUE REFIEREN A INVENTARIO
////Brackeys Inventory mod from here on 

//public Sprite UIItem;
//public GameObject WorldItem;
////Event for Pickup
//public delegate void ItemPicked(PlacedObjectTypeSO placedObjectTypeSO); //acá hay que sacar el prefab del world item del scriptable object. O lo hace otro script?
//public event ItemPicked OnItemPicked;
////Event for Dropdown
//public delegate void ItemDropped(PlacedObjectTypeSO placedObjectTypeSO); //acá hay que referenciar el icon del UI item del scriptable object. O lo hace otro script?
//public event ItemDropped OnItemDropped;

//private void PickUp()
//{
//    Debug.Log("Picked up " + name);
//    //event callback: 1)Add to inventory + inventory UI 2)Remove from world
//    OnItemPicked?.Invoke(this); //OnItemPicked(this);
//}

//private void DropDown()
//{
//    Debug.Log("Dropped down " + name);
//    //event callback: 1)Add to world 2)Remove from inventory + inventory UI
//    OnItemDropped?.Invoke(this); //OnItemDropped(this);
//}

//Interacción que llama a las funciones de pick y drop.
//Tal vez si hace el interactable una interface?
*/