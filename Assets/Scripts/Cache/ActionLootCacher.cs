﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLootCacher : TransformCacher
{

    Vector3 sizeMul;
    float radiusMul;

    protected override void Awake()
    {
   
            UnityEngine.AI.NavMeshObstacle nmo = GetComponent<UnityEngine.AI.NavMeshObstacle>();
            if (nmo != null)
            {
                if (nmo.shape == UnityEngine.AI.NavMeshObstacleShape.Capsule)
                {
                    radiusMul = nmo.radius;
                }
                else
                {
                    sizeMul = nmo.size;
                }
            }

            base.Awake();
       

        
    }

    protected override void Init(string data)
    {
        base.Init(data);

        int id = 1;
        if (Position)
            id++;
        if (Rotation)
            id++;
        if (Size)
            id++;

        string[] splits = data.Split(',');
        SS.LootAction action = GetComponentInChildren<SS.LootAction>();
        //action.LootCurrent = (int)CacheUtility.CacheStringToFloat(splits[id]);
        action.LootCurrent = (int)int.Parse(splits[id]);
        
        if(action.LootCurrent == 0)
        {
            id++;
            //action.GrowingElapsed = CacheUtility.CacheStringToFloat(splits[id]);
            action.GrowingElapsed = float.Parse(splits[id]);
        }

        // Resize nav mesh object if exists
        UnityEngine.AI.NavMeshObstacle nmo = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        if (nmo != null)
        {
            if (nmo.shape == UnityEngine.AI.NavMeshObstacleShape.Capsule)
            {
                nmo.radius = nmo.radius * transform.localScale.x * radiusMul;
            }
            else
            {
                Vector3 size = nmo.size;
                size.x = transform.localScale.x * sizeMul.x;
                size.z = transform.localScale.z * sizeMul.z;

                nmo.size = size;
            }
        }


    }


    protected override void HandleOnSave()
    {
        //MainManager mainMan = GameObject.FindObjectOfType<MainManager>();
        //if (mainMan.SandboxMode)
        //    return;

        base.HandleOnSave();

        // Store loot data
        SS.LootAction action = GetComponentInChildren<SS.LootAction>();
     
        string extData = action.LootCurrent.ToString();
        
        if (action.LootCurrent == 0)
            extData += "," + Mathf.RoundToInt(action.GrowingElapsed);

        string data = CacheManager.Instance.GetValue(name);
        data += "," + extData;
        CacheManager.Instance.AddOrUpdate(name, data);
    }


}
