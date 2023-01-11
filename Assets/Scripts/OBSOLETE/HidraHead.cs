using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidraHead : MonoBehaviour
{
    Hidra _hidra; // tengo que asignarlo a un objeto?
    public bool markedHead = false;
    
    private Animator _animator;

    //HITBOX HIDRA HEAD
    private void OnTriggerEnter(Collider other)
    {
        //checkear que sea una flecha que entra en el hitbox
        if (other.tag == "Arrow")
        {
            HeadKilled();
            /*
            //esto no sirve. lo manejamos con el animator component bools
            //Si la cabeza es correcta
            if (markedHead)
            {
                CorrectHead();
            }
            //si la cabeza es incorrecta
            else
            {
                IncorrectHead();
            }
            */
        }
    }

    void HeadKilled()
    {
        _animator.SetBool("headKilled", true);
    }

    //creo que se puede manejar con el bool del animator y con eventos en la anim para el sonido
    void CorrectHead()
    {
        //Head Killed Animation
        _animator.SetBool("headKilled", true);
        //Play Sound

        //aca necesito un callback al script hidra para que corrobore si faltan cabezas.
        _hidra.HeadKilled();
    }
    void IncorrectHead()
    {
        //Head Regenerates Animation
        _animator.SetBool("headRegen", true);
    }
}
