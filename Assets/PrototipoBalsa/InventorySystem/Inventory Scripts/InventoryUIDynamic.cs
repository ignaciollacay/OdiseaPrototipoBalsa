using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void ItemSelected(ItemSO itemSO);

public class InventoryUIDynamic : InventoryUI
{
    public GameObject itemUIPrefab;
    //[SerializeField] private GameEvent_ItemSO OnItemSelect;
    //[SerializeField] private GameEvent_ItemSO OnItemUnselect;


    public ItemSelected OnItemSelect;
    public ItemSelected OnItemUnselect;

    public PlayerInventory playerInventory;
    public UI_InputManager inputManager;

    public override void CreateSlots()
    {
        currentItemSlots = new Dictionary<GameObject, InventorySlot>(); //not necessary. Make sure its a new dictionary

        for (int i = 0; i < inventorySO.GetInventorySlots.Length; i++)
        {
            var slotUI = Instantiate(itemUIPrefab, Vector3.zero, Quaternion.identity, transform); //Create inventory slot (generic)

            AddEvent(slotUI, EventTriggerType.PointerUp, delegate { OnClick(slotUI); });//adds event

            inventorySO.GetInventorySlots[i].slotDisplay = slotUI; //sets the slot ui to the new slot ?

            currentItemSlots.Add(slotUI, inventorySO.GetInventorySlots[i]); //add the new slot to the dictionary
        }
    }

    public void OnClick(GameObject clickedItemSlot)
    {
        if (currentItemSlots[clickedItemSlot].item.Id >= 0) //if the clicked slot has an item
        {
            //Debug.Log("Player Clicked on slot " + clickedItemSlot.gameObject);

            //if it is not equipped, equip
            if (currentItemSlots[clickedItemSlot].slotPosition == ItemPosition.None)  //if the SlotUIPrefab is not set as equipped
            {
                //      UNEQUIP IF THERE IS ANOTHER EQUIPPED SLOT IN THE SAME POSITION AS ITEM TO EQUIP
                List<GameObject> equippedItems; List<InventorySlot> equippedSlots;

                if (ComparePosition(clickedItemSlot, out equippedItems, out equippedSlots)) //busca si hay otro slot equipado en la misma posicion
                {
                    Debug.Log("Unequipping");
                    //OnItemUnselect.Invoke(equippedSlot.GetItemSO);
                    //      UNEQUIP

                    //  INVENTORY UI
                    foreach (var equippedSlot in equippedSlots)
                    {
                        equippedSlot.slotPosition = ItemPosition.None; //Set Inventory slot as not equipped
                    }
                    foreach (var equippedItem in equippedItems)
                    {
                        ChangeSlotColor(equippedItem, 255); //cambiar el color a no equipado
                    }

                    //  PLAYER INVENTORY
                    foreach (var equippedSlot in equippedSlots)
                    {
                        playerInventory.DestroyWorldItem(equippedSlot.GetItemSO);
                    }

                    playerInventory.OnItemEquip.RaiseEvent(null); //o key
                }

                //      EQUIP
                //OnItemSelect.Invoke(currentItemSlots[clickedItemSlot].GetItemSO);

                //  INVENTORY UI
                currentItemSlots[clickedItemSlot].slotPosition = currentItemSlots[clickedItemSlot].item.Position; //Set Inventory slot position to item position (set as equipped)

                if (currentItemSlots[clickedItemSlot].GetItemSO.interactableType != InteractableType.None)
                {
                    ChangeSlotColor(clickedItemSlot, 0); //update Slot UI to equipped
                }

                //  PLAYER INVENTORY
                playerInventory.CreateWorldItem(currentItemSlots[clickedItemSlot].GetItemSO); //TODOING Integrar acá funcionalidad PlayerInventory 
            }
            //if it is equipped, unequip
            else                                             //if the SlotUIPrefab is set as equipped
            {
                //      UNEQUIP
                //OnItemUnselect.Invoke(currentItemSlots[clickedItemSlot].GetItemSO);
                //  INVENTORY UI
                currentItemSlots[clickedItemSlot].slotPosition = ItemPosition.None; //Set Inventory slot as not equipped
                ChangeSlotColor(clickedItemSlot, 255); //update Slot UI to unequipped


                //  PLAYER INVENTORY
                playerInventory.DestroyWorldItem(currentItemSlots[clickedItemSlot].GetItemSO);
                
                //  FIXME Agrego esto acá porque no lo puedo llamar desde el onDestroy del ItemPlacable.
                            //el OnDestroy llega más tarde que el Start.
                            //Probé pasar el equip al proximo frame, pero queda un frame en el que se corre la camara de posicion por la transicion del aimCamera al normalCamera
                            //pero es raro mantenerlo acá porque solo es algo del ItemPlaceable, y lo ejecutaría con los otros interactables.
                PlayerManager.Instance.playerController.SetSensitivity(1);
                PlayerManager.Instance.playerAimCamera.gameObject.SetActive(false);
                PlayerManager.Instance.raycast.enabled = false;
            }
        }
        else //no sé bien que hace esto                   //Item.Id = -1. The clicked slot does not have an item?  
        {
            Debug.LogWarning("Inventory SO remove item called. Not sure what it does.");
            inventorySO.RemoveItem(currentItemSlots[clickedItemSlot].item); //remove item from the inventory system
        }
    }

    private void ChangeSlotColor(GameObject itemSlotPrefab, int blueValue)
    {
        Color SlotColor = itemSlotPrefab.GetComponent<Image>().color; //get the slot current color
        Color EquippedColor = SlotColor; //create new color for equipped, same as slot current color
        EquippedColor.b = blueValue; //set equipped color to yellow
        itemSlotPrefab.GetComponent<Image>().color = EquippedColor; //set slot color to equipped color
    }

