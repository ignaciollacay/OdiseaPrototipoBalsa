using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldItem : MonoBehaviour//, ISerializationCallbackReceiver
{
    public ItemSO itemSO;

//    public void OnAfterDeserialize()
//    {

//    }

//    //Makes it easier to set the mesh of the world item in Editor
//    public void OnBeforeSerialize()
//    {
//#if UNITY_EDITOR

//        if (transform.childCount == 0) //fix rari para que no genere infinitas copias. //Reescribo lo mismo del de abajo que funcionaba más legible
//        {
//            Instantiate(itemSO.worldItem, transform.position, transform.rotation, transform); //create world item
//            name = itemSO.name; //rename World Item
//        }

//        #region Obsolete. Alternative way to create 3D world item. Changes MeshFilter&Renderer. Doesnt work on prefabs
//        //Funciona bien, pero no cuando el item es más que un simple mesh, como el placeable object prefab
//        //GetComponentInChildren<MeshFilter>().mesh = itemSO.worldItem.GetComponent<MeshFilter>().sharedMesh;
//        //EditorUtility.SetDirty(GetComponentInChildren<MeshFilter>());
//        //GetComponentInChildren<MeshRenderer>().material = itemSO.worldItem.GetComponent<MeshRenderer>().sharedMaterial;
//        //EditorUtility.SetDirty(GetComponentInChildren<MeshRenderer>());
//        #endregion
//#endif
//    }
}
