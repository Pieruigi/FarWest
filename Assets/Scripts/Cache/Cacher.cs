using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cacher : MonoBehaviour
{
    int spawnableIndex;
    public int SpawnableIndex
    {
        get { return spawnableIndex; }
    }

    protected abstract void HandleOnSave();
    protected abstract void Init(string data);

    protected virtual void Awake()
    {
        CacheManager.Instance.OnSave += HandleOnSave;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        string data = CacheManager.Instance.GetValue(name);
        if (data != null)
            Init(data);
    }

    protected void OnDestroy()
    {
        CacheManager.Instance.OnSave -= HandleOnSave;

        // Remove from cache
        CacheManager.Instance.Delete(gameObject.name);
    }

    public void InitCache(string cacheCode, int spawnableIndex)
    {
        name = cacheCode;
        this.spawnableIndex = spawnableIndex;
        //Debug.Log("InitCache:" + name + "," + spawnableIndex);
    }
}