    public void Remove(Item _item)
    {
        //get the slot
        InventorySlot _slot = inventorySO.GetItemSlot(_item);
        //change amount
        _slot.amount -= 1;
        //Debug.Log("New Slot amount is " + _slot.amount + _slot.slotDisplay.gameObject);


        if (_slot.amount == 0)
        {
            //Debug.Log("Slot amount is 0 " + _slot.amount + _slot.slotDisplay.gameObject);

            //remove item from player inventory
            ////-- raise unequip event
            //ItemSO itemSO = itemsDisplayed[_slot.slotDisplay].GetItemSO;
            //Debug.Log("Trying to raise On Item Equip Event with ItemSO " + itemSO);
            //OnItemUnequip?.RaiseEvent(itemSO);
            

            //  PLAYER INVENTORY
            Destroy(PlayerInventory.equippedInteractableItem);
            PlayerInventory.equippedInteractableItem = null;
            playerInventory.OnItemEquip.RaiseEvent(PlayerInventory.equippedInteractableItem);


            //  INVENTORY UI
            //reset the slot prefab and inventorySlot
            _slot.item.Name = ""; //clear data 
            _slot.item.Id = -1; //clear data 
            _slot.slotPosition = ItemPosition.None; //clear data 
            ChangeSlotColor(_slot.slotDisplay, 255); //clear color
            OnSlotUpdate(_slot); //clear image and text
            //Debug.Log("Item Removed from inventory ui " + _slot.slotDisplay.gameObject);


            //  INPUT UI
            //OnItemUnselect?.RaiseEvent(itemsDisplayed[clickedItemSlot].GetItemSO);
            inputManager.UpdateIcon(null); //TODO faltaría cambiar Icon a funcionar con el GO en vez de ItemSO
            inputManager.UpdateButton(PlayerInventory.equippedInteractableItem);

            //remove from dictionary
            //itemsDisplayed.Remove(_slot.slotDisplay);
            //Debug.Log("Item Removed from inventory dictionary " + _slot.slotDisplay);
            //remove item from inventory
            //inventorySO.RemoveItem(_slot.item);
            //Debug.Log("Item Removed from inventory SO " + _slot.item);
        }
        else
        {
            //Debug.Log("Slot amount greater than 0 " + _slot.amount + _slot.slotDisplay.gameObject);
            OnSlotUpdate(_slot); //update slot ui to display new amount
        }
    }

    public void Add(Item _item)
    {
        
        InventorySlot _slot = inventorySO.GetItemSlot(_item);     //get the slot

        if (_slot != null)      //if item is in inventory
        {
            
            _slot.amount += 1;      //change amount

            OnSlotUpdate(_slot);    //update slot ui to display new amount
        }

        else if ((_slot == null) && (inventorySO.AddItem(_item, 1)))     //if item not in inventory && can be added in inventory
        {
            Debug.Log("Item not found in inventory. Added new slot for " + _item.Name);     //Item picked up
        }

        else
        {
            Debug.LogError("Item could not be added to inventory. Unexpected case. Is inventory full?");
        }
    }

    //TODO esta funcion debería ir en el InventorySO Script. No tiene que ver con la UI.
    private bool ComparePosition(GameObject clickedItemSlot, out List<GameObject> item, out List<InventorySlot> slot)
    {
        item = new List<GameObject>(); slot = new List<InventorySlot>();

        switch (currentItemSlots[clickedItemSlot].item.Position)
        {
            case ItemPosition.DoubleHanded:
                foreach (KeyValuePair<GameObject, InventorySlot> itemSlot in currentItemSlots)
                {
                    if (itemSlot.Value.slotPosition == ItemPosition.LeftHand || (itemSlot.Value.slotPosition == ItemPosition.RightHand))
                    {
                        item.Add(itemSlot.Key);
                        slot.Add(itemSlot.Value);
                    }
                }
                if (slot.Count != 0)
                    return true;
                else
                    return false;

            case ItemPosition.LeftHand:
            case ItemPosition.RightHand:
                foreach (KeyValuePair<GameObject, InventorySlot> itemSlot in currentItemSlots) //en cada gameobject-inventoryslot
                {
                    if (itemSlot.Value.slotPosition == ItemPosition.DoubleHanded)
                    {
                        item.Add(itemSlot.Key);
                        slot.Add(itemSlot.Value);
                        return true;
                    }
                    if (itemSlot.Value.slotPosition == currentItemSlots[clickedItemSlot].item.Position) //si hay un slot con la misma posicion que el item
                    {
                        //Debug.Log("There is another slot equipped in same position as clicked slot.");
                        item.Add(itemSlot.Key);
                        slot.Add(itemSlot.Value);
                        return true;
                    }
                }
                return false;

            default:
                foreach (KeyValuePair<GameObject, InventorySlot> itemSlot in currentItemSlots) //en cada gameobject-inventoryslot
                {
                    if (itemSlot.Value.slotPosition == currentItemSlots[clickedItemSlot].item.Position) //si hay un slot con la misma posicion que el item
                    {
                        //Debug.Log("There is another slot equipped in same position as clicked slot.");
                        item.Add(itemSlot.Key);
                        slot.Add(itemSlot.Value);
                        return true;
                    }
                }
                return false;
        }
    }

    //Helper function to clear inventory.
    //#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        inventorySO.Clear();
    }
//#endif
}
