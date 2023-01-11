using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public bool MouseUp = false;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MouseUp = true;
            Debug.Log("Mouse up state is " + MouseUp);
        }
        else
        {
            MouseUp = false;
        }
    }
}
