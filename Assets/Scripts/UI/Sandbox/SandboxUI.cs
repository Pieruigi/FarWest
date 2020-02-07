using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandboxUI : MonoBehaviour
{
    [SerializeField]
    GameObject recipePrefab;

    [SerializeField]
    Transform content;

    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Text thanksText;

    [SerializeField]
    Button btnBuildMode;

    bool buildMode = false;

    MainManager mainManager;
    BuildingMaker buildingMaker;

    List<Recipe> recipes;
    //SpawnManager spawnManager;
    //List<GameObject> recipeSlotList;

    ScrollRect recipeView;
    bool forceDescription = false;
    bool noDescription = false;

    bool destroyEnabled = false;
    bool destroying = false;

    private void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        if (!mainManager.SandboxMode)
            Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        buildingMaker = GameObject.FindObjectOfType<BuildingMaker>();
        buildingMaker.OnBuildingCreated += HandleOnBuildingMakerDone;
        buildingMaker.OnBuildingCancelled += HandleOnBuildingMakerCancelled;

        recipes = new List<Recipe>(RecipeCollection.GetAssetAll()).FindAll(r=>r.Output.GetType().Equals(typeof(Building)));
        
        recipeView = content.GetComponentInParent<ScrollRect>();

        // Hide description and thanks
        Utility.HideThanks(thanksText);
        Utility.HideItemDescription(descriptionText);

        FillRecipeSlots(false);
        ShowRecipes(false);

        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (buildMode)
        {
           
            RaycastHit hit;
            //bool noBuilding = true;
            Recipe onSceneRecipe = null;
            GameObject onSceneObject = null;
            if (RayCastFromMouse(out hit))
            {
                
                GameObject onScenePrefab = null;
                //onSceneObject = hit.collider.gameObject;
              
                Cacher cacher = hit.collider.gameObject.GetComponentInParent<Cacher>();
                if(cacher != null)
                {
                    int index = cacher.SpawnableIndex;
                    onScenePrefab = SpawnManager.GetSpawnable(index);
                    onSceneRecipe = GetRecipeByBuildingPrefab(onScenePrefab);
                    if (onSceneRecipe)
                    {
                   
                        onSceneObject = cacher.gameObject;
                        ShowItemDescription(onSceneRecipe.AssetName +" - " + onSceneRecipe.Description);
                        forceDescription = false;
                        //noBuilding = false;
                    }
                }
                //if(prefab != null)
                //{
                //    Debug.Log(prefab.gameObject.name);
                //    Recipe recipe = GetRecipeByBuildingPrefab(prefab);
                //    Debug.Log(recipe.name);


                //}
            }

            if (!onSceneRecipe && !forceDescription)
                HideItemDescription();

            if (destroyEnabled && !destroying)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (onSceneObject)
                    {
                       DestroyFromScene(onSceneObject);
                       
                    }
                }
                
            }
        }
        else // You exit the sandbox mode only if you are in 3D person view
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MessageBox.Show(MessageBox.Types.YesNo, "Quit sandbox mode?", ExitSandboxModeOk, null);
            }
        }
        
    }

    public void EnableDestroyMode(bool value)
    {
        destroyEnabled = value;
        ShowRecipes(!value);
        buildingMaker.BuildingCamera.GetComponent<BuildingCamera>().ZoomDisabled = !value;
        buildingMaker.ShowKeys(value);
    }

    public void SwitchBuildMode()
    {
        if (buildMode)
            CloseBuildMode();
        else
            OpenBuildMode();
    }

    public void OpenBuildMode()
    {
        if (buildMode)
            return;

        buildMode = true;
        btnBuildMode.gameObject.SetActive(false);
        buildingMaker.SetEnable(true);
        ShowRecipes(true);

        // Enable all the recipes
        Toggle[] rList = content.GetComponentsInChildren<Toggle>();
        foreach (Toggle r in rList)
            r.interactable = true;
    }

    public void CloseBuildMode()
    {
        if (!buildMode)
            return;

        buildMode = false;
        btnBuildMode.gameObject.SetActive(true);
        buildingMaker.SetEnable(false);
        

        // Disable all the recipes
        Toggle[] rList = content.GetComponentsInChildren<Toggle>();
        foreach (Toggle r in rList)
        {
            r.isOn = false;
            r.interactable = false;
        }


        ShowRecipes(false);
    }

    public void RecipeChanged()
    {
        Recipe sel = GetSelectedRecipe();
        buildingMaker.Init(sel);
        buildingMaker.SetHelper();
        buildingMaker.BuildingCamera.GetComponent<BuildingCamera>().ZoomDisabled = false;
        buildingMaker.ShowKeys(true);

        Utility.HideItemDescription(descriptionText);
        Utility.HideThanks(thanksText);
        ShowRecipes(false);
        noDescription = true;
    }

    Recipe GetSelectedRecipe()
    {
        Toggle[] tgs = content.GetComponentsInChildren<Toggle>();
        foreach(Toggle tg in tgs)
        {
            if (tg.isOn)
                return tg.GetComponent<RecipeUI>().Recipe;
        }

        return null;
    }

    void FillRecipeSlots(bool interactable)
    {
        // Clear slots
        RecipeUI[] list = content.GetComponentsInChildren<RecipeUI>();
        for (int i = 0; i < list.Length; i++)
            Destroy(list[i].gameObject);

        // Fill slots
        foreach (Recipe recipe in recipes)
        {
            if (!SpawnManager.PrefabHasAlreadySpawned((recipe.Output as Building).SceneObject))
            {
                GameObject obj = Instantiate(recipePrefab, content);
                obj.GetComponent<RecipeUI>().Set(recipe);
                obj.GetComponent<Toggle>().isOn = false;
                obj.GetComponent<Toggle>().interactable = interactable;
                
            }

        }
    }

    void HandleOnBuildingMakerDone(Recipe recipe)
    {
        FillRecipeSlots(true);
        ShowRecipes(true);
        noDescription = false;
    }

    void HandleOnBuildingMakerCancelled(Recipe recipe)
    {
        destroyEnabled = false;
        ShowRecipes(true);
        noDescription = false;
    }

    Recipe GetRecipeByBuildingPrefab(GameObject prefab)
    {

        List<Recipe> allRecipes = new List<Recipe>(RecipeCollection.GetAssetAll());
        foreach(Recipe recipe in allRecipes)
        {
            
            if (recipe.Output.GetType().Equals(typeof(Building)))
            {
                if ((recipe.Output as Building).SceneObject == prefab)
                    return recipe;
            }
            
        }

        return null;
    } 

    public void ShowRecipes(bool value)
    {
        RecipeDeselectAll();
        recipeView.gameObject.SetActive(value);

    }

    void RecipeDeselectAll()
    {
        Toggle[] tl = content.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in tl)
            t.isOn = false;
    }

    public void ShowItemDescription(string description)
    {
        if (noDescription)
            return;
        

        forceDescription = true;
        Utility.ShowItemDescription(descriptionText, description);
        
    }

    public void ShowThanks(string thanks)
    {
        if (noDescription)
            return;

        Utility.ShowThanks(thanksText, thanks);
    }

    public void HideItemDescription()
    {
        forceDescription = false;
        Utility.HideItemDescription(descriptionText);
    }

    public void HideThanks()
    {
        Utility.HideThanks(thanksText);
    }

    private bool RayCastFromMouse(out RaycastHit hitInfo)
    {
        Ray ray = buildingMaker.BuildingCamera.ScreenPointToRay(Input.mousePosition);

        //int mask = LayerMask.GetMask(new string[] { Constants.De });
        //mask = ~mask;

        if (Physics.Raycast(ray, out hitInfo, 10000))
        //if (Physics.Raycast(ray, out hitInfo, 10000))
        {
            return true;
        }

        return false;
    }

    private void DestroyFromScene(GameObject obj)
    {
        StartCoroutine(DoDestroyFromScene(obj));
    }

    private IEnumerator DoDestroyFromScene(GameObject obj)
    {
        Debug.Log("Destroy:" + obj.name);
        destroying = true;
        LeanTween.scale(obj, Vector3.zero, 1f).setEaseInElastic();
        yield return new WaitForSeconds(1.1f);

        SpawnManager.Unspawn(obj);
        destroying = false;

        FillRecipeSlots(true);
    }

    void ExitSandboxModeOk()
    {
        mainManager.ExitSandboxMode();
    }
}
