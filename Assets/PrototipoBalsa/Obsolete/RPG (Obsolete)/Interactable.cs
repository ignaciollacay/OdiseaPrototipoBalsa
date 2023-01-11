using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// en general se usa una interface para esto creo.
/// podria ser un scriptable object? Suma?
/// </summary>
public class Interactable : MonoBehaviour
{
    [SerializeField] string interactionTag = "Player";
    [SerializeField] bool requiresInput = true;
    private bool insideTrigger = false;

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + transform.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == interactionTag)
        {
            insideTrigger = true;
            Debug.Log(other.tag + " has entered Trigger Zone");
            if (!requiresInput)
            {
                Interact();
            }
        }
        else
        {
            insideTrigger = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == interactionTag)
        {
            insideTrigger = true;
            Debug.Log(other.tag + " has left Trigger Zone");
        }
    }

    private void Update()
    {
        if (insideTrigger)
        {
            if (Input.GetMouseButtonDown(0)) //TBD replace with input action
            {
                Interact();
            }
        }
    }
}
