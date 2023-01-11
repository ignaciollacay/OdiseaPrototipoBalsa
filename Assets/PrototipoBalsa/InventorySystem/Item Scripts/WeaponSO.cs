using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// debería poner acá las propiedades de interacción?
/// UI Button Interaction refs and actions?
/// </summary>
[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Inventory/Items/Weapon")]
public class WeaponSO : ItemSO
{
    //public GameObject equippableWorldItem;
    [Header("Weapon Properties")]
    public int damage;

    public void Awake()
    {
    }
}
