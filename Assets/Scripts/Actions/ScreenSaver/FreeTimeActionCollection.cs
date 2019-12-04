using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreeTimeAction
{
 
    //[System.Serializable]
    //public class StepData
    //{
    //    //[System.Serializable]
    //    //public class PrefabData
    //    //{
    //    //    [SerializeField]
    //    //    GameObject prefab;
    //    //    public GameObject Prefab
    //    //    {
    //    //        get { return prefab; }
    //    //    }

    //    //    [SerializeField]
    //    //    bool playerIsParent;
    //    //    public bool PlayerIsParent
    //    //    {
    //    //        get { return playerIsParent; }
    //    //    }

    //    //    [SerializeField]
    //    //    string playerNodeName;
    //    //    public string PlayerNodeName
    //    //    {
    //    //        get { return playerNodeName; }
    //    //    }

    //    //    [SerializeField]
    //    //    float startTime;
    //    //    public float StartTime
    //    //    {
    //    //        get { return startTime; }
    //    //    }

    //    //    [SerializeField]
    //    //    float stopTime;
    //    //    public float StopTime
    //    //    {
    //    //        get { return stopTime; }
    //    //    }
    //    //}

    //    [SerializeField]
    //    int animationId;
    //    public int AnimationId
    //    {
    //        get { return animationId; }
    //    }
            

    //    //[SerializeField]
    //    //List<PrefabData> prefabDataList;
    //    //public List<PrefabData> PrefabDataList
    //    //{
    //    //    get { return prefabDataList; }
    //    //}

        

    //}

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
    }

    [SerializeField]
    bool cameraCloseDisabled = false;
    public bool CameraCloseDisabled
    {
        get { return cameraCloseDisabled; }
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

    //[SerializeField]
    //List<StepData> loopSteps = null;
    //public IList<StepData> LoopSteps
    //{
    //    get { return loopSteps.AsReadOnly(); }
    //}

    [SerializeField]
    int minLoopCount;
    public int MinLoopCount
    {
        get { return minLoopCount; }
    }

    [SerializeField]
    int maxLoopCount;
    public int MaxLoopCount
    {
        get { return maxLoopCount; }
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
