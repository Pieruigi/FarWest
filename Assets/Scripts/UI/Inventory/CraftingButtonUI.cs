using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingButtonUI : MonoBehaviour
{
    CraftingSystem craftingSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        craftingSystem = GameObject.FindObjectOfType<CraftingSystem>();
        craftingSystem.OnRecipeChanged += HandleOnRecipeChanged;
        craftingSystem.OnSlotListChanged += HandleOnSlotListChanged;
        GetComponent<Button>().interactable = craftingSystem.HasRecipe;
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleOnRecipeChanged(Recipe recipe)
    {
        GetComponent<Button>().interactable = false;
    }

    private void HandleOnSlotListChanged()
    {
        if (craftingSystem.Recipe.Output.GetType() != typeof(Building) || !SpawnManager.PrefabHasAlreadySpawned((craftingSystem.Recipe.Output as Building).SceneObject))
        {
            GetComponent<Button>().interactable = craftingSystem.CheckForResources();
        }
        
        
    }
}
