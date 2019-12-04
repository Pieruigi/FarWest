using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCacher : Cacher
{
    [Header("Transform")]
    [SerializeField]
    bool position;
    protected bool Position
    {
        get { return position; }
    }
    [SerializeField]
    bool rotation;
    protected bool Rotation
    {
        get { return rotation; }
    }
    [SerializeField]
    bool size;
    protected bool Size
    {
        get { return size; }
    }


    protected override void Start()
    {
        string data = CacheManager.Instance.GetValue(name);

      
        if (data != null)
            Init(data);
    }


    protected override void Init(string data)
    {
        string[] splits = data.Split(',');
        int id = 1;
        if (position)
        {
            Vector3 pos;
            CacheUtility.CacheStringToVector(splits[id], out pos);
            transform.position = pos;
            id++;
        }

        if (rotation)
        {
            Vector3 angle;
            CacheUtility.CacheStringToVector(splits[id], out angle);
            transform.eulerAngles = angle;
            id++;
        }

        if (size)
        {
            Vector3 scale;
            CacheUtility.CacheStringToVector(splits[3], out scale);
            transform.localScale = scale;
        }
        
    }

    protected override void HandleOnSave()
    {
      
        string data = SpawnableIndex.ToString();
        if (position)
        {
            data += "," + CacheUtility.VectorToCacheString(transform.position);
        }
        if (rotation)
        {
            data += "," + CacheUtility.VectorToCacheString(transform.eulerAngles);
        }
        if (size)
        {
            data += "," + CacheUtility.VectorToCacheString(transform.localScale);
        }

        CacheManager.Instance.AddOrUpdate(gameObject.name, data);
    }
}
