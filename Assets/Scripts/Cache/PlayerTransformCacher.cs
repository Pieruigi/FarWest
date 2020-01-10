using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformCacher : TransformCacher
{
    
    protected override void HandleOnSave()
    {
        
        string data = SpawnableIndex.ToString();
        if (Position)
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            data += "," + CacheUtility.VectorToCacheString(pos);
        }
        if (Rotation)
        {
            data += "," + CacheUtility.VectorToCacheString(transform.eulerAngles);
        }
        if (Size)
        {
            data += "," + CacheUtility.VectorToCacheString(transform.localScale);
        }

        CacheManager.Instance.AddOrUpdate(gameObject.name, data);
    }

}
