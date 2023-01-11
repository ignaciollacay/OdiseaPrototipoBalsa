using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySO inventorySO;
    public InventorySO inventoryEquippedSO;

    private void Save() // public? Event ref?
    {
        inventorySO.Save();
        inventoryEquippedSO.Save();
        Debug.Log("Inventory Saved");
    }

    private void Load() //public? Event ref?
    {
        inventorySO.Load();
        inventoryEquippedSO.Load();
        Debug.Log("Inventory Loaded");
    }

    private void OnApplicationQuit()
    {
        //inventorySO.inventory.Clear();
        //inventoryEquippedSO.inventory.Clear();
        inventorySO.Clear();
        inventoryEquippedSO.Clear();
    }

    //TODO temporal, remove
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N)) // TBD. Change input to Save button in Menu when UI implemented. Could be a event that triggers Save Function directly
    //    {
    //        Save();
    //    }
    //    if (Input.GetKeyDown(KeyCode.M)) // TBD. Change input to Load button in Menu when UI implemented. Could be a event that triggers Load Function directly
    //    {
    //        Load();
    //    }
    //}
}
