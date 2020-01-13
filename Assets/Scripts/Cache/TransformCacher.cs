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
            //pos = pos / 1000f;
            transform.position = pos;
            id++;
        }

        if (rotation)
        {
            Vector3 angle;
            CacheUtility.CacheStringToVector(splits[id], out angle);
            //angle = angle / 1000f;
            transform.eulerAngles = angle;
            id++;
        }

        if (size)
        {
            Vector3 scale;
            
            CacheUtility.CacheStringToVector(splits[3], out scale);
            //scale = scale / 1000f;
            transform.localScale = scale;
        }
        
    }

    protected override void HandleOnSave()
    {
      
        string data = SpawnableIndex.ToString();
        //Vector3 v;
        if (position)
        {
            //v = transform.position;
            //v.x = Mathf.RoundToInt(v.x * 1000);
            //v.y = Mathf.RoundToInt(v.y * 1000);
            //v.z = Mathf.RoundToInt(v.z * 1000);
            data += "," + CacheUtility.VectorToCacheString(transform.position);
            //data += "," + CacheUtility.VectorToCacheString(v);
        }
        if (rotation)
        {
            //v = transform.eulerAngles;
            //v.x = Mathf.RoundToInt(v.x * 1000);
            //v.y = Mathf.RoundToInt(v.y * 1000);
            //v.z = Mathf.RoundToInt(v.z * 1000);
            data += "," + CacheUtility.VectorToCacheString(transform.eulerAngles);
            //data += "," + CacheUtility.VectorToCacheString(v);
        }
        if (size)
        {
            //v = transform.localScale;
            //v.x = Mathf.RoundToInt(v.x * 1000);
            //v.y = Mathf.RoundToInt(v.y * 1000);
            //v.z = Mathf.RoundToInt(v.z * 1000);
            data += "," + CacheUtility.VectorToCacheString(transform.localScale);
            //data += "," + CacheUtility.VectorToCacheString(v);
        }

        CacheManager.Instance.AddOrUpdate(gameObject.name, data);
    }
}
