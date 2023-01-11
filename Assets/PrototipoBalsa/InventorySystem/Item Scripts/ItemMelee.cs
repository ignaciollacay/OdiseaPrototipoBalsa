using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMelee : InteractableItem
{
    public WeaponSO weaponSOType;

    private Transform parent;
    private Transform worldPosition;

    private Transform worldRotation;
    //private Vector2Int origin;

    public override void OnButtonDown()
    {
        base.OnButtonDown();
        Debug.Log("Melee Interaction. Button Down");

        //Cut();
    }
    public override void OnButtonUp()
    {
        base.OnButtonUp();
        Debug.Log("Melee Interaction. Button Up");

        //playerAnimator.SetBool("Cut", false);
    }

    //void Cut()
    //{
    //    playerAnimator.SetBool("Cut", true);
    //}


    //los parámetros de creación los quiero serializar según el objeto.
    /*public static ItemMelee CreateItemMelee(Vector3 worldPosition, Quaternion rotation, ItemSO itemSO, Transform parent)
    {
        //create object in grid
        Transform meleeWorldItemTransform = Instantiate(weaponSOType.worldItem.transform, worldPosition, rotation, parent);

        ItemMelee meleeWorldItem = meleeWorldItemTransform.GetComponent<ItemMelee>();

        meleeWorldItem.weaponSOType = weaponSOType;
        //meleeWorldItem.origin = origin; //no sé si necesito guardar esto
        meleeWorldItem.worldRotation.rotation = rotation; //no sé si necesito guardar esto

        return meleeWorldItem; //returns a ItemMelee for the created itemMelee
    }*/
    public static GameObject Create(ItemSO equippedItemSO, Transform parent)
    {
        GameObject equippedWorldItem = Instantiate(equippedItemSO.interactableWorldItem, parent);
        ItemMelee interactableType = equippedWorldItem.GetComponent<ItemMelee>();
        interactableType.weaponSOType = (WeaponSO)equippedItemSO;
        Debug.Log(equippedWorldItem.GetComponent<ItemMelee>());

        Debug.Log("Assigned ItemMelee private variable - weaponSOType - to: " + equippedWorldItem.GetComponent<ItemMelee>().weaponSOType);

        return equippedWorldItem;
    }
}