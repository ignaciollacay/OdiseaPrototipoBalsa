using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Items/Equipment")]

public class EquipmentSO : ItemSO
{
    //public GameObject equippableWorldItem;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
