using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowAttack : MonoBehaviour
{/*
    public Camera orthoCam;
    public GameObject crosshair;
    public Sprite crosshairPoint;
    public Sprite crosshairAim;
    bool isDragged = false;
    Vector3 inputStartPosition;

    public GameObject arrowObject;
    public Transform arrowPoint;
    public float arrowSpeed = 25f;

    private void OnMouseDown()
    {
        isDragged = true;
        inputStartPosition = orthoCam.ScreenToWorldPoint(Input.mousePosition);
        crosshairPoint = crosshair.GetComponent<Image>().sprite; // guardo el crosshair inicial
        crosshair.GetComponent<Image>().sprite = crosshairAim; // cambio el crosshair a punteria
        crosshair.transform.position = inputStartPosition; // posicion crosshair a posicion presion pantalla
    }
    private void OnMouseDrag()
    {
        if (isDragged)
        {
            crosshair.transform.localPosition = crosshair.transform.position + (orthoCam.ScreenToWorldPoint(Input.mousePosition) - inputStartPosition); //punteria drag
        }
    }
    private void OnMouseUp()
    {
        isDragged = false;
        //lanzar flecha a posicion donde dejo de apretar
        //posicion donde dejo de arrastrar

        GameObject arrow = Instantiate(arrowObject, arrowPoint.position, transform.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(crosshair.transform.localPosition * arrowSpeed, ForceMode.Impulse);

        //swtich back crosshair
        crosshair.GetComponent<Image>().sprite = crosshairPoint;
    }*/
}
