using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryUI : MonoBehaviour
{
    public UnityAction OnOpen;
    public UnityAction OnClose;

    [Header("Common Section")]
    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Text thanksText;

    [SerializeField]
    GameObject drawingIcoPrefab;

    

    [Header("Crafting Section")]
    [SerializeField]
    GameObject inventoryForCrafting;

    [SerializeField]
    GameObject craftingPanel;

    [SerializeField]
    Transform recipeContent;

    [SerializeField]
    GameObject recipePrefab;

    //[Header("Filter Section")]
    //[SerializeField]
    //InputField recipeSearchField;

    [Header("Storage Section")]
    [SerializeField]
    GameObject inventoryForStorage;

    [SerializeField]
    GameObject storagePanel;

    [Header("Audio Section")]
    [SerializeField]
    AudioClip clipOpen;

    [SerializeField]
    AudioClip clipClose;

    Inventory inventory;
    CraftingSystem craftingSystem;
    PlayerController playerController;
    Storage storage;

    GameObject panel;
    List<SlotUI> uiSlots;

    int sourceId = -1;
    int targetId = -1;
    bool multiple = false;
    GameObject drawingIco;
    bool raycastDisabled = false;
    public bool OnOffInputEnabled
    {
        get { return !raycastDisabled; }
        set { raycastDisabled = !value; }
    }

    bool keepSelection = false;

    bool isOpened = false;
    public bool IsOpended
    {
        get { return isOpened; }
        set { isOpened = value; }
    }

    bool craftingEnabled = false;
    public bool CraftingEnabled
    {
        get { return craftingEnabled; }
    }
    bool workbenchEnabled = false;

    int inventoryCapacity;
    bool recipeChanged = false;
    bool dropDisabled = false;

    GameObject panelStorage;

    AudioSource source;

    MenuManager menuManager;

    QuantitySelectorUI quantitySelectorUI;

    BuildingMaker buildingMaker;

    #region FILTERS
    string searchString;
    List<System.Type> filters;
    int filterGameFreeTime = 0;
    #endregion

    private void Awake()
    {
        MainManager mainManager = GameObject.FindObjectOfType<MainManager>();
        if (mainManager.IsScreenSaver || mainManager.SandboxMode)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        buildingMaker = GameObject.FindObjectOfType<BuildingMaker>();
        
        quantitySelectorUI =  GameObject.FindObjectOfType<QuantitySelectorUI>();
        menuManager = GameObject.FindObjectOfType<MenuManager>();
        inventory = GameObject.FindObjectOfType<Inventory>();
        craftingSystem = GameObject.FindObjectOfType<CraftingSystem>();
        storage = GameObject.FindObjectOfType<Storage>();
        source = transform.root.GetComponent<AudioSource>();

        playerController = GameObject.FindObjectOfType<PlayerController>();
        panel = transform.GetChild(0).gameObject;
        panelStorage = transform.GetChild(1).gameObject;
        //recipeSearchField.onValueChanged.AddListener(OnRecipeResearch);
        filters = new List<System.Type>();

        HideItemDescription();
        HideThanks();

        panel.SetActive(false);
        panelStorage.SetActive(false);
    }

    void Update()
    {
        if (quantitySelectorUI.IsActive)
            return;

        if (MainManager.Instance.IsLoading)
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isOpened)
            {
                if(!menuManager.IsOpened && !buildingMaker.IsEnabled)
                    Open(true, false); // Open and enable crafting system without workbench recipes
            }
            else
            {
                if(!raycastDisabled)
                    Close();
            }
        }


        //if (inventory.IsOpened && !raycastDisabled)
        if (isOpened && !raycastDisabled)
        {

            GraphicRaycaster gr = GetComponent<GraphicRaycaster>();
            PointerEventData pe = new PointerEventData(null);
            pe.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            gr.Raycast(pe, hits);

            if (hits.Count > 0)
            {
              
                if (sourceId < 0) // No item has been selected yet
                {
              
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                    {

                        if (hits.Exists(r => r.gameObject.GetComponent<SlotUI>())) // There is a slot in the hit list
                        {
                            
                            RaycastResult slotHit = hits.Find(r => r.gameObject.GetComponent<SlotUI>());
                            
                            SlotUI slotUI = slotHit.gameObject.GetComponent<SlotUI>();
                          
                            if (!slotUI.IsDisabled)
                            {
                                if (slotUI != null)
                                {
                                    PlayClick();

                          
                                    sourceId = GetSlotIndex(slotUI); // Get the source id ( can be from inventory, storage or crafting system )
                          
                                    if (GetSlotAtIndex(sourceId) != null)
                                    {
                                        if (Input.GetMouseButtonDown(1))
                                            multiple = true;
                                    }
                                    else
                                    {
                                        uiSlots[sourceId].GetComponentInChildren<ItemSprite>()?.Darken(false);
                                        sourceId = -1;
                                    }

                                }
                            }



                        }


                    }
                }
                else // We already selected some item
                {
                    if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                    {

                        //
                        // Try move the item if possible ( if the target slot is enabled )
                        //
                        if (hits.Exists(r => r.gameObject.GetComponent<SlotUI>()))
                        {
                            RaycastResult slotHit = hits.Find(r => r.gameObject.GetComponent<SlotUI>());

                            if (!slotHit.gameObject.GetComponent<SlotUI>().IsDisabled)
                            {

                                targetId = GetSlotIndex(slotHit.gameObject.GetComponent<SlotUI>());

                                Slot target = null, source = null;

                                // Is an item inventory slot or a crafting system resource slot ?
                                target = GetSlotAtIndex(targetId);
                                source = GetSlotAtIndex(sourceId);

                                //(slotHit.gameObject.GetComponent<SlotUI>().GetType() != typeof(CraftingSlotUI) || slotHit.gameObject.GetComponent<CraftingSlotUI>().CheckItem())
                                bool ok = true;
                                bool switchItems = false;
                                if (targetId == sourceId)
                                    ok = false;

                                if (craftingEnabled && targetId < inventoryCapacity)
                                {
                                    if (target != null && target.Item == source.Item && target.Amount >= source.Item.SlotMaxAmount)
                                        ok = false;

                                    if (target != null && target.Item != source.Item)
                                        switchItems = true;
                                    //if (target != null && (target.Item != source.Item || target.Amount >= source.Item.SlotMaxAmount))
                                    //    ok = false;
                                }

                                if (!craftingEnabled)
                                {
                                    if (target != null && target.Item != source.Item)
                                        switchItems = true;
                                }

                                if (craftingEnabled && targetId >= inventoryCapacity)
                                {
                                    Recipe recipe = craftingSystem.Recipe;

                                    if (craftingSystem.Recipe.Resources[targetId - inventoryCapacity].Item != source.Item)
                                    {
                                        ok = false;
                                    }
                                    else
                                    {
                                        if (target != null && target.Amount == craftingSystem.Recipe.Resources[targetId - inventoryCapacity].Amount)
                                            ok = false;

                                    }

                                    
                                }



                                //if ((targetId != sourceId) && 
                                //    (target == null || (target.Item == source.Item && source.Item.SlotMaxAmount > target.Amount)) 
                                //    )
                                if (ok)
                                {   // Moving selected item to another slot

                                    if (!multiple) // Move just one 
                                    {
                                        Item srcItem = source.Item;

                                        // Move item
                                        if (!switchItems)
                                        {
                                            RemoveItem(1, sourceId);
                                            AddItem(srcItem, 1, targetId);
                                        }
                                        else
                                        {
                                            Item sourceItem = source.Item;
                                            int sourceAmount = source.Amount;
                                            Item targetItem = target.Item;
                                            int targetAmount = target.Amount;
                                            RemoveItem(sourceAmount, sourceId);
                                            RemoveItem(targetAmount, targetId);
                                            AddItem(targetItem, targetAmount, sourceId);
                                            AddItem(sourceItem, sourceAmount, targetId);
                                        }

                                        // Update UI
                                        UpdateSlotsUI();

                                        // Deselect ui slot
                                        
                                        uiSlots[targetId].GetComponentInChildren<ItemSprite>()?.Darken(false);


                                    }
                                    else // Create slide
                                    {
                                        // Choose how many item components must be moved
                                        int max = 0;
                                        if (craftingEnabled && targetId >= inventoryCapacity)
                                        {
                                            max = Mathf.Min(source.Amount, craftingSystem.Recipe.Resources[targetId - inventoryCapacity].Amount - (target != null ? target.Amount : 0));
                                        }
                                        else
                                        {
                                            max = Mathf.Min(source.Amount, source.Item.SlotMaxAmount - (target != null ? target.Amount : 0));
                                        }

                                        // Open slider
                                        OpenQuantitySelector(max, source.Item.Icon, MoveItemCallback);
                                        
                                    }
                                }
                            }


                        }
                        else
                        {

                            //
                            // Drop the item
                            //
                            if (!dropDisabled && hits.Exists(r => r.gameObject.GetComponent<DropPanel>()))
                            {
                                DropPanel dropPanel = hits.Find(p => p.gameObject.GetComponent<DropPanel>() != null).gameObject.GetComponent<DropPanel>();

                                if (!multiple)
                                {
                                    // Drop just one item
                                    //dropPanel.DropItem(inventory.GetSlotAtIndex(sourceId).Item, 1);
                                    dropPanel.DropItem(GetSlotAtIndex(sourceId).Item, 1);

                                    // Remove the item
                                    RemoveItem(1, sourceId);

                                    // Update UI
                                    UpdateSlotsUI();

                                }
                                else
                                {
                                    // Multiple objects, open the slider
                                    //Slot source = inventory.GetSlotAtIndex(sourceId);
                                    Slot source = GetSlotAtIndex(sourceId);
                                    int max = source.Amount;

                                    OpenQuantitySelector(max, source.Item.Icon, DropItemCallback);

                                }
                            }

                        }


                        if (!keepSelection) // If the multiple choice slider opens we can't reset yet
                        {
                            Reset();

                        }

                    }
                }

            }


            if (sourceId < 0)
            {
                if (drawingIco != null) // Release the drawing ico
                {
                    GameObject.Destroy(drawingIco);
                    drawingIco = null;
                }

            }
            else
            {
                
                if (drawingIco == null) // Create the drawing ico
                {
                    drawingIco = GameObject.Instantiate(drawingIcoPrefab, transform, false);

                    if (sourceId < inventoryCapacity)
                    {
                        drawingIco.GetComponent<ItemSprite>().SetIco(inventory.GetSlotAtIndex(sourceId).Item.Icon);
                    }
                    else
                    {
                        if(craftingEnabled)
                            drawingIco.GetComponent<ItemSprite>().SetIco(craftingSystem.GetSlotAtIndex(sourceId - inventoryCapacity).Item.Icon);
                        else
                            drawingIco.GetComponent<ItemSprite>().SetIco(storage.GetSlotAtIndex(sourceId - inventoryCapacity).Item.Icon);

                       
                    }

                    uiSlots[sourceId].GetComponentInChildren<ItemSprite>().Darken(true);

                }

                drawingIco.transform.position = Input.mousePosition;

            }


            //
            // Check recipe list
            //
            if (craftingEnabled && recipeChanged)
            {
                recipeChanged = false;
                bool found = false;
                int count = recipeContent.GetComponentsInChildren<RecipeUI>().Length;
                for (int i = 0; i < count && !found; i++)
                {
                    RecipeUI rcp = recipeContent.GetChild(i).GetComponent<RecipeUI>();
                    if (rcp.GetComponent<Toggle>().isOn)
                    {
                        found = true; // A recipe has been selected
                        ClearCraftingSlots();
                        craftingSystem.SetRecipe(rcp.Recipe);
                    }
                }

                // Try to free UI slot
                //FreeAllRecipeSlots(true);



                if (!found) // No recipe selected
                {
                    ClearCraftingSlots();
                    craftingSystem.SetRecipe(null);
                }

                UpdateSlotsUI();
            }
        }

    }

    public void DropDisabled(bool value)
    {
        dropDisabled = value;
    }

    public void OpenFromStorage()
    {
        Open(false, false);
    }

    public void OpenWithCraftingSystemDisabled()
    {
        Open(false, false);
    }

    public void OpenWithCraftingSystemEnabled(bool workbenchEnabled)
    {
        Open(true, workbenchEnabled);
    }

    public void Close()
    {
       
        source.clip = clipClose;
        source.Play();

        isOpened = false;

        // Crafting system reset
        if (craftingEnabled)
        {
            ClearCraftingSlots();
            craftingSystem.ResetRecipe();
            filters.Clear();
            searchString = "";
            filterGameFreeTime = 0;
        }

        Reset();

        uiSlots.Clear();

        if (craftingEnabled)
            ClearRecipeContent();
        //recipeSearchField.text = "";

        panel.SetActive(false);
        panelStorage.SetActive(false);
        playerController.SetInputEnabled(true);


        OnClose?.Invoke();
    }


    #region CRAFTING
    public void ClearCraftingSlots()
    {
        
        List<Slot> craftingSlots = new List<Slot>(craftingSystem.Slots);
        
        int count = 1;
        foreach (Slot slot in craftingSlots)
        {
           
            if (slot != null)
            {
                inventory.AddItem(slot.Item, slot.Amount);
            }
            
            count++;
            
        }
        
        craftingSystem.ClearSlots();

        UpdateSlotsUI();
    }

    public void CraftSomethig()
    {
        if (craftingSystem.Recipe.Output.GetType() == typeof(Item) && inventory.NoRoomForItem(craftingSystem.Recipe.Output as Item))
        {
            ShowErrorMessage("No room for the item " + craftingSystem.Recipe.Output.name);
            return;
        }

        if (craftingSystem.Recipe.Output.GetType() == typeof(Item) && (craftingSystem.Recipe.Output as Item).IsUnique && inventory.ItemExists((craftingSystem.Recipe.Output as Item)))
        {
            ShowErrorMessage("You already have the " + craftingSystem.Recipe.Output.name);
            return;
        }

        //if(craftingSystem.Recipe.Output.GetType() == typeof(Building) && playerController.Equipped != ItemCollection.GetAssetByCode("ItemHammer"))
        //{
        //    ShowErrorMessage("You need to equip the hammer to build the " + craftingSystem.Recipe.Output.name);
        //    return;
        //}

        try
        {
            if(craftingSystem.Recipe.Output.GetType() == typeof(Item)) // Craft an item
            {


                Item item;
                int quantity;

                // Craft and add in the inventory
                craftingSystem.CraftItem(out item, out quantity);
                inventory.AddItem(item, quantity);

                // Update UI
                UpdateSlotsUI();
            }
            else
            {
                if(craftingSystem.Recipe.Output.GetType() == typeof(Building)) // Craft a building
                {
                    // Close inventory and try to put the building on the scene
                    craftingSystem.CraftBuilding(workbenchEnabled);
                    Close();
                }
            }
            
        }
        catch (CustomException ce)
        {
            ShowErrorMessage(ce.Message);
        }

    }
    #endregion



    #region SHOW_MESSAGES
    public void ShowErrorMessage(string errorMessage)
    {
        descriptionText.text = errorMessage;
        descriptionText.color = Color.red;
        descriptionText.transform.parent.gameObject.SetActive(true);
    }

    public void ShowItemDescription(string description)
    {
        descriptionText.text = description;
        descriptionText.color = Color.white;
        descriptionText.transform.parent.gameObject.SetActive(true);
    }

    public void ShowThanks(string thanks)
    {
        thanksText.text = thanks;
        thanksText.color = Color.white;
        thanksText.transform.parent.gameObject.SetActive(true);
    }

    public void HideItemDescription()
    {
        descriptionText.transform.parent.gameObject.SetActive(false);
        descriptionText.text = "";
    }

    public void HideThanks()
    {
        thanksText.transform.parent.gameObject.SetActive(false);
        thanksText.text = "";
    }
    #endregion


    #region RECIPE
    public void RecipeChanged()
    {
        recipeChanged = true;
    }

    public void FillRecipeContent()
    {
        ClearCraftingSlots();

        ClearRecipeContent();

        List<Recipe> recipes = new List<Recipe>(RecipeCollection.GetAssetAll()).FindAll(r => searchString == null || r.name.ToLower().Contains(searchString.ToLower()));
        foreach (Recipe recipe in recipes)
        {
            if (workbenchEnabled || !recipe.WorkbenchOnly)
            {
                // Apply filters
                if(filters.Count == 0 || !filters.Contains(recipe.Output.GetType()))
                {
                    if(filterGameFreeTime == 0 || (filterGameFreeTime == 1 && !recipe.Output.FreeTimeOnly) || (filterGameFreeTime == 2 && recipe.Output.FreeTimeOnly))
                    {
                        GameObject obj = GameObject.Instantiate(recipePrefab, recipeContent, false);
                        obj.GetComponent<RecipeUI>().Set(recipe);
                    }
                    
                }

                
            }


        }
    }

    


    public void FilterByName(string searchString)
    {
        this.searchString = searchString;
        FillRecipeContent();
    }

    public void FilterEnabled(System.Type filter)
    {
        if (!filters.Contains(filter))
        {
            filters.Add(filter);
            FillRecipeContent();
        }
            

    }

    public void FilterDisabled(System.Type filter)
    {
        int index = filters.FindIndex(f => f == filter);
        if (index >= 0)
        {
            filters.RemoveAt(index);
            FillRecipeContent();
        }
            
    }

    public void FilterGameFreeTime(int filter)
    {
        filterGameFreeTime = filter;
        FillRecipeContent();
    }

    public void PlayClick()
    {
        GetComponentInParent<MouseClick>().Play();
    }

    #endregion

    #region PRIVATE
    private int GetSlotIndex(SlotUI slotUI)
    {
        return uiSlots.IndexOf(slotUI);

    }

    private int AddItem(Item item, int quantity, int index)
    {
        if(index < inventoryCapacity)
            return inventory.AddItem(item, quantity, index);

        if (craftingEnabled)
            return craftingSystem.AddItem(item, quantity, index - inventoryCapacity);
        else
            return storage.AddItem(item, quantity, index - inventoryCapacity);

        
    }

    private void RemoveItem(int quantity, int index)
    {
        if (index < inventoryCapacity)
        {
            inventory.RemoveItem(quantity, index);
            return;
        }
            
        if (craftingEnabled)
            craftingSystem.RemoveItem(quantity, index - inventoryCapacity);
        else
            storage.RemoveItem(quantity, index - inventoryCapacity);

    }

    private void Reset()
    {
        if (sourceId >= 0)
            uiSlots[sourceId].GetComponentInChildren<ItemSprite>().Darken(false);

        if (targetId >= 0)
            uiSlots[targetId]?.GetComponentInChildren<ItemSprite>()?.Darken(false);

        if (drawingIco != null)
            GameObject.Destroy(drawingIco);

        sourceId = -1;
        targetId = -1;
        multiple = false;
        raycastDisabled = false;
        keepSelection = false;

        drawingIco = null;

    }

    private void Open(bool craftingEnabled, bool workbenchEnabled)
    {
        source.clip = clipOpen;
        source.Play();

        isOpened = true;
        recipeChanged = false;
        this.craftingEnabled = craftingEnabled;
        this.workbenchEnabled = workbenchEnabled;

        // Init slots
        if (craftingEnabled)
        {
       
            uiSlots = new List<SlotUI>(inventoryForCrafting.GetComponentsInChildren<SlotUI>());
       
            inventoryCapacity = uiSlots.Count;
       
            uiSlots.AddRange(craftingPanel.GetComponentsInChildren<SlotUI>());
       
            // Init recipe list
            FillRecipeContent();
       
        }
        else
        {
            if (!storage)
            {
                storage = GameObject.FindObjectOfType<Storage>();
                panelStorage = transform.GetChild(1).gameObject;
            }
            uiSlots = new List<SlotUI>(inventoryForStorage.GetComponentsInChildren<SlotUI>());
            inventoryCapacity = uiSlots.Count;
            uiSlots.AddRange(storagePanel.GetComponentsInChildren<SlotUI>());
        }

        playerController.SetInputEnabled(false);

        if (craftingEnabled)
            panel.SetActive(true);
        else
            panelStorage.SetActive(true);

        // Fill slots
        UpdateSlotsUI();

        

        OnOpen?.Invoke();
    }

    private void ClearRecipeContent()
    {
        int count = recipeContent.childCount;
        for (int i = 0; i < count; i++)
            DestroyImmediate(recipeContent.GetChild(0).gameObject);
    }

   

    private void UpdateSlotsUI()
    {
        for (int i = 0; i < inventory.NumberOfSlots; i++)
        {
            uiSlots[i].Set(inventory.GetSlotAtIndex(i));
        }

        int offset = inventory.NumberOfSlots;

        if (craftingEnabled)
        {
           
            for (int i = 0; i < craftingSystem.NumberOfSlots; i++)
            {
                
                if (craftingSystem.NumberOfRecipeSlots > i)
                {
                    uiSlots[offset + i].SetDisabled(false);
                    uiSlots[offset + i].Set(craftingSystem.GetSlotAtIndex(i));
                }
                else
                {
                   
                    uiSlots[offset + i].SetDisabled(true);
                }
            }
        }
        else // Crafting is disabled only when storage is opened
        {
            for(int i=0; i<storage.NumberOfSlots; i++)
            {
                uiSlots[offset + i].Set(storage.GetSlotAtIndex(i));
            }

        }
 
    }

    private void FreeAllRecipeSlots(bool moveInInventory)
    {
        if (craftingEnabled)
        {
            int offset = inventory.NumberOfSlots;

            for (int i = 0; i < craftingSystem.NumberOfSlots; i++)
            {
                if (uiSlots[offset + i] != null)
                {
                    if(moveInInventory)
                        inventory.AddItem(uiSlots[offset + i].Item, uiSlots[offset + i].Quantity);

                    craftingSystem.RemoveItem(uiSlots[offset + i].Quantity, i);
                    uiSlots[offset + i].Set(null);
                }

            }
        }
    }

    private void OpenQuantitySelector(int max, Sprite icon, UnityAction<int> callback)
    {
      
        //QuantitySelectorUI quantitySelectorUI = GameObject.FindObjectOfType<QuantitySelectorUI>();
        quantitySelectorUI.Show(icon, max, callback);

        raycastDisabled = true; // Avoid to pick items with the slider opened
       
        keepSelection = true;

    }

    private void MoveItemCallback(int count)
    {
        if(count > 0)
        {
            // Move the item
            Slot source = GetSlotAtIndex(sourceId);
            Slot target = GetSlotAtIndex(targetId);
            Item srcItem = source.Item;
            AddItem(srcItem, count, targetId);
            RemoveItem(count, sourceId);

            // Update UI
            UpdateSlotsUI(); // Reload UI

        }

        Reset();

    }

    private Slot GetSlotAtIndex(int index)
    {
        if (index < inventoryCapacity)
            return inventory.GetSlotAtIndex(index);
        
        if (craftingEnabled)
            return craftingSystem.GetSlotAtIndex(index - inventoryCapacity);
        else
            return storage.GetSlotAtIndex(index - inventoryCapacity);

       
    }

    private void DropItemCallback(int count)
    {
        if(count > 0)
        {
            // Drop 
            DropPanel dropPanel = GetComponentInChildren<DropPanel>();
            //dropPanel.DropItem(inventory.GetSlotAtIndex(sourceId).Item, count);
            dropPanel.DropItem(GetSlotAtIndex(sourceId).Item, count);
            RemoveItem(count, sourceId);

            // Update UI
            UpdateSlotsUI();

           
        }

        Reset();
    }

    #endregion
}
