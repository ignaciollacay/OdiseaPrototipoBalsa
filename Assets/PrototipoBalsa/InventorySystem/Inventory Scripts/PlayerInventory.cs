/// <summary>
/// Manages the behaviour of the Equipment Inventory.
/// TODOs
/// 1. Refactor into InventoryUIFixed ?
/// 2. Refactor Equip position into array & remove BothHands Item Position
/// 3. Remove equippedWorldItem ?
/// </summary>

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory : MonoBehaviour
{
    public UI_InputManager inputManager;

    [Header("Inventory")]
    public InventorySO inventoryPlayerEquipped;
    public static GameObject equippedInteractableItem; //TODO Delete

    [SerializeField] private GameObject playerRig;  //

    [Header("Events")]
    [SerializeField] public GameEvent_GameObject OnItemEquip;
    [SerializeField] public GameEvent_GameObject OnPlaceableEquipped;
    public ItemSelected OnItemUnselected;
    public ItemSelected OnItemSelected;

    private ItemPosition[] inventorySlotPositions = //TODO Set in InventoryUIFixed
        new ItemPosition[7] 
    {
        ItemPosition.Head,
        ItemPosition.Body,
        ItemPosition.Hands,
        ItemPosition.Legs,
        ItemPosition.Feet,
        ItemPosition.LeftHand,
        ItemPosition.RightHand
    };

    private GameObject head;
    private GameObject chest;
    private GameObject hands;
    private GameObject legs;
    private GameObject feet;
    private GameObject leftHand;
    private GameObject rightHand;

    private BoneCombiner boneCombiner;

    private void Awake() //TODO Helper, Delete. Automatic inventorySO clear on ApplicationQuit
    {
        inventoryPlayerEquipped.GetInventorySlots[0].slotPosition = inventorySlotPositions[0]; //head
        inventoryPlayerEquipped.GetInventorySlots[1].slotPosition = inventorySlotPositions[1]; //chest
        inventoryPlayerEquipped.GetInventorySlots[2].slotPosition = inventorySlotPositions[2]; //hands
        inventoryPlayerEquipped.GetInventorySlots[3].slotPosition = inventorySlotPositions[3]; //legs
        inventoryPlayerEquipped.GetInventorySlots[4].slotPosition = inventorySlotPositions[4]; //feet
        inventoryPlayerEquipped.GetInventorySlots[5].slotPosition = inventorySlotPositions[5]; //left h
        inventoryPlayerEquipped.GetInventorySlots[6].slotPosition = inventorySlotPositions[6]; //right h
    }


    //TODOING Probando copiar forma InventoryUI con una lista en vez de un Dictionary.
    public List<InventorySlot> inventorySlots;
    private void Start()
    {
        boneCombiner = new BoneCombiner(playerRig);

        OnItemUnselected += UnequipItem;
        OnItemSelected += EquipItem;

        for (int i = 0; i < inventoryPlayerEquipped.GetInventorySlots.Length; i++)
        {
            inventorySlots.Add(inventoryPlayerEquipped.GetInventorySlots[i]);
        }
    }
    /// <summary>
    /// Convert Slot from inventorySO to inventory List
    /// Me permite evitar los errores por null reference cuando intento agarrar el id del slot.
    ///
    /// Get the slot that matches item position (from InventorySO to List)
    /// Replaces old functionality -- InventorySlot _slot = inventoryPlayerEquipped.GetSlotPosition(_itemSO);
    /// Which gave null ref errors
    /// </summary>
    public InventorySlot GetSlotFromList(ItemSO _itemSO)
    {
        InventorySlot _slot = inventoryPlayerEquipped.GetSlotPosition(_itemSO); //Get the slot that matches item position

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i] == _slot)
            {
                //Debug.Log("1. " + inventorySlots[i].item.Id);
                _slot = inventorySlots[i];
                //Debug.Log("2. " + _slot.item.Id);
                return _slot;
            }
        }
        return null;
    }
    //necesito ver cual es el slot del inventory que tiene la misma posición que la posición indicada en el ItemSO
    //tira problema en acceder a la data del slot
    public void EquipItem(ItemSO _itemSO)
    {
        //Get the slot that matches item position 
        InventorySlot _slot = inventoryPlayerEquipped.GetItemSlot(_itemSO.data);
        //InventorySlot _slot    = GetSlotFromList(_itemSO);
        Debug.Log("3. " + _slot.item.Id);
        if (_slot != null)
        {
            if (_slot.item.Id <= -1) //if slot is empty //TODO Solo está funcionando en el primer item equipado, luego falla.
            {
                Debug.Log("Inventory slot item id is... " + _slot.item.Id);
                Debug.Log("Inventory slot position is empty. Adding item...");
                Equip(_itemSO, _slot);
            }
            else //slot is full
            {
                Debug.Log("Inventory Slot position is full. Replacing item...");
                Unequip(_slot);
                Equip(_itemSO, _slot);
            }
        }
        else
        {
            Debug.Log("Inventory slot position not found... Can't equip that item.");
        }
    }

    public void UnequipItem(ItemSO _itemSO)
    {
        //InventorySlot _slot = inventoryPlayerEquipped.GetSlotPosition(_itemSO); //TODOING TESTING. Reemplazo por la funcion de abajo
        InventorySlot _slot = inventoryPlayerEquipped.GetItemSlot(_itemSO.data);

        if (_slot != null) 
        {
            if (_slot.item.Id > -1) //if there is an item in the slot
            {
                Unequip(_slot);
            }
            else // if there is not an item in the slot
            {
                Debug.Log("Can't unequip, Slot is empty");
            }
        }
        else
        {
            Debug.Log("Inventory slot position not found... Can't unequip.");
        }
    }

    private void Equip(ItemSO _itemSO, InventorySlot _slot)
    {
        _slot.UpdateSlot(_itemSO.data);
        CreateWorldItem(_itemSO);
        Debug.Log("Item equipped");
    }

    private void Unequip(InventorySlot _slot)
    {
        _slot.UpdateSlot(null, 0); //clear slot
        //Destroy(equippedInteractableItem); //clear equippedWorldItem
        DestroyWorldItem(_slot.GetItemSO);
        Debug.Log("Item unequipped");


        //interactableEquipped = false;
    }

    //TODOING Testing CreateWorldItemv2
    public void CreateWorldItem(ItemSO _itemSO)
    {
        //case equipment
        //create object and add bones
        switch (_itemSO.position)
        {
            case ItemPosition.Head:
                head = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(head, _itemSO.boneNames); //helmet
                inventoryPlayerEquipped.GetInventorySlots[0].UpdateSlot(_itemSO.data);
                break;
            case ItemPosition.Body:
                chest = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(chest, _itemSO.boneNames); //armor
                inventoryPlayerEquipped.GetInventorySlots[1].UpdateSlot(_itemSO.data);
                break;
            case ItemPosition.Hands:
                hands = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(hands, _itemSO.boneNames); //gauntlets
                inventoryPlayerEquipped.GetInventorySlots[2].UpdateSlot(_itemSO.data);
                break;
            case ItemPosition.Legs:
                legs = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(legs, _itemSO.boneNames); //pantalon
                inventoryPlayerEquipped.GetInventorySlots[3].UpdateSlot(_itemSO.data);
                break;
            case ItemPosition.Feet:
                feet = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(feet, _itemSO.boneNames); //shoes
                inventoryPlayerEquipped.GetInventorySlots[4].UpdateSlot(_itemSO.data);
                break;
            case ItemPosition.LeftHand:
                leftHand = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(leftHand, _itemSO.boneNames);
                inventoryPlayerEquipped.GetInventorySlots[5].UpdateSlot(_itemSO.data);
                
                break;
            case ItemPosition.RightHand:
                rightHand = Instantiate(_itemSO.interactableWorldItem, playerRig.transform);
                boneCombiner.CombineBones(rightHand, _itemSO.boneNames);
                inventoryPlayerEquipped.GetInventorySlots[6].UpdateSlot(_itemSO.data);

                equippedInteractableItem = rightHand;

                inputManager.UpdateButton(equippedInteractableItem);
                inputManager.UpdateIcon(_itemSO);

                OnItemEquip.RaiseEvent(equippedInteractableItem);

                //switch (_itemSO.interactableType)
                //{
                //    case InteractableType.WeaponRanged:
                //        ItemRanged itemRanged = equippedInteractableItem.AddComponent<ItemRanged>();
                //        itemRanged.weaponSOType = (WeaponSO)_itemSO;
                //        break;
                //    case InteractableType.WeaponMelee:
                //        ItemMelee itemMelee = equippedInteractableItem.AddComponent<ItemMelee>();
                //        itemMelee.weaponSOType = (WeaponSO)_itemSO;
                //        break;
                //    case InteractableType.Placeable:
                //        ItemPlaceable itemPlaceable = equippedInteractableItem.AddComponent<ItemPlaceable>();
                //        itemPlaceable.placeableSOType = (PlaceableSO)_itemSO;
                //        InvokePlaceableEvent();
                //        break;
                //    default:
                //        break;
                //}
                break;
            case ItemPosition.DoubleHanded:
                if (leftHand)
                {
                    Destroy(leftHand);
                    inventoryPlayerEquipped.GetInventorySlots[5].UpdateSlot(null);
                }

                switch (_itemSO.interactableType)
                {
                    case InteractableType.WeaponRanged:
                        rightHand = ItemRanged.Create(_itemSO, playerRig.transform);
                        boneCombiner.CombineBones(rightHand, _itemSO.boneNames);
                        equippedInteractableItem = rightHand;
                        break;
                    case InteractableType.Placeable:
                        rightHand = ItemPlaceable.Create(_itemSO, playerRig.transform);
                        equippedInteractableItem = rightHand;
                        InvokePlaceableEvent();
                        break;
                    default:
                        break;
                }
                inventoryPlayerEquipped.GetInventorySlots[5].UpdateSlot(_itemSO.data);
                inventoryPlayerEquipped.GetInventorySlots[6].UpdateSlot(_itemSO.data);

                inputManager.UpdateButton(equippedInteractableItem);
                inputManager.UpdateIcon(_itemSO);

                OnItemEquip.RaiseEvent(equippedInteractableItem);

                break;

        }
    }

    public void DestroyWorldItem(ItemSO _itemSO)
    {
        //case equipment
        //create object and add bones
        switch (_itemSO.position)
        {
            case ItemPosition.Head:
                Destroy(head);
                inventoryPlayerEquipped.GetInventorySlots[0].RemoveItem();
                break;
            case ItemPosition.Body:
                Destroy(chest);
                inventoryPlayerEquipped.GetInventorySlots[1].RemoveItem();
                break;
            case ItemPosition.Hands:
                Destroy(hands);
                inventoryPlayerEquipped.GetInventorySlots[2].RemoveItem();
                break;
            case ItemPosition.Legs:
                Destroy(legs);
                inventoryPlayerEquipped.GetInventorySlots[3].RemoveItem();
                break;
            case ItemPosition.Feet:
                Destroy(feet);
                inventoryPlayerEquipped.GetInventorySlots[4].RemoveItem();
                break;
            case ItemPosition.LeftHand:
                Destroy(leftHand);
                inventoryPlayerEquipped.GetInventorySlots[5].RemoveItem();
                break;
            case ItemPosition.RightHand:
                Destroy(rightHand);
                inventoryPlayerEquipped.GetInventorySlots[6].RemoveItem();
                inputManager.UpdateIcon(null);
                inputManager.UpdateButton(null);
                break;
            case ItemPosition.DoubleHanded:
                Destroy(rightHand);

                inventoryPlayerEquipped.GetInventorySlots[5].RemoveItem();
                inventoryPlayerEquipped.GetInventorySlots[6].RemoveItem();

                inputManager.UpdateIcon(null);
                inputManager.UpdateButton(null);
                break;
            default:
                break;
        }
    }

    private void InvokePlaceableEvent()
    {
        OnPlaceableEquipped.RaiseEvent(equippedInteractableItem);
        //Debug.Log("OnPlaceableEquipped Event Raised. Equipped item is: " + equippedWorldItem);
    }


    //Helper function to clear inventory.
//#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        inventoryPlayerEquipped.Clear();
    }
//#endif
}
