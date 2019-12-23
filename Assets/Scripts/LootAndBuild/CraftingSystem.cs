using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/**
 * This class only takes care of the crafting itself; for recipes filtering please refers to the UI
 * */
public class CraftingSystem : MonoBehaviour
{
    public UnityAction<Recipe> OnRecipeChanged;
    public UnityAction OnSlotListChanged; // True no resource has been loaded, otherwise false
    
    Recipe currentRecipe;
    public Recipe Recipe
    {
        get { return currentRecipe; }
    }

    public bool HasRecipe
    {
        get { return currentRecipe ? true : false; }
    }

    //Inventory inventory;

    List<Slot> slots; // They are slots for crafting
    public IList<Slot> Slots
    {
        get { return slots.AsReadOnly(); }
    }

    int numberOfSlots = 5;
    public int NumberOfSlots
    {
        get { return numberOfSlots; }
    }

    public int NumberOfRecipeSlots
    {
        get { return currentRecipe ? currentRecipe.Resources.Count : 0; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //inventory = GameObject.FindObjectOfType<Inventory>();
        slots = new List<Slot>(numberOfSlots);
        for (int i = 0; i < numberOfSlots; i++)
            slots.Add(null);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool SlotListIsEmpty()
    {
        if (slots == null)
            return true;

        for(int i=0; i<slots.Count; i++)
        {
            if (slots[i] != null)
                return false;
        }

        return true;
    }

    public void SetRecipe(Recipe recipe)
    {
        this.currentRecipe = recipe;


        OnRecipeChanged?.Invoke(recipe);
        
    }

    public void ResetRecipe()
    {
        currentRecipe = null;

        OnRecipeChanged?.Invoke(null);
    }

    public int AddItem(Item item, int quantity, int slotId)
    {
        
        if (slots == null)
            throw new CustomException(ErrorCode.GenericError, "The crafting slot array is not initializated.");

        if (slotId >= slots.Count)
            return 0;

        if (item != Recipe.Resources[slotId].Item)
            return 0;
        
        if(slots[slotId] != null && slots[slotId].Item != item)
        {
            return 0;
        }


        int q = 0;
        q = Mathf.Min(quantity, Recipe.Resources[slotId].Amount);
            
        if (slots[slotId] == null)
        {
            slots[slotId] = new Slot(item, q);
        }
        else
        {
            if(q + slots[slotId].Amount > Recipe.Resources[slotId].Amount)
                q = Recipe.Resources[slotId].Amount - slots[slotId].Amount;
            
            slots[slotId].Amount += q;
        }

        OnSlotListChanged?.Invoke();
       
        return q;
    }

    public void RemoveItem(int quantity, int slotId)
    {
        if (slots[slotId] == null)
            throw new CustomException(ErrorCode.GenericError, "No item found in the selected slot.");

        if (slots[slotId].Amount < quantity)
            throw new CustomException(ErrorCode.GenericError, "Not enough item quantity in the current slot.");

        slots[slotId].Amount -= quantity;
        if (slots[slotId].Amount == 0)
        {
            slots[slotId] = null;
        }

        OnSlotListChanged?.Invoke();
    }

    public void CraftItem(out Item item, out int quantity)
    {
        Recipe recipe = currentRecipe;

        ClearSlots();

        // Set output
        item = (Item)recipe.Output;
        quantity = 1;

    }

    public void CraftBuilding(bool workbenchEnabled)
    {
       
        BuildingMaker.Init(currentRecipe);

        ClearSlots();

        BuildingMaker.WorkbenchEnabled = workbenchEnabled;
        BuildingMaker.SetEnable(true);

    }

    public Slot GetSlotAtIndex(int index)
    {
        return slots[index];
    }

    public void ClearSlots()
    {
        bool found = false;

        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i] != null)
            {
                found = true;
                RemoveItem(slots[i].Amount, i);
            }
                
        }

        if(found)
            OnSlotListChanged?.Invoke();
    }

    public bool CheckForResources()
    {
        Recipe recipe = currentRecipe;
        List<Slot> resources = new List<Slot>(recipe.Resources);
        if (resources == null)
        {
            throw new CustomException(ErrorCode.GenericError, "Recipe - Brick list non instantiated.");
        }

        foreach (Slot resource in resources)
        {
            if (!CheckForResource(resource.Item, resource.Amount))
                return false;
        }

        return true;
    }


    /**
     * Checks if the item you are asking for.
     * */
    private bool CheckForResource(Item item, int quantity)
    {
        int count = 0;
        for (int i=0; i<currentRecipe.Resources.Count; i++)
        {
            if (slots[i] != null && slots[i].Item == item)
                count += slots[i].Amount;

        }

        if (count != quantity)
            return false;

        return true;
    }
    
    

}
