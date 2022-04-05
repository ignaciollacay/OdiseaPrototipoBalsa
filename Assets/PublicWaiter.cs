using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PublicWaiter : MonoBehaviour
{
    public int waitTime;
    public bool DelayFinished = false; //el problema de tenerlo aca es que no puede ejecutarse simultaneamente en distintos lados

    public IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        DelayFinished = true;
        Debug.Log("Wait finished ? " + DelayFinished);
    }
}


/*
public class PrivateWaiter : MonoBehaviour
{
    public IEnumerator Delay(int seconds, bool DelayState)
    {
        bool newDelayState = false;
        DelayState = false;
        yield return new WaitForSeconds(seconds);
        newDelayState = true;
    }
*/
