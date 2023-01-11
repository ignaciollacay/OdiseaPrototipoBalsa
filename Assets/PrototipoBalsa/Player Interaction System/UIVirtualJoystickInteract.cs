using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIVirtualJoystickInteract : UIVirtualJoystick
{
    [Header("Rotation Settings")]
    public int min = 10;
    public int max = 20;
    public bool placeableEquipped = false;
    private Vector2 _pointerPosition;

    [Header("Output")]
    public UnityEvent<InputDirection> joystickOutputEventRotation;


    public override void OutputPointerEventValue(Vector2 pointerPosition) //TODO Event -- change here Rotate / Look behaviour
    {
        if (!placeableEquipped)
        {
            base.OutputPointerEventValue(pointerPosition); // == joystickOutputEvent.Invoke(pointerPosition);
        }
        else
        {
            _pointerPosition = pointerPosition; // store pointer position value in private variable, to get the position on pointer up.
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (placeableEquipped)
        {
            //Debug.Log("Last pointer position before On Pointer Up: " + _pointerPosition);
            InputDirection rotationDirection = ItemPlaceable.GetDirection(_pointerPosition, min, max); //get direction from the last pointer position before On Pointer Up
            //Debug.Log("Got rotation direction: " + rotationDirection);
            joystickOutputEventRotation.Invoke(rotationDirection); //invoke event with the direction
            //Debug.Log("JoystickOutputEventRotation Invoked " + rotationDirection);
        }

        base.OnPointerUp(eventData);
    }

    public void PlaceableEquipped(GameObject equippedWorldItem)
    {
        placeableEquipped = true;
        //Debug.Log("Interact Joystick Placeable Equipped State is " + placeableEquipped);
    }

    public void PlaceableUnequipped()
    {
        placeableEquipped = false;
        //Debug.Log("Interact Joystick Placeable Equipped State is " + placeableEquipped);
    }
}