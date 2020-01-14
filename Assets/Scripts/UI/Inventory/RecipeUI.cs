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

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
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
        inventoryUI.PlayClick();
        inventoryUI.RecipeChanged();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!"".Equals(recipe.Description))
            inventoryUI.ShowItemDescription(recipe.Description);

        if (!"".Equals(recipe.Thanks))
            inventoryUI.ShowThanks(recipe.Thanks);

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.HideItemDescription();
        inventoryUI.HideThanks();
    }
}
