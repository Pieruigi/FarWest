using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftingSlotUI : SlotUI
{
    [SerializeField]
    Image placeHoderIcon;
       
    int requiredQuantity;

    CraftingSystem craftingSystem;

    Vector2 placeHolderSize;

    int checkUnicode = 0x2714; // 0x2713: normal, 0x2714: bold

    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        craftingSystem = GameObject.FindObjectOfType<CraftingSystem>();
        craftingSystem.OnRecipeChanged += HandleOnRecipeChanged;

        placeHolderSize = placeHoderIcon.rectTransform.sizeDelta;
        placeHoderIcon.transform.gameObject.SetActive(false);
        SetDisabled(true);
    }

    public override void Set(Slot slot)
    {
        base.Set(slot);

        if (slot != null)
        {
            if(requiredQuantity > Quantity)
            {
                TextQuantity.color = Color.white;
                TextQuantity.text = (requiredQuantity - Quantity).ToString();
            }
            else
            {
                TextQuantity.color = Color.green;
                TextQuantity.text = ((char)checkUnicode).ToString();
            }

            TextQuantity.transform.parent.gameObject.SetActive(true);
            ItemIco.GetComponent<ItemSprite>().SetSize(placeHolderSize);
        }
        else
        {
            TextQuantity.color = Color.white;
            TextQuantity.transform.parent.gameObject.SetActive(true);
            TextQuantity.text = requiredQuantity.ToString();
            
        }
    }

    public override void SetDisabled(bool value)
    {
        base.SetDisabled(value);

        Image img = GetComponent<Image>();

        img.color = new Color32(0,0,0,0);
    }

    void HandleOnRecipeChanged(Recipe recipe)
    {
        if(recipe != null && Index < recipe.Resources.Count)
        {
            // Set placeholder
            Slot res = recipe.Resources[Index];
            placeHoderIcon.transform.gameObject.SetActive(true);
            //TextQuantity.transform.parent.gameObject.SetActive(true);
            placeHoderIcon.sprite = res.Item.Icon;
            requiredQuantity = res.Amount;
            //TextQuantity.text = requiredQuantity.ToString();
            

            SetDisabled(false);

        }
        else
        {
            placeHoderIcon.sprite = null;
            placeHoderIcon.transform.gameObject.SetActive(false);

            SetDisabled(true);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData){}

    public override void OnPointerExit(PointerEventData eventData){}
}
