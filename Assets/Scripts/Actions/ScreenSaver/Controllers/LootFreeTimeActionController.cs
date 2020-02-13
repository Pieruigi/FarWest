using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFreeTimeActionController : BaseFreeTimeActionController
{
    [SerializeField]
    GameObject toolPrefab;

    [SerializeField]
    bool maskHand = false;

    PlayerScreenSaver player;
    Transform handR;

    GameObject tool;
    Animator anim;

    void Awake()
    {
        if (!MainManager.Instance.IsScreenSaver)
            Destroy(gameObject);
    }

    protected override void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScreenSaver>();
        handR = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        anim = player.GetComponent<Animator>();
        base.Start();
    }

    public override void ActionMessage(string message)
    {
        if ("TakeTool".Equals(message))
        {
            Debug.Log("TakeTool...");
            TakeTool();
        }

        base.ActionMessage(message);
    }

    //public override void ActionLoopStart(FreeTimeAction action, int loopId)
    //{
    //    base.ActionLoopStart(action, loopId);

    //}

    public override void ActionExitCompleted(FreeTimeAction action)
    {
        if (tool)
        {
            Utility.ObjectPopOut(tool);
            tool = null;
            if(maskHand)
                anim.SetBool("EquipTool", false);
        }

        FreeTimeLootManager ft = action.Owner.GetComponentInParent<FreeTimeLootManager>();
        ft.CurrentLootAction.LootCurrent = 0;
        ft.CurrentLootAction.OnExhausted?.Invoke(ft.CurrentLootAction);

        CacheManager.Instance.Save();

        base.ActionExitCompleted(action);
    }

    //void TakeTool()
    //{
    //    int type = (int)player.CurrentAction.Owner.GetComponentInParent<FreeTimeLootManager>().LootableType;

    //    switch ((LootableType)type)
    //    {
    //        case LootableType.Tree:
    //            TakeAxe();
    //            break;
    //    }
    //}

    void TakeTool()
    {
        if (tool != null)
            return;

        if(maskHand)
            anim.SetBool("EquipTool", true);

        tool = Utility.ObjectPopIn(toolPrefab, handR);
        tool.transform.localPosition = Vector3.zero;
        tool.transform.localEulerAngles = Vector3.zero;
    }

}
