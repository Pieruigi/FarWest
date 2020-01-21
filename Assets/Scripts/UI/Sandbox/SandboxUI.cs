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

    bool buildMode = false;

    MainManager mainManager;
    BuildingMaker buildingMaker;

    List<Recipe> recipes;
    //SpawnManager spawnManager;
    //List<GameObject> recipeSlotList;

    ScrollRect recipeView;

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

        recipes = new List<Recipe>(RecipeCollection.GetAssetAll()).FindAll(r=>r.Output.GetType().Equals(typeof(Building)));
        Debug.Log("Recipes.count:" + recipes.Count);
        recipeView = content.GetComponentInParent<ScrollRect>(); 
        // spawnManager = GameObject.FindObjectOfType<SpawnManager>();

        //recipeSlotList = new List<GameObject>();
        //foreach(Recipe recipe in recipes)
        //{
        //    if (!SpawnManager.PrefabHasAlreadySpawned((recipe.Output as Building).SceneObject))
        //    {
        //        GameObject obj = Instantiate(recipePrefab, content);
        //        obj.GetComponent<RecipeUI>().Set(recipe);
        //        obj.GetComponent<Toggle>().isOn = false;
        //        obj.GetComponent<Toggle>().interactable = false;
        //        //recipeSlotList.Add(obj);
        //    }

        //}
        FillRecipeSlots(false);
        recipeView.gameObject.SetActive(false);

        
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        buildingMaker.SetEnable(true);
        recipeView.gameObject.SetActive(true);

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
        buildingMaker.SetEnable(false);
        

        // Disable all the recipes
        Toggle[] rList = content.GetComponentsInChildren<Toggle>();
        foreach (Toggle r in rList)
        {
            r.isOn = false;
            r.interactable = false;
        }


        recipeView.gameObject.SetActive(false);
    }

    public void RecipeChanged()
    {
        Recipe sel = GetSelectedRecipe();
        buildingMaker.Init(sel);
        buildingMaker.SetHelper();
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
                //recipeSlotList.Add(obj);
            }

        }
    }

    

    void HandleOnBuildingMakerDone(Recipe recipe)
    {
        FillRecipeSlots(true);
    }

    
}
