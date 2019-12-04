using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Asset
{
    [SerializeField]
    GameObject sceneObject;
    public GameObject SceneObject
    {
        get { return sceneObject; }
    }

    [SerializeField]
    GameObject craftingHelper;
    public GameObject CraftingHelper
    {
        get { return craftingHelper; }
    }


}
