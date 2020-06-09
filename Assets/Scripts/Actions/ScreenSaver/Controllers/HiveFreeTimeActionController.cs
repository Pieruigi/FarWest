using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveFreeTimeActionController : BaseFreeTimeActionController
{
    
    AudioSource source;

    List<BeeController> bees;

    protected override void Start()
    {
        base.Start();
        source = GetComponent<AudioSource>();
        bees = new List<BeeController>(GetComponentsInChildren<BeeController>());
    }

    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("Attack".Equals(message))
        {
            foreach (BeeController bee in bees)
                bee.Attack();
        }
    }

}
