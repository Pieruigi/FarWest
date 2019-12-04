using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        InventoryCreateItem();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InventoryCreateItem()
    {
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();

        // Add items 
        Item stone = Resources.Load<Item>("Items/Stone");
        Item rope = Resources.Load<Item>("Items/Rope");
        Item wood = Resources.Load<Item>("Items/Wood");

        inventory.AddItem(stone, 8);
        inventory.AddItem(rope, 8);
        inventory.AddItem(wood, 8);


        // Load recipe
        Recipe recipe = Resources.Load<Recipe>("Recipes/Hammer");

        CraftingSystem craftingSystem = GameObject.FindObjectOfType<CraftingSystem>();
        craftingSystem.SetRecipe(recipe);
        //craftingSystem.Craft();


    }
}
