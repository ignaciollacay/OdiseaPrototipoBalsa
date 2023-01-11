using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public PlaceableSO placedObjectTypeSO; //public Item item;

    public delegate void ItemPicked(PlaceableSO placedObjectTypeSO);
    public event ItemPicked OnItemPicked; 

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {
        Debug.Log("Picked up " + placedObjectTypeSO.name);
        //Add to inventory
        //Debería ser un evento, pero no sé cual es el item que lo va a llamar. No puedo vincular un objeto ni una lista de objetos
        //quizas si la lista de objetos fuese un scriptableobject, con una lista de cada tipo de objetos en el juego, y referenciar eso?
        Destroy(gameObject); 
    }

    private void DropDown()
    {
        Debug.Log("Dropped down " + placedObjectTypeSO.name);

    }
}
