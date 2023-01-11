using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages Player collision with items
/// TODO
/// WorldItem functionality in editor only
/// Refactor into item to detect collision with player ?
/// Event OnItemPick / OnItemDrop ?
/// </summary>
public class PlayerInventoryController : MonoBehaviour
{
    public InventorySO inventorySO;
    public InventoryUI inventoryUI;

    //EVENT - TBD
    //public delegate void ItemPicked(ItemSO itemPicked, int amount);
    //public event ItemPicked OnItemPicked;

    private void OnTriggerEnter(Collider other)
    {
        var worldItem = other.GetComponent<WorldItem>(); 
        if (worldItem)
        {
            Item itemSO = new Item(worldItem.itemSO);

            if (inventorySO.AddItem(itemSO, 1))  //if there is an available slot in the inventory, Add item to inventory
            {
                //Debug.Log("Picked up " + worldItem.name); //Item picked up
                Destroy(other.gameObject); //destroy world Item gameobject
            }
        }
    }
    #region Old Trigger Enter before Refactoring. Reference for event implementation?
    // Dejo de referencia por si sirve para implementar eventos
    /*
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<WorldItem>();
        if (item)
        {
            Debug.Log("Picked up " + item.name);
            //TBD - Event: OnItemPicked, Replace AddItem and UpdateInventoryUI
            inventorySO.AddItem(new Item(item.itemSO), 1); //Add item to inventory
            Destroy(other.gameObject); //destroy world Item gameobject
            //inventoryUI.UpdateInventoryUI();

            //OnItemPicked?.Invoke((item.itemSO), 1);
        }
    }
    */
    #endregion
}
