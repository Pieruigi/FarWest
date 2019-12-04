using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SceneManager : MonoBehaviour
{

    //[SerializeField]
    //GameObject player;

    [Header("Spawn Section")]
    [SerializeField]
    int minimumNumberOfSpawners;

    [SerializeField]
    int maximumNumberOfSpawners;

    [SerializeField]
    List<GameObject> spawnGroupPrefabs;

    MainManager mainManager;


    // Start is called before the first frame update
    void Start()
    {
      
        mainManager = GameObject.FindObjectOfType<MainManager>();

        //GameObject.Instantiate(player);

        if(CacheManager.Instance.IsEmpty()) // New game
        {
            
            // Spawn resources
            RandomizeSpawners();

            // Save
            //CacheManager.Save();
        }

   
    }



    // Update is called once per frame
    void Update()
    {

    }


    public GameObject GetSpawnGroupPrefab(SpawnerType type)
    {
        return spawnGroupPrefabs[(int)type - 1];
    }

    /**
     * Used when a new game starts.
     * */
    void RandomizeSpawners()
    {
        // Get random spawners
        List<Spawner> spawners = new List<Spawner>(Spawner.GetSpawnersAll());// new List<Spawner>(GameObject.FindObjectsOfType<Spawner>());
        
        int count = Random.Range(minimumNumberOfSpawners, maximumNumberOfSpawners);

        for(int i=0; i<count; i++)
        {
            // Get a randow spawner
            int r = Random.Range(0, spawners.Count);
            Spawner spawner = spawners[r];
            spawners.Remove(spawner);

            // Get a random type
            SpawnerType type;
            type = (i < (int)Spawner.SpawnerTypeHigherIndex) ? (SpawnerType)(i + 1) : (SpawnerType)Random.Range(0, (int)Spawner.SpawnerTypeHigherIndex) + 1;

            
            // Activate the spawner with the given type
            spawner.Activate(type);
        }


    }


}
