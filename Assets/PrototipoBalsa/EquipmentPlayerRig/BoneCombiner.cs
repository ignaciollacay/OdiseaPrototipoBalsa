using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner
{
    public readonly Dictionary<int, Transform> rigBoneDictionary = new Dictionary<int, Transform>(); //holds the rig bone's as int
    private readonly Transform[] rigBoneTransforms = new Transform[69]; //holds the rigs bone's transforms to be added to the dictionary
    private readonly Transform playerTransform;

    public BoneCombiner(GameObject rootObj)
    {
        playerTransform = rootObj.transform;
        TraverseHierarchy(playerTransform); //populate rigBoneDictionary of the item (with it's children)
    }

    public void CombineBones(GameObject equippedItem, List<string> boneNames)
    {
        SkinnedMeshRenderer equippedItemSkinnedMeshRenderer = equippedItem.GetComponentInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < boneNames.Count; i++)
        {
            rigBoneTransforms[i] = rigBoneDictionary[boneNames[i].GetHashCode()]; //use name of bone to use as key to find reference of bone in player, stored in the array to apply them to the skinnedMeshRenderer 
        }
        //add bone structure in the object's renderer
        equippedItemSkinnedMeshRenderer.bones = rigBoneTransforms;
    }

    Transform SetBones(SkinnedMeshRenderer equippedItemSkinnedMeshRenderer, List<string> boneNames)
    {
        //assemble bone structure
        for (int i = 0; i < boneNames.Count; i++)
        {
            rigBoneTransforms[i] = rigBoneDictionary[boneNames[i].GetHashCode()]; //use name of bone to use as key to find reference of bone in player, stored in the array to apply them to the skinnedMeshRenderer 
        }
        //add bone structure in the object's renderer
        equippedItemSkinnedMeshRenderer.bones = rigBoneTransforms;

        //return the object with bone structure
        return equippedItemSkinnedMeshRenderer.transform;
    }

    private void TraverseHierarchy(Transform transform)
    {
        foreach (Transform child in transform)
        {
            rigBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }

    
}
