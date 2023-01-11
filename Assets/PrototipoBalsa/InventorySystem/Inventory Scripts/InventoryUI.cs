using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class InventoryUI : MonoBehaviour
{
    public InventorySO inventorySO;

    public Dictionary<GameObject, InventorySlot> currentItemSlots = new Dictionary<GameObject, InventorySlot>();


    private void Start()
    {
        //link equipment and player inventories
        for (int i = 0; i < inventorySO.GetInventorySlots.Length; i++)
        {
            inventorySO.GetInventorySlots[i].linkedInventory = this; //distinguish which interface the slot belongs to
            inventorySO.GetInventorySlots[i].OnAfterSlotUpdated += OnSlotUpdate;
        }
        CreateSlots();
    }


    public void OnSlotUpdate(InventorySlot _slot)
    {
        //Debug.Log("OnSlotUpdate " + _slot.slotDisplay.gameObject);
        if (_slot.item.Id >= 0) //if it has an item in the slot
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.GetItemSO.uIItem; //set slot ui to item ui

            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0"); //Set Amount to inventory slot . If it is 1 dont display amount, else display the amount

        }
        else
        {
            //It doesnt have an item
            //clear slot to display no items. Dont display anything.
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
            //Debug.Log("Amount of items is 0, clearing slot");

            //agrego cosas para el PlayerInventory
                //remover el equipped world item si ya no tiene más del item
            //inventoryPlayerEquipped.UnequipItem(_slot.GetItemSO);
            //Debug.Log("Unequip the item in player inventory");
        }
    }

    public abstract void CreateSlots();

    public void AddEvent(GameObject itemSlot, EventTriggerType eventType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = itemSlot.GetComponent<EventTrigger>(); //grab event trigger from the itemSlot Button
        var eventTrigger = new EventTrigger.Entry(); //new trigger to add to the event trigger
        eventTrigger.eventID = eventType; //set the trigger event type
        eventTrigger.callback.AddListener(action); //listen for the action
        trigger.triggers.Add(eventTrigger); //add the event trigger
    }
}



/* Obsolete AddItemUI() - Replaced with CreateSlots
    private void AddItemUI()
    {
        for (int i = 0; i < inventorySO.GetSlots.Count; i++)
        {
            InventorySlot slot = inventorySO.GetSlots[i];

            var itemUI = Instantiate(itemUIPrefab, Vector3.zero, Quaternion.identity, transform); //Create inventory slot (generic)
            itemUI.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventorySO.databaseSO.GetItem[slot.item.Id].uIItem; //Set Icon to inventory slot
            itemUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0"); //Set Amount to inventory slot 
            //itemsDisplayed.Add(slot, itemUI);

            inventorySO.GetSlots[i].slotDisplay = itemUI;
        }
    }
    */

/* obsolete UpdateInventoryUI() - Replaced with UpdateSlots
public void UpdateInventoryUI()
{
    for (int i = 0; i < inventorySO.GetSlots.Count; i++)
    {
        InventorySlot slot = inventorySO.GetSlots[i];

        if (itemsDisplayed.ContainsKey(slot)) //find if the item already is in the inventory
        {
            itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        }
        else
        {
            //AddItemUI(); //no lo puedo separar porque me faltan variables. Quizás lo podría settear con variables y parámetros, pero no la tengo muy clara y quizás se resuelve en los prox tuts.
            var itemUI = Instantiate(itemUIPrefab, Vector3.zero, Quaternion.identity, transform); //Create inventory slot (generic)
            itemUI.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventorySO.databaseSO.GetItem[slot.item.Id].uIItem; //Set Icon to inventory slot
            itemUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0"); //Set Amount to inventory slot 
            itemsDisplayed.Add(slot, itemUI);

            inventorySO.GetSlots[i].slotDisplay = itemUI;
        }
    }
}
*/

