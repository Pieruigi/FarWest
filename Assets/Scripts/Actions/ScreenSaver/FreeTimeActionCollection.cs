﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreeTimeAction
{

    [SerializeField]
    GameObject owner;
    public GameObject Owner
    {
        get { return owner; }
    }

    [SerializeField]
    Transform target;
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    [SerializeField]
    bool cameraCloseDisabled = false;
    public bool CameraCloseDisabled
    {
        get { return cameraCloseDisabled; }
    }

    [SerializeField]
    FreeTimeActionCameraController cameraController;
    public FreeTimeActionCameraController CameraController
    {
        get { return cameraController; }
    }

    [SerializeField]
    FreeTimeActionController freeTimeActionController;
    public FreeTimeActionController FreeTimeActionController
    {
        get { return freeTimeActionController; }
    }

    [SerializeField]
    int enterAnimationId = -1;
    public int EnterAnimationId
    {
        get { return enterAnimationId; }
    }

    [SerializeField]
    int exitAnimationId = -1;
    public int ExitAnimationId
    {
        get { return exitAnimationId; }
    }



    [SerializeField]
    List<int> loopAnimationIds = null;
    public IList<int> LoopAnimationIds
    {
        get { return loopAnimationIds.AsReadOnly(); }
    }


    [SerializeField]
    int minLoopCount;
    public int MinLoopCount
    {
        get { return minLoopCount; }
        //set { minLoopCount = value; }
    }

    [SerializeField]
    int maxLoopCount;
    public int MaxLoopCount
    {
        get { return maxLoopCount; }
        //set { maxLoopCount = value; }
    }


}

public class FreeTimeActionCollection: MonoBehaviour
{
    [SerializeField]
    List<FreeTimeAction> freeTimeActions;



    public IList FreeTimeActions
    {
        get { return freeTimeActions.AsReadOnly(); }
    }
}
