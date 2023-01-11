using UnityEngine;

public class RayCast : MonoBehaviour
{
    public Vector3 mouseWorldPosition = Vector3.zero;
    [SerializeField] private LayerMask PlaceableItemRaycastLayerMask; //Set to default on Weapons
    [SerializeField] private LayerMask WeaponRaycastLayerMask; //Set to mousePlane on Placeable
    private LayerMask raycastLayerMask;

    [SerializeField] private Transform raycastDebugVisual;
    [SerializeField] private Vector3 offset; //usado para contrarrestar el offset del aim
    private Camera playerCamera;
    private Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);


    private void Awake()
    {
        playerCamera = Camera.main;
        raycastLayerMask = PlaceableItemRaycastLayerMask;
    }

    /// <summary>
    /// Set the Raycast layer mask according to Interactable Item equipped
    /// Default is Weapon. Set to true if placeable item is equipped
    /// </summary>
    /// <param name="isPlaceableItemEquipped"></param>
    public void SetRaycastLayerMask(bool isPlaceableItemEquipped)
    {
        if (isPlaceableItemEquipped)
        {
            raycastLayerMask = PlaceableItemRaycastLayerMask;
        }
        else
        {
            raycastLayerMask = WeaponRaycastLayerMask;
        }
    }

    private void Update()
    {
        Raycast();
        //CharacterAimRotation(); //Not set to an instance Error.
    }

    int oldGrid = 0;

    //TODO FIXME Es muy poco performatico para distintos items interactuables. Mejor repetir código y tener distintos raycasts para cada item.
    //pero tendria que refactorizar
    //el PlayerManager para tener dos raycast scripts y decirle cual usar segun el equipped world item,
    //o el GBS para funcionar con el raycast de ItemPlaceable.
    private void Raycast()
    {
        Ray ray = playerCamera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, raycastLayerMask))
        {
            raycastDebugVisual.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;

            if (raycastLayerMask == PlaceableItemRaycastLayerMask)
            {
                if (buildingGhost.visual != null)
                    buildingGhost.visual.gameObject.SetActive(true);

                int newGrid = raycastHit.collider.GetComponent<PlacementLog>().logGridIndex;

                if (newGrid != oldGrid)
                {
                    oldGrid = newGrid;
                    raycastHit.collider.GetComponent<PlacementLog>().GetActiveGrid();
                }
            }
        }
        else
        {
            if (raycastLayerMask == PlaceableItemRaycastLayerMask && buildingGhost.visual != null)
                buildingGhost.visual.gameObject.SetActive(false);
            else
                mouseWorldPosition = ray.GetPoint(10);
        }
    }

    public void CharacterAimRotation()
    {
        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = PlayerManager.Instance.PlayerTransform.position.y;
        Vector3 aimDirection = worldAimTarget.normalized; //(worldAimTarget - offset).normalized; //Usaba esto para corregir offset de la animación

        PlayerManager.Instance.PlayerTransform.forward = Vector3.Lerp(PlayerManager.Instance.PlayerTransform.forward, aimDirection, Time.deltaTime * 40f);
    }

    [SerializeField] BuildingGhost buildingGhost;



    //Raycast que funciona para ranged equipped world item
    //integrado al raycast normal con el feo if statement del placeable layerque es al pedo correr en cada cuadro del bow
    private void RaycastRanged()
    {
        Ray ray = playerCamera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, raycastLayerMask))
        {
            raycastDebugVisual.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        else
        {
            mouseWorldPosition = ray.GetPoint(10);
        }
    }

    //Raycast que funciona para placeable equipped world item
    //integrado al raycast normal con el feo if statement del placeable layer que es al pedo correr en cada cuadro del bow
    private void RaycastPlaceable()
    {
        Ray ray = playerCamera.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, PlaceableItemRaycastLayerMask))
        {
            buildingGhost.visual.gameObject.SetActive(true);

            raycastDebugVisual.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;

            int newGrid = raycastHit.collider.GetComponent<PlacementLog>().logGridIndex;

            if (newGrid != oldGrid)
            {
                oldGrid = newGrid;
                raycastHit.collider.GetComponent<PlacementLog>().GetActiveGrid();
            }

        }
        else
        {
            buildingGhost.visual.gameObject.SetActive(false);
            mouseWorldPosition = ray.GetPoint(10);
        }
    }
}