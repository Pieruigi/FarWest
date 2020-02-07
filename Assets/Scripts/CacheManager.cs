
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System;

/// <summary>
/// Manages cache storing/loading game objects state on/from disk.
/// </summary>
public class CacheManager
{
  
    public UnityAction OnSave; // Every object that needs to save its state should call this delegate
    //public static UnityAction OnLoad;


    private static CacheManager instance;
    public static CacheManager Instance
    {
        get { return instance; }
    }
    //public string test = "AAAAAAA";
    //public string GetTest()
    //{
    //    return test;
    //}

    private Dictionary<string, string> cache = new Dictionary<string, string>();

    protected string cachePath = Application.persistentDataPath + "/sav.txt";

    public string CachePath
    {
        get { return cachePath; }
    }

    
    protected CacheManager(string path)
    {
        cachePath = path;
    }

#region PUBLIC
    public static void Create(string path)
    {
        if (instance != null)
            return;

        instance = new CacheManager(path);
       
    }

    public void AddOrUpdate(string key, string value)
    {
        if (!cache.ContainsKey(key))
            cache.Add(key, value);
        else
            cache[key] = value;
    }

    /**
     * Returns the value of the given key if exists, otherwise null
     * */
    public string GetValue(string key)
    {
        if (!cache.ContainsKey(key))
            return null;

        return cache[key];
    }

    public void Delete(string key)
    {
        if (cache.ContainsKey(key))
            cache.Remove(key);
    }

    public Dictionary<string, string>.KeyCollection GetKeysAll()
    {
        return cache.Keys;
    }

    public bool IsEmpty()
    {
        return cache.Keys.Count == 0;
    }

    /**
    * Store cache on disk
    * */

    public void Save()
    {
        if (OnSave != null)
            OnSave();

        File.WriteAllText(cachePath, CacheToString());

    }

    /**
     * Load data from disk
     * */
    public void Load()
    {
        try
        {
            string s = File.ReadAllText(cachePath);
            StringToCache(s);
        }
        catch (Exception)
        {

        }


        //if (OnLoad != null)
        //    OnLoad();
    }

    public void Clear()
    {
        
        cache.Clear();
        File.Delete(cachePath);
    }
#endregion

#region UTILITY
   

#endregion




#region PRIVATE
    private void StringToCache(string str)
    {
        string[] splits = str.Split(';');
        cache.Clear();
        foreach(string s in splits)
        {
            AddOrUpdate(s.Substring(0, s.IndexOf(":")), s.Substring(s.IndexOf(":") + 1));
        }
        
    }

    private string CacheToString()
    {
        string ret = "";

        Dictionary<string, string>.Enumerator cacheEnum = cache.GetEnumerator();

        while (cacheEnum.MoveNext())
        {
            ret += string.Format("{0}:{1};",cacheEnum.Current.Key, cacheEnum.Current.Value);
        }

        if(!"".Equals(ret))
            ret = ret.Substring(0, ret.LastIndexOf(";"));

      
        return ret;
    }


#endregion
}

public class CacheUtility
{
    public static string VectorToCacheString(Vector3 v)
    {
        //return string.Format("{0}_{1}_{2}", v.x.ToString().Replace(",", "."), v.y.ToString().Replace(",", "."), v.z.ToString().Replace(",", "."));
        return string.Format("{0}_{1}_{2}", Mathf.RoundToInt(v.x * 1000) , Mathf.RoundToInt(v.y * 1000), Mathf.RoundToInt(v.z * 1000));
    }

    public static string FloatToCacheString(float f)
    {
        //return f.ToString().Replace(",", ".");
        return Mathf.RoundToInt(f*1000).ToString();
    }

    public static void CacheStringToVector(string str, out Vector3 ret)
    {
        string[] splits = str.Split('_');
        if (splits.Length < 3)
            throw new Exception("Param error - str.Length < 3.");

        //ret = new Vector3(float.Parse(splits[0].Replace(".", ",")), float.Parse(splits[1].Replace(".", ",")), float.Parse(splits[2].Replace(".", ",")));
        ret = new Vector3(float.Parse(splits[0])/1000f, float.Parse(splits[1]) / 1000f, float.Parse(splits[2]) / 1000f);
    }

    public static float CacheStringToFloat(string str)
    {
        //return float.Parse(str.Replace(".", ","));
        return float.Parse(str) / 1000f;
    }

    public static void DebugCache(CacheManager cache)
    {
        Dictionary<string, string>.KeyCollection keys = cache.GetKeysAll();

        
        foreach (string key in keys)
        {
            Debug.Log("KEY:" + key);

            
        }

        
    }
}