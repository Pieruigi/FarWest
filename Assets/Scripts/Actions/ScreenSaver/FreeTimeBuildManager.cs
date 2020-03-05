using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FreeTimeBuildManager : MonoBehaviour
{
    public static readonly string PlayerCacheAI = "AI";
   

    [SerializeField]
    FreeTimeActionCollection actionCollection;

    float buildTimer; // If the last time something was built is older than builderTime then force to build ( or destroy )
    float buildTimerDefault;

#if UNITY_EDITOR
    float minLimit = 0;
#else
    float minLimit = 5;
#endif


    float maxLimit = 10;

    float actionRate;

    float buildRate = 1f; // The destroy rate is 1 - buildRate ( destroy rate is zero if there no building yet )
    
    SpawnManager spawnManager;

    List<GameObject> spawnedList = new List<GameObject>();

    PlayerScreenSaver playerSS;

    List<Recipe> recipeList;

    bool isBuildingOrDestroying = false;

    Recipe currentRecipe = null;

    BuildingHelper buildingHelper;

    GameObject toDestroy = null;
    
    private void Awake()
    {
        if (!MainManager.Instance.IsScreenSaver)
            Destroy(gameObject);

        actionCollection.gameObject.SetActive(false);

        string key = ProfileCacheManager.Instance.GetValue(PlayerCacheAI);
        if(key != null && !"".Equals(key.Trim()) && "0".Equals(key.Trim()))
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSS = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerScreenSaver>();
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();

        // Get all the building recipes
        recipeList = new List<Recipe>(RecipeCollection.GetAssetAll()).FindAll(r=>r.Output.GetType() == typeof(Building));
        //foreach (Recipe r in recipeList)
        //    Debug.Log("Recipe:" + r.AssetName);

        ComputeBuildingValues();
        //Debug.Log("BuildRate:" + buildRate);

        //foreach (GameObject g in spawnedList)
        //    Debug.Log("Prefab " + g.name + " already spawned");
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

            // Check whether we start acting or not
            if (UnityEngine.Random.Range(0f, 1f) > actionRate)
                return;
                

            // Started
            isBuildingOrDestroying = true;

            // Check whether we want to build or destroy
            bool destroy = false;

            float r = UnityEngine.Random.Range(0f, 1f);
            if (r > buildRate)
                destroy = true;

            if (destroy) // We choose to destroy
                DestroyRandomBuilding(); // Being here means there is something that can be destroyed for sure
            else // We choose to build
                CreateRandomBuilding();

            

        }
    }

    void CreateRandomBuilding()
    {

        // Check for any available recipe
        currentRecipe = ChooseBuildingToCreate();
        Debug.Log("I want to build - CurrentRecipe:" + currentRecipe);
        if (currentRecipe != null) // Found some recipe
        {
            StartCoroutine(SetPlaceWhereBuild());
        }
        else // No recipe has been found; for example we may have already built everything, in which case we simply skip out; having many available buildings 
             // should decrease the buildRate giving more chance the next action will destroy something
        {
            isBuildingOrDestroying = false;
        }
    }

    void DestroyRandomBuilding()
    {
        toDestroy = ChooseBuildingToDestroy();
        if(toDestroy == null)
        {
            isBuildingOrDestroying = false;
            return;
        }
        Debug.Log("Destroying building " + toDestroy.name + ":" + SpawnManager.GetSpawnable(toDestroy.GetComponent<Cacher>().SpawnableIndex));
        
        ForceNextFreeTimeActionOnDestroying(toDestroy);


    }

    void ForceNextFreeTimeActionOnDestroying(GameObject building)
    {
        
        FreeTimeAction action = actionCollection.FreeTimeActions[1] as FreeTimeAction;
        playerSS.ForceNextAction(action);
        action.Target = building.GetComponentInChildren<Destroyer>().Target;

        BuildFreeTimeActionController bc = GetComponent<BuildFreeTimeActionController>();
        bc.ParticlePosition = toDestroy.transform.position;
        bc.UseHammer = true;
        bc.SetCallback(DestroyingCallback);
    }

    void ForceNextFreeTimeActionOnBuilding()
    {
        BuildFreeTimeActionController bc = GetComponent<BuildFreeTimeActionController>();
        bc.ParticlePosition = buildingHelper.transform.position;
        bc.SetCallback(BuildingCallback);
        bc.UseHammer = false;
        int id = 0;
        if (currentRecipe.WorkbenchOnly)
        {
            bc.UseHammer = true;
            id = 1;
        }
            

        FreeTimeAction action = actionCollection.FreeTimeActions[id] as FreeTimeAction;
        playerSS.ForceNextAction(action);
        action.Target = buildingHelper.Target;
        
    }

    Recipe ChooseBuildingToCreate()
    {
        //Debug.Log("Choosing recipe to start building");
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
        //Debug.Log("************************************* START FINDING PLACE - "+currentRecipe+" *******************************");

        Vector3 position = Vector3.zero;
        Vector3 eulerAngles = Vector3.zero;

        bool found = false;
        int count = 0;

        // Create the building helper 
        buildingHelper = GameObject.Instantiate((currentRecipe.Output as Building).CraftingHelper).GetComponent<BuildingHelper>();
        buildingHelper.NoRaytracing = true;
        MeshRenderer[] mrl = buildingHelper.gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mrl.Length; i++)
            mrl[i].enabled = false;

        SkinnedMeshRenderer[] smrl = buildingHelper.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < smrl.Length; i++)
            smrl[i].enabled = false;

        buildingHelper.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;



        while (!found && count < 100)
        {
            Vector3 point = Utility.GetRandomPointOnNavMesh();
            point.y = 0;
            //Debug.Log("Checking Point:" + point);

            buildingHelper.transform.position = point;
            buildingHelper.transform.eulerAngles = Vector3.up * UnityEngine.Random.Range(0f, 360f);
            
            yield return null;
            yield return new WaitForEndOfFrame();

            if (buildingHelper.Allowed)
            {
                found = true;
                for (int i=0; i<10 && found; i++)
                {
                    found = buildingHelper.Allowed;
                    yield return null;
                }
                
                position = buildingHelper.transform.position;
                eulerAngles = buildingHelper.transform.eulerAngles;
                //Debug.Log("BuildingHelper.Allowed is true - point:" + point);
                //Debug.Log("BuildingHelper.Allowed is true - HelperPos:" + buildingHelper.transform.position);
                //Debug.Log("BuildingHelper.Allowed is true - HelperAng:" + buildingHelper.transform.eulerAngles);

            }
                
            count++;

            
        }

        //Debug.Log("FreeTimeBuilding - Count:" + count);
        //Debug.Log("FreeTimeBuilding - Found:" + found);

        if (!found)
        {
            isBuildingOrDestroying = false;
            Destroy(buildingHelper.gameObject);
            buildingHelper = null;
            //Debug.Log("No place has been found.");
            yield break;
        }
        else
        {
            //Debug.Log("Place has been found - Position:" + position);
            //Debug.Log("Place has been found - Angles:" + eulerAngles);
            OnSetPlaceWhereBuildCompleted(found, position, eulerAngles);
        }

        //Debug.Log("************************************* STOP FINDING PLACE *******************************");

    }


    void OnSetPlaceWhereBuildCompleted(bool succeed, Vector3 position, Vector3 eulerAngles)
    {
        //Debug.Log("I have a building position:" + position);

        if (!succeed)
        {
            isBuildingOrDestroying = false;
            return;
        }

     
        ForceNextFreeTimeActionOnBuilding();

        
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
        Vector3 position = buildingHelper.transform.position;
        Vector3 eulerAngles = buildingHelper.transform.eulerAngles;
        GameObject.Destroy(buildingHelper.gameObject);
        buildingHelper = null;

        GameObject building = SpawnManager.Spawn((currentRecipe.Output as Building).SceneObject);
        building.transform.position = position;
        building.transform.eulerAngles = eulerAngles;


        if (!building.GetComponentInChildren<FreeTimeStateManager>())
        {
            FreeTimeActionCollection actionCollection = building.GetComponentInChildren<FreeTimeActionCollection>();
            foreach (FreeTimeAction action in actionCollection.FreeTimeActions)
                playerSS.AddFreeTimeAction(action);
        }

        StartCoroutine(BuildingPopIn(building));

        currentRecipe = null;
        isBuildingOrDestroying = false;
        playerSS.ForceNextAction(null);

        GetComponent<BuildFreeTimeActionController>().SetCallback(null);

        ComputeBuildingValues();
    }

    IEnumerator BuildingPopIn(GameObject building)
    {
        building.transform.localScale = Vector3.zero;

        LeanTween.scale(building, Vector3.one, 1f).setEaseOutElastic();

        yield return new WaitForSeconds(1);

        UnityEngine.AI.NavMeshObstacle obs = building.GetComponent<UnityEngine.AI.NavMeshObstacle>();
        if (obs)
        {
            obs.enabled = false;
            yield return new WaitForSeconds(0.5f);
            obs.enabled = true;
            
        }

        
    }

    void DestroyingCallback()
    {
        isBuildingOrDestroying = false;
        playerSS.ForceNextAction(null);
        GetComponent<BuildFreeTimeActionController>().SetCallback(null);
       
        //SpawnManager.Unspawn(toDestroy);
        FreeTimeActionCollection actionCollection = toDestroy.GetComponentInChildren<FreeTimeActionCollection>();
        //foreach (FreeTimeAction action in actionCollection.FreeTimeActions)
        //    playerSS.RemoveFreetimeAction(action);
        if (actionCollection != null)
        {
            foreach (FreeTimeAction action in actionCollection.FreeTimeActions)
                playerSS.RemoveFreetimeAction(action);
        }
        else
        {
            foreach (FreeTimeAction action in toDestroy.GetComponentInChildren<FreeTimeStateManager>().Actions)
                playerSS.RemoveFreetimeAction(action);
        }

        StartCoroutine(BuildingPopOut());

        ComputeBuildingValues();
    }

    IEnumerator BuildingPopOut()
    {
        LeanTween.scale(toDestroy, Vector3.zero, 1f).setEaseInElastic();
        yield return new WaitForSeconds(1.1f);

        SpawnManager.Unspawn(toDestroy);
        toDestroy = null;
    }

    void ComputeBuildingValues()
    {

        SetSpawnedList();
        float baseTimer = 10;

        if(spawnedList.Count <= minLimit)
        {
            buildTimer = baseTimer + 5*spawnedList.Count; // Check every 30 seconds
            actionRate = 1 - spawnedList.Count/(2*minLimit); // Building or destroying rate
            buildRate = 1; // Only building
          
        }
        else 
        if(spawnedList.Count <= maxLimit) // Between minLimit and maxLimit
        {
            buildTimer = baseTimer + 10*spawnedList.Count;
            actionRate = 0.5f;
            float v = maxLimit - (maxLimit - minLimit) * 0.2f;
#if UNITY_EDITOR
            buildRate = 1; // Build rate range: 1 - 0.45
#else
            buildRate = 1 - (spawnedList.Count - minLimit) / v; // Build rate range: 1 - 0.45
#endif

        }
        else
        if(spawnedList.Count > maxLimit)
        {
            buildTimer = baseTimer + 15 * spawnedList.Count;
            actionRate = 0.5f;
            buildRate = 0.2f; 
           
        }
        buildTimerDefault = buildTimer;

      
    }

    void SetSpawnedList()
    {
        List<Destroyer> buildings = new List<Destroyer>(FindObjectsOfType<Destroyer>());

        spawnedList.Clear();

        // Fill the spawned list
        foreach (Destroyer d in buildings)
        {
            Cacher cacher = d.GetComponentInParent<Cacher>();

            GameObject prefab = SpawnManager.GetSpawnable(cacher.SpawnableIndex);
            if (SpawnManager.PrefabHasAlreadySpawned(prefab))
                spawnedList.Add(prefab);
        }
    }
}
