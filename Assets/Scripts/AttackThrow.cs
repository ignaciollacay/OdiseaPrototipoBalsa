using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackThrow : MonoBehaviour
{
    [SerializeField] Transform throwPoint;
    [SerializeField] GameObject throwObject;
    [SerializeField] float arrowSpeed = 25f;

    public Camera cam;

    public void Shoot()
    {
        //tengo que separar la anim de la variable del input. O lo dejamos en el 3PC
        //Play attack animation
        //_animator.SetBool("Attack", _input.attack);

        //replace arrow
        GameObject arrow = Instantiate(throwObject, throwPoint.position, transform.rotation);
        //falta el destroy

        //move arrow
        arrow.GetComponent<Rigidbody>().AddForce(cam.transform.forward * arrowSpeed, ForceMode.Force);
    }
}
