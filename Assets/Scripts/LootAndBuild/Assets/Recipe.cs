using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Recipe: Asset
{
    //[SerializeField]
    //string code;

    [SerializeField]
    bool workbenchOnly;
    public bool WorkbenchOnly
    {
        get { return workbenchOnly; }
    }

    [SerializeField]
    Asset output;
    public Asset Output
    {
        get { return output; }
    }

    //[SerializeField]
    //int quantity;
      

    [SerializeField]
    List<Slot> resources;
    public IList<Slot> Resources
    {
        get { return resources.AsReadOnly(); }
    }



}