// AddItemMesh - In Editor implementation in World Item class. TODO?
/* La idea era que se ocupe de crear el 3D World Item cuando el Usuario quita el objeto de su inventario
public void AddItemMesh()
{
    for (int i = 0; i < inventorySO.inventory.Items.Count; i++)
    {
        InventorySlot slot = inventorySO.inventory.Items[i];

        //Create item
        var itemMesh = Instantiate(itemMeshPrefab, Vector3.zero, Quaternion.identity, transform);
        //Set MeshFilter
        itemMesh.transform.GetChild(0).GetComponentInChildren<MeshFilter>().mesh = inventorySO.databaseSO.GetItem[slot.item.Id].worldItem.GetComponent<MeshFilter>().mesh;
        //Set MeshRenderer
        itemMesh.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material = inventorySO.databaseSO.GetItem[slot.item.Id].worldItem.GetComponent<MeshRenderer>().material;
    }
}
*/

/* EquipItem - TODO Not Implemented. Creo que va en otra clase
public void EquipItem(Item _item)
{
    //_inventorySlot = el objeto que fue clickeado. Como sé quien es?

    //saber el itemSO del inventorySlot que fue clickeado
    //pasar al inventario de objetos equipados?
    //activar interaction button del itemSO
}
*/

//// SOLUTION NOT WORKING. SAVING FOR REFERENCE
//public void OnClick(GameObject itemSlotPrefab)
//{
//    if (itemsDisplayed[itemSlotPrefab].item.Id >= 0) //if the clicked slot has an item
//    {
//        Debug.Log("Player Clicked on slot");

//        //Aca hay un problema. Estoy usando el bool del slot, y no me doy cuenta si hay otro item de esa posicion ya equipado
//        //estoy generando una diferencia entre el PlayerEquipment UI y el PlayerEquipment Inventory

//        InventorySlot inventorySlot = inventorySO.GetSlotPosition(itemsDisplayed[itemSlotPrefab].GetItemSO);

//        //if the PlayerInventory doesnt hold an item with the same position as the clicked item
//        if (inventorySO.GetSlotPosition(itemsDisplayed[itemSlotPrefab].GetItemSO) == null) //didnt find an item with that position
//        {
//            //equip the item
//            EquipItem(itemSlotPrefab);
//            Debug.Log("PlayerInventory doesnt hold an item with the same Position as the Clicked Slot. Item will be equipped");
//        }
//        //else if the PlayerInventory holds an item with the same position as the clicked item
//        else
//        {
//            // unequip currently equipped item (the inventory slot found in GetSlotPosition)
//            inventorySlot.isEquipped = false; //Set Inventory slot as not equipped
//            OnItemUnequipped?.Invoke(inventorySlot.GetItemSO, inventorySlot.amount); //Invoke Event
//            ChangeSlotColor(inventorySlot, 255); //update Slot UI to unequipped

//            Debug.Log("PlayerInventory holds an item with the same Position as the Clicked Slot. Item will be unequipped");

//            // but if it wasn't the same item, we still need to add the new item
//            if (inventorySlot.item.Id != itemsDisplayed[itemSlotPrefab].item.Id)
//            {
//                EquipItem(itemSlotPrefab);
//            }

//            /* lo mismo de arriba pero distinto
//            if (inventorySlot.item == itemsDisplayed[itemSlotPrefab].item)
//            {
//                // if it is the same item
//                // unequip currently equipped item item
//                UnequipItem(itemSlotPrefab);
//            }
//            else
//            {
//                // if it is not same item
//                // unequip currently equipped item (the inventory slot found in GetSlotPosition)
//                // equip new item
//                EquipItem(itemSlotPrefab);
//            }
//            */
//        }

//        /*
//         * Esto es lo que hacia antes, generaba quilombo entre el UI y el System
//         * 
//        //if it is not equipped, equip
//        if (!itemsDisplayed[itemSlotPrefab].isEquipped)  //if the SlotUIPrefab is not set as equipped
//        {
//            EquipItem(itemSlotPrefab);
//        }
//        //if it is equipped, unequip
//        else                                             //if the SlotUIPrefab is not set as equipped
//        {
//            UnequipItem(itemSlotPrefab);
//        }

