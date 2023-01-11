using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableItem1 : MonoBehaviour
{
    [SerializeField] private GameObject UI_Virtual_Joystick_Interact; //TODOING Implementation
    [SerializeField] private GameObject interactItemUI; //TODOING Implementation
    [SerializeField] private Animator playerAnimator; //TODO Implementation
    [SerializeField] private StarterAssetsInputs starterAssetsInputs; //Esto es un scriptable object con los inputs. Podría crear el mío propio en vez de modificar ese.

    private bool playerIsAiming;

    #region Raycast Attributes
    //Raycast Attributes
    // Se podrá hacer en otro script? embarra un poco
    public Transform playerPos;
    private Vector3 mouseWorldPosition = Vector3.zero;
    public LayerMask aimColliderLayerMask;
    public Vector3 offset; //usado para contrarrestar el offset del aim
    #endregion

    //no sé si necesito El Down y Up en cada caso de interacción. Solo Bow?
    //Mismo con el PlayerAim y Raycast.
    public virtual void OnButtonDown()
    {
        playerIsAiming = true;
    }

    protected virtual void OnButtonUp()
    {
        playerIsAiming = true;
    }

    //Estaria bueno sacarlo del update. Podría poner una Coroutine pero el tema es que tiene que ser ejecutado en Update para funcionar correctamente.
    private void Update()
    {
        if (playerIsAiming)
        {
            Raycast();
            CharacterAimRotation();
        }
    }

    #region Raycast Functionality
    //Raycast Functionality
    // Se podrá hacer en otro script? embarra un poco
    private void Raycast()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            //debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        else
        {
            mouseWorldPosition = ray.GetPoint(10);
        }
    }
    private void CharacterAimRotation()
    {
        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = playerPos.position.y;
        Vector3 aimDirection = (worldAimTarget - offset).normalized;

        playerPos.forward = Vector3.Lerp(playerPos.forward, aimDirection, Time.deltaTime * 40f);
    }
    #endregion

}
