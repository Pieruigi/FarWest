using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    ParticleSystem psBuildingDust;

    bool useHammer;
    public bool UseHammer
    {
        get { return useHammer; }
        set { useHammer = value; }
    }

    int handAnimId = 36;
    int hammerAnimId = 37;

    UnityAction callback;

    GameObject hammer;

    Transform handR;

    private Vector3 particlePosition;
    public Vector3 ParticlePosition
    {
        set { particlePosition = value; }
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        handR = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCallback(UnityAction callback)
    {
        this.callback = callback;
    }

    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);

        if (useHammer)
            TakeHammer();

        StartCoroutine(PlayParticleSystem());
    }

    public override void ActionExitCompleted(FreeTimeAction action)
    {
        base.ActionExitCompleted(action);

        if (useHammer)
            DropHammer();

        callback?.Invoke();
    }

    void TakeHammer()
    {
        Item item = ItemCollection.GetAssetByCode("ItemHammer");
        //Debug.Log("hammer:" + hammer.name);

        hammer = Utility.ObjectPopIn(item.EquippmentPrefab, Vector3.zero, Vector3.zero, Vector3.one, handR);
        

    }

    void DropHammer()
    {
        Utility.ObjectPopOut(hammer);
        hammer = null;
    }

    IEnumerator PlayParticleSystem()
    {
        yield return new WaitForSeconds(1.5f);
        psBuildingDust.transform.parent.position = particlePosition;
        psBuildingDust.Play();
        
    }
}
