using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> spawnables;

    static SpawnManager instance;

    int count = 0;

    List<GameObject> spawnedList;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            spawnedList = new List<GameObject>();            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (!CacheManager.Instance.IsEmpty())
            SpawnFromCache();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GetSpawnable(int index)
    {
        return instance.spawnables[index];
    }

    public static bool PrefabHasAlreadySpawned(GameObject objectPrefab)
    {
        return instance.spawnedList.Contains(objectPrefab);
    }

    public static void Unspawn(GameObject obj)
    {
        // Get the obj cache code
        string code = obj.name;
        GameObject.Destroy(obj);

        // Remove the occurrence from the spawnwed object list
        int prefabIndex = obj.GetComponent<Cacher>().SpawnableIndex;
        GameObject prefab = instance.spawnables[prefabIndex];
        instance.spawnedList.Remove(prefab); // Remove the firts occurence
        
    }

    public static GameObject Spawn(GameObject spawnable)
    {
        int index = instance.spawnables.IndexOf(spawnable); 
        if (index < 0) // The object must be in the spawnable list in order to get spawned
            throw new System.Exception("Spawnable object can not be found in the spawnable list.");

        // Create the cache code
        //string cacheCode = string.Format("{0}_{1}", Constants.CacheKeySpawnable, instance.count);
        instance.count++;

        // Spawn the object
        GameObject gameObject = GameObject.Instantiate(instance.spawnables[index]);
        gameObject.GetComponent<Cacher>().InitCache(string.Format("{0}{1}", Constants.CacheKeySpawnable, instance.count.ToString()), index);

        // Store in the spawned list
        //if (!instance.spawnedList.Contains(instance.spawnables[index]))
        instance.spawnedList.Add(instance.spawnables[index]);

        return gameObject;
    }

    private void SpawnFromCache()
    {
        Dictionary<string, string>.KeyCollection keys = CacheManager.Instance.GetKeysAll();

        int max = 0;

        foreach(string key in keys)
        {
         
            if (key.StartsWith(Constants.CacheKeySpawnable))
            {

                int num = int.Parse(key.Substring(Constants.CacheKeySpawnable.Length));
                if (num > max)
                    max = num;

                string data = CacheManager.Instance.GetValue(key);
                int index = int.Parse(data.Split(',')[0]);
                GameObject gameObject = GameObject.Instantiate(instance.spawnables[index]);
                gameObject.GetComponent<Cacher>().InitCache(key, index);

                // Store in the spawned list
                //if (!instance.spawnedList.Contains(instance.spawnables[index]))
                instance.spawnedList.Add(instance.spawnables[index]);
            }
        }

        count = max;
        
    }
}
