using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRPG : MonoBehaviour
{
    private List<PlaceableSO> inventoryPlayer = new List<PlaceableSO>();
    [SerializeField] private InventoryMasterSO inventoryMasterSO;

    private void Start()
    {
        foreach (PlaceableSO item in inventoryMasterSO.inventoryMaster)
        {
            //item.OnItemPicked += AddItem;
            //item.OnItemDropped += RemoveItem;

            //TBD Se supone que me tengo que unsubscribe de los eventos. Pero no sé si aplica al caso. Tal vez cuando se destruye, desubscribir de esa misma instancia que fue destruida? 
        }
    }
    private void AddItem(PlaceableSO item)
    {
        inventoryPlayer.Add(item);

        //Add the UI icon in the UI inventory
            //Instantiate(item.UIItem);
        //Destroy the world item in the world
            //Destroy(item.WorldItem);
    }

    private void RemoveItem(PlaceableSO item)
    {
        inventoryPlayer.Remove(item);

        //Remove the UI Icon in the UI inventory
            //Destroy(item.UIItem);
        //Add the World Item (prefab) in the world
            //Instantiate(item.WorldItem);
    }
}
