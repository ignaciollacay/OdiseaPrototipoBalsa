using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;
/// <summary>
/// Creates a generic inventory which hold values to different items.
/// Works for Shops, Enemies, tutorials.
/// Tutorial Ref: CodingWithUnity
/// </summary>
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventorySO : ScriptableObject
{
    [Header("Save & Load")]
    public string savePath;
    //public string databasePath;
    public ItemDatabaseSO databaseSO;
    [Header("Inventory items")]
    public Inventory inventory;

    public InventorySlot[] GetInventorySlots { get { return inventory.inventorySlots; } }

    private void Start()
    {
        //item.OnItemPicked += AddItem;
        //item.OnItemDropped += RemoveItem;
    }

    //agrega un item en el inventario
        //usado en el pickup
        //pero lo necesito ampliar para que pueda agregar objetos cuando remueve un placeable object.
    public bool AddItem(Item _item, int _amount)
    {
        //if (GetEmptySlotCount <= 0)
        //{
        //    Debug.Log("Inventory is full. Can't add new items");
        //    return false; //no empty slots left in inventory. //Not able to add item to inventory
        //}
        InventorySlot slot = GetItemSlot(_item); //gets the inventory slot of the item.
        //Debug.Log("AddItem got slot: " + slot);
        if ((slot == null) || (!databaseSO.Items[_item.Id].isStackable)) //item not stackable or not in inventory
        {
            SetItemSlot(_item, _amount); //add item in slot and set item new amount
            return true; //able to add item to inventory
        }
        slot.AddAmount(_amount); //set item new amount in inventory
        return true; //able to add item to inventory
    }

    public void RemoveItem(Item _item, int _amount)
    {
        for (int i = 0; i < GetInventorySlots.Length; i++)
        {
            if (GetInventorySlots[i].item == _item) //if the item slot in the inventory is the same as the item to remove
            {
                GetInventorySlots[i].RemoveAmount(_amount);
            }
        }
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < GetInventorySlots.Length; i++)
        {
            if (GetInventorySlots[i].item == _item) //if the item slot in the inventory is the same as the item to remove
            {
                GetInventorySlots[i].UpdateSlot(null, 0);
            }
        }
    }

    public int GetEmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetInventorySlots.Length; i++)
            {
                if (GetInventorySlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    /// <summary>
    /// Recibe un Item y busca en el inventorySO si hay un slot que contenga el mismo item.
    /// Devuelve ese inventory slot si lo encuentra, si no encuentra devuele null.
    /// </summary>
    /// <param name="_item">Item que recibe</param>
    /// <returns>InventorySlot que devuelve con el mismo</returns>
    public InventorySlot GetItemSlot (Item _item)
    {
        for (int i = 0; i < GetInventorySlots.Length; i++)
        {
            if (GetInventorySlots[i].item.Id == _item.Id) //Item is already in inventory
            {
                return GetInventorySlots[i]; //return the item in inventory
            }
        }
        return null; //Item is not in inventory
    }
    public InventorySlot SetItemSlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetInventorySlots.Length; i++)
        {
            if (GetInventorySlots[i].item.Id <= -1)
            {
                GetInventorySlots[i].UpdateSlot(_item, _amount);
                return GetInventorySlots[i]; //reference to the item
            }
        }
        return null; //functionality for full inventory
    }

    //get an inventory slot with same position as item. Used for equipping items in Equipment Inventory.
    public InventorySlot GetSlotPosition(ItemSO _itemSO)
    {
        //in each inventory slot position
        for (int i = 0; i < GetInventorySlots.Length; i++)
        {
            //if an item slot position matches ItemSO Position
            if (GetInventorySlots[i].slotPosition == _itemSO.position)
            {
                //then there is an item matching the position
                //but is it the same item?

                //return that slot position
                return GetInventorySlots[i];
            }
        }
        //if after searching all slots it didnt find an item with that slot position
        return null;
    }

    /* Faulty behaviour. Use GetSlotPosition instead
    ///<summary>
    ///get the inventoryslot with same position as item.
    ///If slot holds item in same position, it removes the item from the inventory and adds the new one. 
    ///Returns null if item position can't be found in in inventory
    ///</summary>
    public InventorySlot AddItemInSlotPosition (ItemSO _itemSO) //TODO Tira null ref error cuando el inventario no fue cleareado y agrego nuevos items.
    {
        for (int i = 0; i < GetInventorySlots.Length; i++) //In each inventory slot
        {
            if (GetInventorySlots[i].itemPosition == _itemSO.position) //Item Slot Position matches Item SO Position
            {
                if (GetInventorySlots[i].item.Id <= -1) //item slot does not hold an item
                {
                    GetInventorySlots[i].UpdateSlot(_itemSO.data); //add item to slot
                    Debug.Log("Item added. Inventory slot position was empty.");
                    return GetInventorySlots[i]; //return the slot in inventory that matches the ItemSO Position
                }
                else                                    //item slot currently holds an item
                {
                    GetInventorySlots[i].UpdateSlot(null, 0); //remove current item from slot
                    GetInventorySlots[i].item.Id = -1;
                    GetInventorySlots[i].UpdateSlot(_itemSO.data); //add new item to slot
                    Debug.Log("Item Replaced. Inventory Slot position was full.");
                    return GetInventorySlots[i];
                }
            }
        }
        Debug.Log("Can't equip item. Inventory slot position not found.");
        return null;
    }
    */
    /* Replaced with GetSlotPosition
    public InventorySlot RemoveItemInSlotPosition(ItemSO _itemSO)
    {
        for (int i = 0; i < GetInventorySlots.Length; i++) //In each inventory slot
        {
            if (GetInventorySlots[i].itemPosition == _itemSO.position) //Item Slot Position matches Item SO Position
            {
                GetInventorySlots[i].UpdateSlot(null, 0); //remove item from slot
                GetInventorySlots[i].item.Id = -1;
                Debug.Log("Item removed from slot position");
                return GetInventorySlots[i]; //return the slot in inventory that matches the ItemSO Position
            }
        }
        Debug.Log("Can't remove item. Inventory slot with same position as Item not found.");
        return null; //Item is not in inventory
    }*/

    //creo que no necesito esto. Es solo para el drag.
    //Pero creo que sirve para tener referencia del slot (y su item) en el PlayerInventory y el PlayerEquippedInventory
    public void SwapItem(InventorySlot oldItemSlot, InventorySlot updatedItemSlot)
    {
        //if it can be placed do:
            //en mi caso siempre va a poder. Pero necesito saber en qué posición va el Item, y si había un Item ya en el slot
        InventorySlot newItemSlot = new InventorySlot(updatedItemSlot.item, updatedItemSlot.amount); //store new item slot values
        updatedItemSlot.UpdateSlot(oldItemSlot.item, oldItemSlot.amount); //add new values to old item slot
        oldItemSlot.UpdateSlot(newItemSlot.item, newItemSlot.amount); //store new item slot in old item slot
    }
    

    [ContextMenu("Save")]
    public void Save()
    {
        #region JSON Alternative (obsolete)
        /* 
        //Json approach. Is editable by user in text file.
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
        */
        #endregion
        //Iformatter approach. Non editable by user.
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, inventory);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            #region JSON Alternative (obsolete)
            /* 
            //Json approach. Is editable by user in text file.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
            */
            #endregion
            //Iformatter approach. Non editable by user.
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newInventory = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetInventorySlots.Length; i++)
            {
                GetInventorySlots[i].UpdateSlot(newInventory.inventorySlots[i].item, newInventory.inventorySlots[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        inventory = new Inventory();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] inventorySlots = new InventorySlot[7];

    public void Clear()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot _slot); //Creo que es ItemEquipped 
public delegate void EquippedSlotCleared(InventorySlot _slot); //Creo que es ItemEquipped 

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    public ItemPosition slotPosition = new ItemPosition(); //deberia ser un array, en caso de Left y Right, por ahora no lo hago
        //public ItemPosition[] itemPosition = new ItemPosition[0];


    [System.NonSerialized]
    public InventoryUI linkedInventory; //so slots are distingushed between PlayerInv & PlayerInvEquipped
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnBeforeSlotUpdated; //add items callback -- OnItemEquipped ?
    [System.NonSerialized]
    public SlotUpdated OnAfterSlotUpdated; //remove items callback -- OnItemUnequipped ?


    [System.NonSerialized]
    public SlotUpdated OnEquippedSlotCleared; //remove items callback -- OnItemUnequipped ?
    //allowing items
    //public ItemPosition position; //pero quiero que esten en el slot o sea una propiedad del item


    //return itemSO in the slot, based on the Item Id.
    public ItemSO GetItemSO
    {
        get
        {
            if (item.Id >= 0) //if the id exists in the database
            {
                return linkedInventory.inventorySO.databaseSO.Items[item.Id]; //Return null
            }
            else
            {
                return null;
            }
        }
    }

    public void UpdateSlot(Item _item, int _amount)
    {
        OnBeforeSlotUpdated?.Invoke(this);
        item = _item;
        amount = _amount;
        OnAfterSlotUpdated?.Invoke(this);
    }
    public void UpdateSlot(Item _item)
    {
        item = _item;
    }

    public InventorySlot() //default inventory slot (empty)
    {
        UpdateSlot(new Item(), 0);
        //OnItemPicked?.Invoke(this);
    }

    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
        //OnItemPicked?.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void AddAmount(int value)
    {
        if (amount >= 0)
        {
            UpdateSlot(item, amount += value);
        }
        else
        {
            //UpdateSlot(null, 0);
            RemoveItem();             //TODO TODO TODO probar si va la de arriba bien y/o si con esta es mejor o igual
            Debug.Log("amount is 0, clearing slot");
            OnEquippedSlotCleared?.Invoke(this);
        }
    }
    public void RemoveAmount(int value)
    {
        if(amount > 1)
        {
            UpdateSlot(item, amount -= value);
        }
        else
        {
            UpdateSlot(null, 0);
        }
    }

    //checking for allowed items
    //usa un bool para ver si el slot del FixedInv acepta el slot del PlayerInv. public bool CanPlaceInSlot()
    //pero como yo no estoy usando el drag, quiero solo mandarlo automáticamente.
    //if ((_itemSO == null) || (_itemSO.data.Id < 0) //check if the slot is empty
}