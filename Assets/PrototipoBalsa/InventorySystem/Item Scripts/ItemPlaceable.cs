using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

/// <summary>
///Interaction Controller - Placeable Item
/// 
/// Creo que esta clase se superpone en parte con el PlacedObject.
/// Ambos son Monos del ItemSO instanceado al crear el world item
/// El GameEvent de OnItemEquipped lo estoy mandando desde PlacedObject,
/// porque mantiene ref del ItemSO al ser creado.
/// Ver de implementar al crear el world item en PlayerInventory.
/// </summary>
public class ItemPlaceable : InteractableItem
{
    public PlaceableSO placeableSOType;

    //public GameEvent_GameObject OnPlaceableEquip;
    //public GameEvent_GameObject OnPlaceableUnequip;

    private Vector2Int origin;
    private PlaceableSO.ItemDirection dir;

    private CinemachineVirtualCamera playerAimCamera;
    [SerializeField] private float normalSensitivity = 1;
    [SerializeField] private float aimSensitivity = 0.8f;
    [SerializeField] private float aimCameraDistance = 4f;

    private Camera playerCamera;
    public override void Awake()
    {
        base.Awake();
        playerCamera = Camera.main;
    }

    private void Start()
    {
        Debug.Log("OnStart called by " + this.gameObject.name, this.gameObject.gameObject);
        playerAimCamera = PlayerManager.Instance.playerAimCamera;
        SetAimCameraDistance();

        playerRaycast.SetRaycastLayerMask(true);
        playerRaycast.enabled = true;

        playerAimCamera.gameObject.SetActive(true);
        PlayerManager.Instance.playerController.SetSensitivity(aimSensitivity);

        //OnPlaceableEquip.RaiseEvent(this.gameObject);
        //GetComponent<ItemSO>().OnPlaceableEquip.RaiseEvent(this.gameObject); //sería algo así para que el vínculo del evento este guardado en el SO y usarlo.
    }

    public override void OnButtonDown()
    {
        //base.OnButtonDown();
        //Debug.Log("Placeable Interaction. Button Down");
    }
    public override void OnButtonUp()
    {
        //base.OnButtonUp();
        
        //Debug.Log("Placeable Interaction");
        Interact(playerInputs.rotate);
    }

    public void Interact(InputDirection rotate)
    {
        //TODO Simplificar lo que hace el Place y Rotate desde acá en vez de desde el GridBuildingSystem
        if (rotate == InputDirection.None)
        {
            //Debug.Log("Place item called");
            GridBuildingSystem.Instance.PlaceObject();

                //puedo enviar en parámetro el List<Vector2Int> gridPositionList de GetGridPosition?
                //porque en el Place el Grid viene a buscar la info acá
                //pero sabemos el placeObjectType
        }
        else
        {
            GridBuildingSystem.Instance.RotateItem(playerInputs.rotate);
            //Debug.Log("Rotate item");
        }
    }

    //TODO no funciona bien cuando switcheo. Se ejecuta más tarde que el Start del proximo equippedWorldItem
    //private void OnDestroy()
    //{
    //    Debug.Log("OnDestroy called by " + this.gameObject.name, this.gameObject.gameObject);
    //    //playerAimCamera.gameObject.SetActive(false);
    //    //PlayerManager.Instance.playerController.SetSensitivity(normalSensitivity);
    //    //playerRaycast.enabled = false;
    //    //OnPlaceableUnequip.RaiseEvent(this.gameObject);
    //}


    //get the grid positions that the object is occupying
    public List<Vector2Int> GetGridPositionList()
    {
        return placeableSOType.GetGridPositionList(origin, dir);
    }

    //destroy placed object prefab
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void SetAimCameraDistance()
    {
        CinemachineComponentBase componentBase = playerAimCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = aimCameraDistance;
        }
    }

    //Create PlacedObject (idem PlacedObject Script, al cual vendría a reemplazar)
    public static ItemPlaceable CreateItemPlaceable(Vector3 worldPosition, Vector2Int origin, PlaceableSO.ItemDirection dir, PlaceableSO placedObjectTypeSO, Transform parent)
    {
        //create object in grid
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.pfPlacedObject, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        ItemPlaceable placedObject = placedObjectTransform.GetComponent<ItemPlaceable>();

        placedObject.placeableSOType = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    //Create EquippableWorldItem
    public static GameObject Create(ItemSO equippedItemSO, Transform parent)
    {
        //Vector3 offset = new Vector3(0, 0, 0);
        //Vector3 playerRotation = positionInPlayer.rotation.eulerAngles;
        //Vector3 rotationV3 = playerRotation + playerRotation;
        //Quaternion rotation = Quaternion.Euler(rotationV3);

        GameObject equippedWorldItem = Instantiate(equippedItemSO.interactableWorldItem, parent);
        ItemPlaceable interactableType = equippedWorldItem.GetComponent<ItemPlaceable>();
        interactableType.placeableSOType = (PlaceableSO)equippedItemSO;

        //Debug.Log("Assigned ItemPlaceable private variable - PlaceableSOType - to: " + equippedWorldItem.GetComponent<ItemPlaceable>().placeableSOType);

        return equippedWorldItem;
    }

    public static InputDirection GetDirection(Vector2 pointerPosition, int min, int max)
    {
        if ((pointerPosition.x < -max) && ((pointerPosition.y <= min) || (pointerPosition.y >= -min)))
        {
            return InputDirection.Left;
        }

        if ((pointerPosition.x > max) && ((pointerPosition.y <= min) || (pointerPosition.y >= -min)))
        {
            return InputDirection.Right;
        }

        if (((pointerPosition.x <= min) || (pointerPosition.x >= -min)) && (pointerPosition.y > max))
        {
            return InputDirection.Down;
        }

        if (((pointerPosition.x <= min) || (pointerPosition.x >= -min)) && (pointerPosition.y < -max))
        {
            return InputDirection.Up;
        }

        else
            return InputDirection.None;
    }
}

public enum InputDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}