//        */
//    }
//    else //no sé bien que hace esto                   //Item.Id = -1. The clicked slot does not have an item?  
//    {
//        //remove item ?
//        inventorySO.RemoveItem(itemsDisplayed[itemSlotPrefab].item); //remove item from the inventory system
//        Debug.Log("Inventory SO remove item called. Not sure what it does.");
//    }
//}
//private void EquipItem(GameObject itemSlotPrefab)
//{
//    itemsDisplayed[itemSlotPrefab].isEquipped = true; //Set Inventory slot as equipped
//    OnItemEquipped?.Invoke(itemsDisplayed[itemSlotPrefab].GetItemSO, itemsDisplayed[itemSlotPrefab].amount); //Invoke Event
//    ChangeSlotColor(itemSlotPrefab, 0); //update Slot UI to equipped
//}
////no me sirve esta funcion, porque usa un gameobject y yo obtuve un inventory slot
//private void UnequipItem(GameObject itemSlotPrefab)
//{
//    itemsDisplayed[itemSlotPrefab].isEquipped = false; //Set Inventory slot as not equipped
//    OnItemUnequipped?.Invoke(itemsDisplayed[itemSlotPrefab].GetItemSO, itemsDisplayed[itemSlotPrefab].amount); //Invoke Event
//    ChangeSlotColor(itemSlotPrefab, 255); //update Slot UI to unequipped
//}

//private void ChangeSlotColor(GameObject itemSlotPrefab, int blueValue)
//{
//    Color SlotColor = itemSlotPrefab.GetComponent<Image>().color; //get the slot current color
//    Color EquippedColor = SlotColor; //create new color for equipped, same as slot current color
//    EquippedColor.b = blueValue; //set equipped color to yellow
//    itemSlotPrefab.GetComponent<Image>().color = EquippedColor; //set slot color to equipped color
//}

//SOLUTION 1 . NOT WORKING PROPERLY
//public void OnClick(GameObject itemSlotPrefab)
//{
//    if (itemsDisplayed[itemSlotPrefab].item.Id >= 0) //if the clicked slot has an item
//    {
//        Debug.Log("Player Clicked on slot");

//        //Aca hay un problema. Estoy usando el bool del slot, y no me doy cuenta si hay otro item de esa posicion ya equipado
//        //estoy generando una diferencia entre el PlayerEquipment UI y el PlayerEquipment Inventory

//        //if it is not equipped, equip
//        if (!itemsDisplayed[itemSlotPrefab].isEquipped)  //if the SlotUIPrefab is not set as equipped
//        {
//            EquipItem(itemSlotPrefab);
//        }
//        //if it is equipped, unequip
//        else                                             //if the SlotUIPrefab is not set as equipped
//        {
//            UnequipItem(itemSlotPrefab);
//        }
//    }
//    else //no sé bien que hace esto                   //Item.Id = -1. The clicked slot does not have an item?  
//    {
//        //remove item ?
//        inventorySO.RemoveItem(itemsDisplayed[itemSlotPrefab].item); //remove item from the inventory system
//        Debug.Log("Inventory SO remove item called. Not sure what it does.");
//    }
//}
//private void EquipItem(GameObject itemSlotPrefab)
//{
//    itemsDisplayed[itemSlotPrefab].isEquipped = true; //Set Inventory slot as equipped
//    OnItemEquipped?.Invoke(itemsDisplayed[itemSlotPrefab].GetItemSO, itemsDisplayed[itemSlotPrefab].amount); //Invoke Event
//    ChangeSlotColor(itemSlotPrefab, 0); //update Slot UI to equipped
//}
////no me sirve esta funcion, porque usa un gameobject y yo obtuve un inventory slot
//private void UnequipItem(GameObject itemSlotPrefab)
//{
//    itemsDisplayed[itemSlotPrefab].isEquipped = false; //Set Inventory slot as not equipped
//    OnItemUnequipped?.Invoke(itemsDisplayed[itemSlotPrefab].GetItemSO, itemsDisplayed[itemSlotPrefab].amount); //Invoke Event
//    ChangeSlotColor(itemSlotPrefab, 255); //update Slot UI to unequipped
//}
