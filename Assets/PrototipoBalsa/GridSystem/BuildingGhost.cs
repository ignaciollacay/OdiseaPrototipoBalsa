using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [Header("Position Offset")]
    [SerializeField] float y = 0;
    public Transform visual;
    private PlaceableSO placeableSOType;

    [SerializeField] Material ghostMaterial;
    private void Start()
    {
        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged (object sender, System.EventArgs e)
    {
        RefreshVisual();
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = y;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }

    public void RefreshVisual()
    {
        if(visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        placeableSOType = GridBuildingSystem.Instance.GetPlaceableSOType(); //toma el objeto equipado

        if (placeableSOType != null) //si tiene un objeto equipado
        {
            visual = Instantiate(placeableSOType.visual, Vector3.zero, placeableSOType.visual.rotation); //crea un nuevo objeto, que va a ser el ghost
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = placeableSOType.visual.rotation.eulerAngles;
            SetLayerRecursive(visual.gameObject, 7); //NOTE: Layer number must correspond with Ghost Layer in Editor!
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer) //pone en el ghost layer el objeto junto a sus hijos
    {
        targetGameObject.layer = layer;
        //Material[] materials = targetGameObject.transform.GetComponent<MeshRenderer>().materials;
        //for (int i = 0; i < materials.Length; i++)
        //{
        //    materials[i] = ghostMaterial;
        //}
        
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
        
    }
}
