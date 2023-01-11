using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// //Esta clase no me sirve. Refiere a UI. Yo el Inventario fijo del player quiero que sea solo sistema
/// </summary>
public class InventoryUIFixed : InventoryUI
{
    public GameObject[] slots; //TODO cambiar a InventorySlots. No uso graficos

    public override void CreateSlots()
    {
        currentItemSlots = new Dictionary<GameObject, InventorySlot>(); //make sure there are no links between database and ui

        for (int i = 0; i < inventorySO.GetInventorySlots.Length; i++)
        {
            GameObject slotUI = slots[i]; //TODO cambiar a InventorySlots. No uso graficos

            //UPDATE. Event only on Dynamic
            //AddEvent(slotUI, EventTriggerType.PointerUp, delegate { OnClick(slotUI); }); //wait for event

            inventorySO.GetInventorySlots[i].slotDisplay = slotUI; //update GameObject slotPrefab (item) in Fixed to match the GameObject slotPrefab (item) clicked in Dynamic Inv.

            currentItemSlots.Add(slotUI, inventorySO.GetInventorySlots[i]); //add the new slot to the dictionary
        }
    }
}


/// <summary>
/// intento abandonado de recrear la clase de arriba,
/// La idea era cambiar de GameObject a Inventory slots, porque el PlayerUI no tendría gameObjects de slots
/// luego integrando la clase PlayerInvnetory
///
/// El problema es el InventoryUI
/// El evento usa un gameobject, y esta entrelazado con los for loops y el diccionario
/// Y creo que lo hace para poder acceder al botón, que está en el gameobject, no en el inventorySlot
/// </summary>
/*
public class InventoryUIPlayer : InventoryUI
{
    public InventorySlot[] slots; //TODO cambiar a InventorySlots. No uso graficos

    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>(); //make sure there are no links between database and ui

        for (int i = 0; i < inventorySO.GetInventorySlots.Length; i++)
        {
            InventorySlot slotUI = slots[i]; //TODO cambiar a InventorySlots. No uso graficos

            AddEvent(slotUI, EventTriggerType.PointerUp, delegate { OnClick(slotUI); }); //wait for event

            inventorySO.GetInventorySlots[i].slotDisplay = slotUI; //update GameObject slotPrefab (item) in Fixed to match the GameObject slotPrefab (item) clicked in Dynamic Inv.

            itemsDisplayed.Add(slotUI, inventorySO.GetInventorySlots[i]); //add the new slot to the dictionary
        }
    }
}
*/