using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FreeTimeBuildManager : MonoBehaviour
{
    float buildTimer = 10; // If the last time something was built is older than builderTime then force to build ( or destroy )
    float buildTimerDefault;

    float buildRate = 1f; // The destroy rate is 1 - buildRate ( destroy rate is zero if there no building yet )
    float buildRateDefault;
    float buildRateMul = 0.05f;
    float buildRateMin = 0.5f;

    SpawnManager spawnManager;

    List<GameObject> spawnedList;

    PlayerScreenSaver playerSS;

    List<Recipe> recipeList;

    bool isBuildingOrDestroying = false;

    Recipe currentRecipe = null;

    private void Awake()
    {
        if (!MainManager.Instance.IsScreenSaver)
            Destroy(gameObject);

        buildTimerDefault = buildTimer;
        buildRateDefault = buildRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();

        // Get all the building recipes
        recipeList = new List<Recipe>(RecipeCollection.GetAssetAll()).FindAll(r=>r.Output.GetType() == typeof(Building));
        foreach (Recipe r in recipeList)
            Debug.Log("Recipe:" + r.AssetName);

        // All the buildings currently placed on the 3D world
        List<Destroyer> buildings = new List<Destroyer>(FindObjectsOfType<Destroyer>());

        spawnedList = new List<GameObject>();

        // Fill the spawned list
        foreach (Destroyer d in buildings)
        {
            Cacher cacher = d.GetComponentInParent<Cacher>();
            
            GameObject prefab = SpawnManager.GetSpawnable(cacher.SpawnableIndex);
            if (SpawnManager.PrefabHasAlreadySpawned(prefab))
                spawnedList.Add(prefab);
        }

        // Decrease the building rate for each new building place in the world
        buildRate = buildRateDefault - buildRateMul * spawnedList.Count;
        buildRate = Mathf.Max(buildRate, buildRateMin);
        Debug.Log("BuildRate:" + buildRate);

        foreach (GameObject g in spawnedList)
            Debug.Log("Prefab " + g.name + " already spawned");
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuildingOrDestroying)
            return;

        buildTimer -= Time.deltaTime;

        // If buildTimer reachs zero then force Chico to build ( or destroy ) something
        if(buildTimer < 0) 
        {
            buildTimer = buildTimerDefault;
            isBuildingOrDestroying = true;

            Debug.Log("Force to build or destroy something");

            bool destroy = false;
            float r = UnityEngine.Random.Range(0f, 1f);
            if (r > buildRate)
                destroy = true;

            destroy = true;
            Debug.Log("Destroy is " + destroy);
            
            if (destroy)
            {
                DestroyRandomBuilding(); // Destroy is true because buildRate is less than one, so I'm sure there is something that can be destroyed
            }
            else
            {
                
                currentRecipe = ChooseBuildingToCreate();
                Debug.Log("CurrentRecipe:" + currentRecipe);
                if(currentRecipe == null)
                {
                    DestroyRandomBuilding(); // Recipe null means all the building have already been placed
                }
                else
                {
                    StartCoroutine(SetPlaceWhereBuild());
                }
                
            }





        }
    }

    void DestroyRandomBuilding()
    {
        GameObject toDestroy = ChooseBuildingToDestroy();
        Debug.Log("Destroying building " + toDestroy.name);
        ForceNextFreeTimeActionToDestroy(toDestroy);
    }

    void ForceNextFreeTimeActionToDestroy(GameObject building)
    {
        Debug.Log("Destroying " + building);
        
    }

    Recipe ChooseBuildingToCreate()
    {
        Debug.Log("Choosing recipe to start building");
        if (spawnedList.Count == 0)
        {
            return recipeList[UnityEngine.Random.Range(0, recipeList.Count)];
        }

        
        List<Recipe> tmp = new List<Recipe>();
        foreach (Recipe r in recipeList)
        {
            if (!spawnedList.Exists(s => s == (r.Output as Building).SceneObject))
                tmp.Add(r);
        }

        if (tmp.Count == 0) // We already created all the buildings
            return null;

        return tmp[UnityEngine.Random.Range(0, tmp.Count)];
    }

    IEnumerator SetPlaceWhereBuild()
    {
        
        bool found = false;
        int count = 0;
        while(!found && count < 100)
        {
            count++;
            yield return null;
        }

        if (!found)
            isBuildingOrDestroying = false;
    }

    GameObject ChooseBuildingToDestroy()
    {
        Destroyer[] buildings = FindObjectsOfType<Destroyer>();
        if (buildings == null || buildings.Length == 0)
            return null;

        return buildings[UnityEngine.Random.Range(0, buildings.Length)].transform.root.gameObject;
    }


    void BuildingCallback()
    {
        currentRecipe = null;
        isBuildingOrDestroying = false;
    }

    void DestroyingCallback()
    {
        isBuildingOrDestroying = false;
    }
}
