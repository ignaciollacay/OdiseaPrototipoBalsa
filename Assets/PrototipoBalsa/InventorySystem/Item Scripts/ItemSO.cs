using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// base class for creating other item assets
/// Tutorial ref: CodingWithUnity - https://youtu.be/_IqTeruf3-s 
/// </summary>
///
public enum ItemType
{
    Placeable,
    Weapons, //TODO Remove, replaced with InteractionType
    WeaponRanged, WeaponMelee, //TODO Remove, replaced with InteractionType
    
    Equipment, Interactable
}
public enum ItemPosition
{
    None,
    Head, Body, Hands, Legs, Feet, //equipment
    LeftHand, RightHand, DoubleHanded //interactable items
}
public enum InteractableType
{
    None, WeaponRanged, WeaponMelee, Placeable
}

//public enum PlayerEquipmentType ?
public abstract class ItemSO : ScriptableObject
{
    [Header("Item Properties")]
    public Sprite uIItem;
    public GameObject interactableWorldItem;
    public GameObject pickableWorldItem;
    public ItemType type;
    [TextArea (5, 10)]
    public string description;

    public ItemPosition position;
    public InteractableType interactableType;

    public Item data = new Item();
    public bool isStackable = true;

    //public SkinnedMeshRenderer skinnedMeshRenderer;
    [Header("Interactable Items Properties")]
    public GameObject playerRigItem;    //Referenciar al item en el PlayerRig FBX, para mantener vinculo con huesos
    public List<string> boneNames;

    private void OnValidate()
    {
        //if it already has boneNames, return
        //but would need a function to update bones.
        //and remember to update it when changing the item
        boneNames.Clear();
        if (playerRigItem == null)
            return;
        if (!playerRigItem.GetComponent<SkinnedMeshRenderer>())
            return;
        SkinnedMeshRenderer renderer = playerRigItem.GetComponent<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;
        foreach (Transform bone in bones)
        {
            boneNames.Add(bone.name);
        }
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemPosition Position;

    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemSO item)
    {
        Name = item.name;
        Id = item.data.Id;
        Position = item.position;
    }
}
