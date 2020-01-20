using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject itemIcoPrefab;

    [SerializeField]
    Text textQuantity;
    protected Text TextQuantity
    {
        get { return textQuantity; }
    }

    [SerializeField]
    GameObject background;

    [SerializeField]
    GameObject frame;

    GameObject itemIco;
    protected GameObject ItemIco
    {
        get { return itemIco; }
    }

    //Slot slot;
    string itemDescription = "";

    bool isDisabled = false;
    public bool IsDisabled
    {
        get { return isDisabled; }
        //set { isDisabled = value; }
    }

    InventoryUI inventoryUI;

    Item item;
    public Item Item
    {
        get { return item; }
    }

    int quantity;
    public int Quantity
    {
        get { return quantity; }
    }

    int index;
    protected int Index
    {
        get { return index; }
    }

    protected virtual void Awake()
    {
        textQuantity.transform.parent.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();

        // Get internal index
        bool found = false;
        int count = transform.parent.childCount;
        for(int i=0; i<count && !found; i++)
        {
            if(transform.parent.GetChild(i) == transform)
            {
                found = true;
                index = i;
            }
        }

        if (background)
        {
            if (GetComponentInParent<GridLayoutGroup>())
            {
                Vector2 cellSize = GetComponentInParent<GridLayoutGroup>().cellSize;

                (background.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x * 0.95f);
                (background.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * 0.95f);

                //itemIco.GetComponent<ItemSprite>().SetIco(slot.Item.Icon, new Vector2(cellSize.x * 0.9f, cellSize.y * 0.9f));
            }
        }

        if (frame)
        {
            if (GetComponentInParent<GridLayoutGroup>())
            {
                Vector2 cellSize = GetComponentInParent<GridLayoutGroup>().cellSize;

                (frame.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
                (frame.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);

                //itemIco.GetComponent<ItemSprite>().SetIco(slot.Item.Icon, new Vector2(cellSize.x * 0.9f, cellSize.y * 0.9f));
            }
        }
    }

    public virtual void Set(Slot slot)
    {
        
        if (slot != null)
        {
            item = slot.Item;
            quantity = slot.Amount;

            if (!itemIco) 
            {
                itemIco = GameObject.Instantiate(itemIcoPrefab, transform, false);
            }

            if (GetComponentInParent<GridLayoutGroup>())
            {
                Vector2 cellSize = GetComponentInParent<GridLayoutGroup>().cellSize;

                (itemIco.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x * 0.9f);
                (itemIco.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y * 0.9f);

                itemIco.GetComponent<ItemSprite>().SetIco(slot.Item.Icon, new Vector2(cellSize.x * 0.9f, cellSize.y*0.9f));
            }
            else
            {
                itemIco.GetComponent<ItemSprite>().SetIco(slot.Item.Icon);
            }
            
            textQuantity.transform.parent.gameObject.SetActive(true);
            textQuantity.text = slot.Amount.ToString();
            itemDescription = slot.Item.Description;
        }
        else
        {
            item = null;
            quantity = 0;

            GameObject.Destroy(itemIco);
            textQuantity.transform.parent.gameObject.SetActive(false);
            itemDescription = "";
        }
        

    }

    public virtual void SetDisabled(bool value)
    {
        isDisabled = value;

        Image img = GetComponent<Image>();

        Color c = img.color;

        if (isDisabled)
        {
            c.a = 0.5f;

        }
        else
        {
            c.a = 1;
        }

        img.color = c;

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(!"".Equals(itemDescription))
            inventoryUI.ShowItemDescription(itemDescription);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        inventoryUI.HideItemDescription();
    }


    
}
