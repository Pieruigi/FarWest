using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectLootFreeTimeActionController : LootFreeTimeActionController
{
   
    

    protected override void Start()
    {
        base.Start();
    }

    public override void ActionExitCompleted(FreeTimeAction action)
    {
        FreeTimeLootManager ft = action.Owner.GetComponentInParent<FreeTimeLootManager>();

        //List<GameObject> oList = new List<GameObject>((ft.CurrentLootAction as SS.DirectLootAction).ObjectList);

        foreach(GameObject obj in (ft.CurrentLootAction as SS.DirectLootAction).ObjectList)
        {
            if(obj.activeSelf)
                (ft.CurrentLootAction as SS.DirectLootAction).OnResourceTaken(ft.CurrentLootAction as SS.DirectLootAction, obj);
        }
            

        base.ActionExitCompleted(action);
    }


}
