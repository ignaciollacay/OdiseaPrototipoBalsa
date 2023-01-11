using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWaitCoroutine : MonoBehaviour
{
    [SerializeField]private PublicWaiter waiter;
    public int firstInt;
    public int secondInt;
    public int thirdInt;

    private int multiplyResult;
    private int addResult;

    void Multiply(int myFirstInt, int mySecondInt)
    {
        multiplyResult = myFirstInt * mySecondInt;
        Debug.Log("Multiply result is " + multiplyResult);
    }

    void Add(int myFirstInt, int mySecondInt)
    {
        addResult = myFirstInt + mySecondInt;
        Debug.Log("Add Result is " + addResult);
    }

    private void Update() //private void Start() //not working, needs an event
    {
        Multiply(1, 2);
        StartCoroutine(waiter.Delay(waiter.waitTime));
        if (waiter.DelayFinished)
        {
            Add(multiplyResult, thirdInt);
        }
    }
    ///<summary>
    /// Este fue un primer intento de hacer una funcion generica de una Coroutine que haga esperar entre la ejecucion de una funcion
    /// la idea es no tener que armar cada vez una Coroutine de WaitForSeconds
    /// No funciono en Start, en tal caso requiere de un evento.
    ///
    /// otras cosas a probar
    ///     Events
    ///         (en vez del Update)
    ///     Interfaces
    ///         (en vez de la clase de Public Waiter)
    ///</summary>
}
