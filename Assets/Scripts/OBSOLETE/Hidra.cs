using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidra : MonoBehaviour
{
    /// <PROBLEMA>
    /// establezco manualmente las cabezas. Pero en el momento en el que se elige un numero de cabezas, no estan creandose mas cabezas
    /// Crear cabezas instanceadas no seria una buena idea. Tendria que desplazarlas, seria todo un quilombo
    ///
    /// Lo que se me ocurre es que el modelo de la hidra venga con el numero maximo de cabezas.
    /// Segun el numero, tiene que eliminar las cabezas que sobren, desde los extremos hacia al centro.
    ///
    /// Igual esto no es el problema mas grande.
    /// De hecho, tambien lo puedo solucionar con prefab variants para cada numero de cabezas, y llamar al correspondiente segun el numero de cabezas total
    ///
    /// 
    /// 
    /// </summary>
    public GameObject[] heads; //must have HidraHead Script attached
    GameObject headMarked;

    int firstMultiple; //numero multiplo cabeza

    //FIXED INITIALIZED VARIABLES
    int multiplier = 1;
    //int minHeadTotal = 3; //no lo estaria usando si lo defino segun el first multiple
    int maxHeadTotal = 12; //no lo estaria usando si uso el max head amount

    int headsTotal;
    int headAmount;
    int minHeadAmount;
    int maxHeadAmount = 12;

    //animacion headKilled;
    //animacion headRegenerate;

    //newHeadMarked

    private void Start()
    {
        //MARCAR CABEZA
        firstMultiple = Random.Range(2, 5); //definir aleatoriamente el primer multiplo
        Debug.Log("Selected first multiple is " + firstMultiple);
        headMarked = heads[firstMultiple]; //asignar el multiplo a la cabeza correspondiente en el array de cabezas
        Debug.Log("Hidra first marked head is " + headMarked.name, headMarked);
        //DEFINIR CANTIDAD DE CABEZAS
        minHeadAmount = firstMultiple * 2; //que hayan al menos dos cabezas a disparar por hidra
        headAmount = Random.Range(minHeadAmount, maxHeadAmount);
        Debug.Log("Hidra head amount is " + headAmount);
        //MARCAR CABEZA
        AssignMarkedHead(headMarked);
    }

    //MARCAR CABEZA CORRECTA
    void AssignMarkedHead(GameObject hidraHead)
    {
        hidraHead.GetComponent<HidraHead>().markedHead = true;
        hidraHead.GetComponent<Animator>().SetBool("markedHead", true);
    }

    //esto ya es otro script para el personaje, que tengo que vincular con este script (callback?)
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //bow recoil
            //increased zoom
        }
        if (Input.GetMouseButtonUp(0))
        {
            //shoot arrow
        }
    }
    /// <summary>
    /// esto esta mal. El usuario puede pegarle a cualquier cabeza dentro del conjunto
    /// mientras sea parte de la tabla de multiplos.
    /// </summary>
    //se asigna una nueva cabeza/multiplo
    public void HeadKilled()
    {
        multiplier++; //definir proximo multiplicador para definir el prox multiplo
        int newHeadMarked = firstMultiple * multiplier; //definir el numero de cabeza del proximo multiplo
        
        //si aun quedan multiplos en el conjunto de cabezas
        if(newHeadMarked < headAmount)
        {
            headMarked = heads[newHeadMarked]; //asignar marca a la nueva cabeza en Hidra Script
            AssignMarkedHead(headMarked); //asignar marca a la nueva cabeza en Hidra Head Script
        }
        //si no quedan multiplos en el conjunto de cabezas
        else
        {
            AllHeadsKilled();
        }
    }
    void AllHeadsKilled()
    {
        //Hidra Death Anim
    }
    //saber si el player clickea en alguna cabeza
    //disparar en el momento que clickea
    //cuando dispara a una cabeza... saber si fue correcta o no
    //si es correcta, headKilled
    //si es incorrecta, headRegenerate
    //cuando mata una cabeza...saber si quedan cabezas o no
    //si no quedan cabezas, hidraKilled
}
