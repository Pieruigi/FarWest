using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
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
}
