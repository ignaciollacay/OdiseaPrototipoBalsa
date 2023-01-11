using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using Cinemachine;

/// <summary>
/// Extends ThirdPersonController functionality for interaction
/// Currently manages functionality for Bow Interaction
/// TODO Necesito que varíe según el tipo de arma que tenga
/// TODO especialmente con el PlaceableObject
/// </summary>

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform debugTransform;

    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;

    [SerializeField] private GameObject pfProjectile; //RANGEDWEAPON
    [SerializeField] private Transform projectileSpawnPosition; //RANGEDWEAPON

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private bool drawFinished = false; //RANGEDWEAPON

    [SerializeField] private Vector3 offset; //usado para contrarrestar el offset del aim

    //testing input
    private Animator _animator;

    private Vector3 mouseWorldPosition = Vector3.zero;

    //public bool holdInputState = false;
    //public bool releaseInputState = false;
    public bool playerIsAiming = false;

    //SOUND EVENT EMITTERS
    //[Header("Sound Events")]
    //public string arrowRelease = "FMOD Event Path"; //
    //public string bowDraw = "FMOD Event Path"; //
    //public string bowDrawIdle = "FMOD Event Path"; //medio desprolija la solucion. Seguro lleva a bugs.
    //public string bowUndraw = "FMOD Event Path"; //

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
    }

    public void OnButtonDown()
    {
        Aim();
        playerIsAiming = true;
    }
    public void OnButtonUp()
    {
        Shoot();
        playerIsAiming = false;
    }

    void Aim()
    {
        _animator.SetBool("Draw", true);
        _animator.SetBool("Undraw", false);

        //FMODUnity.RuntimeManager.PlayOneShot(bowDraw);

        aimVirtualCamera.gameObject.SetActive(true);
        thirdPersonController.SetSensitivity(aimSensitivity);
        thirdPersonController.SetRotateOnMove(false);

        //Vector3 worldAimTarget = mouseWorldPosition;
        //worldAimTarget.y = transform.position.y;
        //Vector3 aimDirection = (worldAimTarget - offset).normalized;
    }

    void notAiming()
    {
        _animator.SetBool("Draw", false);
        drawFinished = false;

        aimVirtualCamera.gameObject.SetActive(false);
        thirdPersonController.SetSensitivity(normalSensitivity);
        thirdPersonController.SetRotateOnMove(true);
    }

    void Shoot()
    {
        if (drawFinished)
        {
            _animator.SetBool("Attack", true);
            _animator.SetBool("Undraw", false);
            notAiming();
        }
        else
        {
            _animator.SetBool("Undraw", true);
            notAiming();

            //FMODUnity.RuntimeManager.PlayOneShot(bowUndraw);
        }
    }

    void ShootProjectile()
    {
        Vector3 aimDir = (mouseWorldPosition - projectileSpawnPosition.position).normalized;
        Instantiate(pfProjectile, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

        //FMODUnity.RuntimeManager.PlayOneShot(arrowRelease);

        starterAssetsInputs.attack = false;
        _animator.SetBool("Attack", false);
    }

    void Raycast()
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

    void DrawFinished()
    {
        drawFinished = true;
        //FMODUnity.RuntimeManager.PlayOneShot(bowDrawIdle);
    }

    private void Update()
    {
        if (playerIsAiming)
        {
            //CHARACTER AIM ROTATION
            Raycast();
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - offset).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 40f);
            //transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 40f);
            //starterAssetsInputs.look.x
        }
    }


    /*
    //non aim interaction
    public void AimShoot()
    {
        Raycast();
        _animator.SetBool("Attack", true);
        StartCoroutine(WaitForAttackAnimation());
    }
    IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("Attack", false);
    }
    */

    /*
    private void Update()
    {
        //testeo, hay que pasarlo al inputasset
        //PRESS
        if (holdInputState)//(Input.GetMouseButton(0))
        {
            Aim();
            holdingMouseButton = true;
        }
        //RELEASE
        if (holdingMouseButton)
        {
            if (releaseInputState)//(Input.GetMouseButtonUp(0))
            {
                if (drawFinished)
                {
                    _animator.SetBool("Attack", true);
                    holdingMouseButton = false;
                    _animator.SetBool("Undraw", false);
                    notAiming();
                }
                else
                {
                    _animator.SetBool("Undraw", true);
                    notAiming();
                }
            }
            else
            {
                _animator.SetBool("Attack", false);
            }
        }
    }*/
}
