using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileCacheManager : CacheManager
{

    private static ProfileCacheManager instance;
    public new static ProfileCacheManager Instance
    {
        get { return instance; }
    }

    public new static void Create(string path)
    {
        if (instance != null)
            return;

        instance = new ProfileCacheManager(path);
    }

    private ProfileCacheManager(string path) : base(path) 
    {
        
    }
}
