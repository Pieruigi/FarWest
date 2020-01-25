using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Image imageIcon;

    Recipe recipe;
    public Recipe Recipe
    {
        get { return recipe; }
    }

    Toggle toggle;

    InventoryUI inventoryUI;
    MainManager mainManager;
    SandboxUI sandboxUI;

    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();

        if (!mainManager.SandboxMode)
        {
            inventoryUI = GetComponentInParent<InventoryUI>();
            //toggle = GetComponent<Toggle>();
            //toggle.group = transform.parent.GetComponent<ToggleGroup>();
            //toggle.onValueChanged.AddListener(OnValueChanged);
        }
        else
        {
            sandboxUI = GetComponentInParent<SandboxUI>();

        }

        toggle = GetComponent<Toggle>();
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Recipe recipe)
    {
        this.recipe = recipe;
        imageIcon.sprite = recipe.Icon;
    }
    
    void OnValueChanged(bool value)
    {

        if (!mainManager.SandboxMode)
        {
            inventoryUI.PlayClick();
            inventoryUI.RecipeChanged();
        }
        else
        {
            if (value)
                sandboxUI.RecipeChanged();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //if (!mainManager.SandboxMode)
        //{
        //    if (!"".Equals(recipe.Description))
        //        inventoryUI.ShowItemDescription(recipe.Description);

        //    if (!"".Equals(recipe.Thanks))
        //        inventoryUI.ShowThanks(recipe.Thanks);
        //}
        //else
        //{
        //    if (!"".Equals(recipe.Description))
        //        sandboxUI.ShowItemDescription(recipe.Description);

        //    if (!"".Equals(recipe.Thanks))
        //        sandboxUI.ShowThanks(recipe.Thanks);
        //}

        if (recipe.Description != null && !"".Equals(recipe.Description))
            SendMessageUpwards("ShowItemDescription", recipe.AssetName + " - " + recipe.Description, SendMessageOptions.DontRequireReceiver);

        if (recipe.Thanks != null && !"".Equals(recipe.Thanks))
            SendMessageUpwards("ShowThanks", recipe.Thanks, SendMessageOptions.DontRequireReceiver);
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //if (!mainManager.SandboxMode)
        //{
        //    inventoryUI.HideItemDescription();
        //    inventoryUI.HideThanks();
        //}
        SendMessageUpwards("HideItemDescription", SendMessageOptions.DontRequireReceiver);
        SendMessageUpwards("HideThanks", SendMessageOptions.DontRequireReceiver);
    }
}
