using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool lookAt = true;

    private void Update()
    {
        if (lookAt)
        {
            LookAtTarget();
        }
    }
    
    void LookAtTarget()
    {
        transform.LookAt(target);
    }
}
