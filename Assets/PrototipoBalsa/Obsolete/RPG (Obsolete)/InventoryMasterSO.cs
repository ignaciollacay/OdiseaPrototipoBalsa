using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/InventoryMaster")]
public class InventoryMasterSO : ScriptableObject
{
    public List<PlaceableSO> inventoryMaster;
}
