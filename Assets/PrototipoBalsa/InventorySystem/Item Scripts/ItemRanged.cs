using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using StarterAssets;
using Cinemachine;
/// <summary>
/// Interaction Controller, Ranged Weapon
/// </summary>
public class ItemRanged : InteractableItem
{
    public WeaponSO weaponSOType;

    //static player variables
    //private StarterAssetsInputs playerInputs;
    private CinemachineVirtualCamera playerAimCamera;
    private Transform projectileSpawnPosition;

    [SerializeField] private float normalSensitivity = 1;
    [SerializeField] private float aimSensitivity = 0.5f;
    [SerializeField] private float aimCameraDistance = 2.5f;
    [SerializeField] private GameObject pfProjectile;
    //public bool drawFinished = false;

    public override void Awake()
    {
        //Set Player Interaction Controller variables
        base.Awake(); //set raycast to enabled
        
        playerAimCamera = PlayerManager.Instance.playerAimCamera;
        projectileSpawnPosition = PlayerManager.Instance.projectileSpawnPosition;

        
    }
    private void Start()
    {
        playerRaycast.SetRaycastLayerMask(false);
        SetAimCameraDistance();
    }
    public override void OnButtonDown()
    {
        base.OnButtonDown(); //set raycast to enabled
        Debug.Log("Ranged Interaction. Button Down");
        Draw();
    }
    public override void OnButtonUp()
    {
        base.OnButtonUp(); //set raycast to disabled
        Debug.Log("Ranged Interaction. Button Up");
        Shoot();
    }

    private void Draw()
    {
        playerAnimator.SetBool("Draw", true);
        playerAnimator.SetBool("Undraw", false);

        //FMODUnity.RuntimeManager.PlayOneShot(bowDraw);
        playerAimCamera.gameObject.SetActive(true);
        PlayerManager.Instance.playerController.SetSensitivity(aimSensitivity);
        PlayerManager.Instance.playerController.SetRotateOnMove(false);
    }
    void Undraw()
    {
        playerAimCamera.gameObject.SetActive(false);
        PlayerManager.Instance.playerController.SetSensitivity(normalSensitivity);
        PlayerManager.Instance.playerController.SetRotateOnMove(true);
    }
    private void Shoot()
    {
        playerAnimator.SetBool("Draw", false);
        
        if (playerAnimator.GetBool("DrawFinished"))
        {
            ShootProjectile();
        }
        else
        {
            playerAnimator.SetBool("Undraw", true);
            Undraw();

            //FMODUnity.RuntimeManager.PlayOneShot(bowUndraw);
        }

        //playerAnimator.SetBool("Drawed", false);
    }
    void ShootProjectile() //run from animator
    {
        Vector3 aimDir = (playerRaycast.mouseWorldPosition - projectileSpawnPosition.position).normalized;
        Instantiate(pfProjectile, projectileSpawnPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

        //FMODUnity.RuntimeManager.PlayOneShot(arrowRelease);
    }

    private void SetAimCameraDistance()
    {
        CinemachineComponentBase componentBase = playerAimCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = aimCameraDistance;
        }
    }

    public static GameObject Create(ItemSO equippedItemSO, Transform parent)
    {
        GameObject equippedWorldItem = Instantiate(equippedItemSO.interactableWorldItem, parent);
        ItemRanged interactableType = equippedWorldItem.GetComponent<ItemRanged>();
        interactableType.weaponSOType = (WeaponSO)equippedItemSO;

        Debug.Log("Assigned ItemRanged private variable - WeaponSOType - to: " + equippedWorldItem.GetComponent<ItemRanged>().weaponSOType);
        return equippedWorldItem;
    }
}
