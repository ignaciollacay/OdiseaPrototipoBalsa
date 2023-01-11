using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterSlot : MonoBehaviour
{
    public string letterSlot;

    public bool correctLetterInCorrectSlotTrigger = false;
    //public bool incorrectLetterInCorrectSlotTrigger = false;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Letra")
        {
            //verificacion de el tag de la letra con el tag de la letra del casillero
            if (other.gameObject.GetComponentInChildren<TextMeshProUGUI>().text == letterSlot)
            {
                correctLetterInCorrectSlotTrigger = true;
            }/*
            else
            {
                incorrectLetterInCorrectSlotTrigger = true;
            }*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Letra")
        {
            correctLetterInCorrectSlotTrigger = false;
            //incorrectLetterInCorrectSlotTrigger = false;
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Letra")
        {
            //verificacion de el tag de la letra con el tag de la letra del casillero
            if (other.gameObject.GetComponentInChildren<TextMeshProUGUI>().text == letterSlot)
            {
                correctLetterInCorrectSlotTrigger = true;
            }
            else
            {
                //incorrectLetterInCorrectSlotTrigger = true;
            }
        }
    }
    */
}