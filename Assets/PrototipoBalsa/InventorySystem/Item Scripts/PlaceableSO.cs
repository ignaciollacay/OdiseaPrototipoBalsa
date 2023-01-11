
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Item", menuName = "Inventory/Items/Placeable")]
public class PlaceableSO : ItemSO
{
    //public GameObject equippableWorldItem;
    [Header("Placeable Properties")]
    public Transform pfPlacedObject;
    public Transform visual;
    public int width;
    public int height; //(length)

    public static ItemDirection GetDir(InputDirection inputDirection) //convert Input Direction to PlaceableSO Direction, to rotate Item on rotation input
    {
        switch (inputDirection)
        {
            default:
            case InputDirection.Left: return ItemDirection.Left;
            case InputDirection.Right: return ItemDirection.Right;
            case InputDirection.Down: return ItemDirection.Down;
            case InputDirection.Up: return ItemDirection.Up;
        }
    }

    public enum ItemDirection
    {
        Down,
        Left,
        Up,
        Right,
    }
    public int GetRotationAngle(ItemDirection dir)
    {
        switch (dir)
        {
            default:
            case ItemDirection.Left: return 0;
            case ItemDirection.Up: return 90;
            case ItemDirection.Right: return 180;
            case ItemDirection.Down: return 270;
        }
    }

    public Vector2Int GetRotationOffset(ItemDirection dir)
    {
        switch (dir)
        {
            default:
            case ItemDirection.Left: return new Vector2Int(0, 0);
            case ItemDirection.Up: return new Vector2Int(0, width);
            case ItemDirection.Right: return new Vector2Int(width, height);
            case ItemDirection.Down: return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, ItemDirection dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case ItemDirection.Left:
            case ItemDirection.Right:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case ItemDirection.Up:
            case ItemDirection.Down:
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

    public void Awake()
    {
        type = ItemType.Placeable;
    }
}
