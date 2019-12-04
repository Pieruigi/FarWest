using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLootCacher : TransformCacher
{
    
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
        action.LootCurrent = (int)CacheUtility.CacheStringToFloat(splits[id]);
        if(action.LootCurrent == 0)
        {
            id++;
            action.GrowingElapsed = CacheUtility.CacheStringToFloat(splits[id]);
        }

        // Resize nav mesh object if exists
        UnityEngine.AI.NavMeshObstacle nmo = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        if (nmo != null)
        {
            if (nmo.shape == UnityEngine.AI.NavMeshObstacleShape.Capsule)
            {
                nmo.radius = nmo.radius * transform.localScale.x;
            }
            else
            {
                Vector3 size = nmo.size;
                size.x = transform.localScale.x;
                size.z = transform.localScale.z;
                nmo.size = size;
            }
        }


    }


    protected override void HandleOnSave()
    {
        base.HandleOnSave();

        // Store loot data
        SS.LootAction action = GetComponentInChildren<SS.LootAction>();
        string extData = CacheUtility.FloatToCacheString(action.LootCurrent);
        if(action.LootCurrent == 0)
            extData += "," + CacheUtility.FloatToCacheString(action.GrowingElapsed);

        string data = CacheManager.Instance.GetValue(name);
        data += "," + extData;
        CacheManager.Instance.AddOrUpdate(name, data);
    }
}
