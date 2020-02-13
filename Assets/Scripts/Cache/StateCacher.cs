using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCacher : TransformCacher
{
    [SerializeField]
    int state;
    public int State
    {
        get { return state; }
        set { state = value; }
    }

    protected override void Init(string data)
    {
        base.Init(data);

        string[] splits = data.Split(',');
        
        int id = GetDataLength(data);
        Debug.Log("S:" + splits[id]);
        int s;
        if (int.TryParse(splits[id], out s))
            state = s;
    }

    protected override void HandleOnSave()
    {
        Debug.Log("HandleOnSaveForStateCacher:"+gameObject.name);
        base.HandleOnSave();

        string data = CacheManager.Instance.GetValue(name);
        data += "," + state;

        CacheManager.Instance.AddOrUpdate(name, data);
    }
}
