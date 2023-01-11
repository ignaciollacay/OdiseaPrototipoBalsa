using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using StarterAssets;
using Cinemachine;
/// <summary>
/// Este script se encarga de controlar la UI del Player Input (de manera dinámica, en Runtime)
/// Actualiza la imagen del Interact UI Virtual Button (según el ItemSO correspondiente)
/// Actualiza las funciones del Botón (derivadas al ItemSO correspondiente)
/// Según el Item Equipado, al equipar un Item.
/// </summary>
public class UI_InputManager : MonoBehaviour
{
    //public GameObject interactableItem;

    [SerializeField] private GameObject uiInteractButton;
    [SerializeField] private GameObject uiInteractIcon;
    [SerializeField] private Sprite defaultInteractImage;
    private Image interactImage;
    private UIVirtualJoystickInteract interactJoystick; //not used

    private void Awake()
    {
        interactImage = uiInteractIcon.GetComponent<Image>();
        interactJoystick = uiInteractButton.GetComponent<UIVirtualJoystickInteract>(); //not used
    }

    //Triggered by InventoryUI Event OnItemSelect & OnItemUnselect 
    public void UpdateIcon(ItemSO equippedItemSO) //TODO Not working
    {
        if (equippedItemSO != null)
        {
            interactImage.sprite = equippedItemSO.uIItem; //set ui icon to equippedItem icon
            //Debug.Log("Interact Button Icon updated: " + uiInteractIcon.GetComponent<Image>().sprite);
        }
        else
        {
            //Debug.Log("Equipped World Item is null. Set Default interact image");
            interactImage.sprite = defaultInteractImage;
        }
    }

    //Triggered by PlayerInventory Event OnItemEquip
    public void UpdateButton(GameObject equippedWorldItem) 
    {
        if (equippedWorldItem != null)
        {
            //InteractableItem item = interactableItem.GetComponent<InteractableItem>();
            //Debug.Log("Updating button for equipped world item " + equippedWorldItem);

            ChangeEvent(equippedWorldItem);

            //AddEvent(uiInteractButton, EventTriggerType.PointerDown, delegate { equippedWorldItem.GetComponent<InteractableItem>().OnButtonDown(); }); //set ui button up method to item equipped button up method
            //AddEvent(uiInteractButton, EventTriggerType.PointerUp, delegate { equippedWorldItem.GetComponent<InteractableItem>().OnButtonUp(); }); //set ui button down method to item equipped button up method
        }
        else
        {
            //Debug.Log("Equipped World Item is null. Set Default interact action");
            ClearEvents();
        }
    }

    //Agregar los trigger events del botón de manera dinámica, según el objeto equipado
    public void AddEvent(GameObject uiInteractButton, EventTriggerType eventType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = uiInteractButton.GetComponent<EventTrigger>(); //grab event trigger from the itemSlot Button
        var eventTrigger = new EventTrigger.Entry(); //new trigger to add to the event trigger
        eventTrigger.eventID = eventType; //set the trigger event type

        //eventTrigger.callback.RemoveAllListeners();
        //trigger.triggers.Clear();

        eventTrigger.callback.AddListener(action); //listen for the action
        trigger.triggers.Add(eventTrigger); //add the event trigger
    }
    void ChangeAction(UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = uiInteractButton.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.callback.RemoveAllListeners();
        eventTrigger.callback.AddListener(action); //listen for the action
        //trigger.triggers.Add(eventTrigger); //add the event trigger
    }

    void ChangeEvent(GameObject equippedWorldItem)
    {
        //EventTrigger trigger = uiInteractButton.GetComponent<EventTrigger>();
        //trigger.triggers.Clear();

        if (equippedWorldItem.TryGetComponent(out ItemMelee MeleeInteraction))
        {
            AddEvent(uiInteractButton, EventTriggerType.PointerDown, delegate { MeleeInteraction.OnButtonDown(); }); //set ui button up method to item equipped button up method
            AddEvent(uiInteractButton, EventTriggerType.PointerUp, delegate { MeleeInteraction.OnButtonUp(); }); //set ui button down method to item equipped button up method
        }
        if (equippedWorldItem.TryGetComponent(out ItemPlaceable placeableInteraction))
        {
            AddEvent(uiInteractButton, EventTriggerType.PointerDown, delegate { placeableInteraction.OnButtonDown(); }); //set ui button up method to item equipped button up method
            AddEvent(uiInteractButton, EventTriggerType.PointerUp, delegate { placeableInteraction.OnButtonUp(); }); //set ui button down method to item equipped button up method
        }
        if (equippedWorldItem.TryGetComponent(out ItemRanged rangedInteraction))
        {
            AddEvent(uiInteractButton, EventTriggerType.PointerDown, delegate { rangedInteraction.OnButtonDown(); }); //set ui button up method to item equipped button up method
            AddEvent(uiInteractButton, EventTriggerType.PointerUp, delegate { rangedInteraction.OnButtonUp(); }); //set ui button down method to item equipped button up method
        }
    }

    public void ClearEvents()
    {
        EventTrigger trigger = uiInteractButton.GetComponent<EventTrigger>();
        trigger.triggers.Clear();
    }
}