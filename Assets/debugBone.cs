using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugBone : MonoBehaviour
{
    SkinnedMeshRenderer skin;
    // Start is called before the first frame update
    void Start()
    {
        skin = GetComponent<SkinnedMeshRenderer>();
        //foreach (Transform bone in skin.bones)
        //{
        //    Debug.Log(this.name + ": " + bone);
        //}
        Debug.Log("Bone Amount - " + skin.bones.Length +" of " + this.name,this.gameObject);

        foreach (Transform bone in skin.bones)
        {
            Debug.Log(bone + " of " + this.name, this.gameObject);
            Debug.Log(bone.name + " of " + this.name, this.gameObject);
        }
    }
}
