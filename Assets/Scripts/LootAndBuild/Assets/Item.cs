using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Items are objects that you can store in your inventory; they can be resources like stones or tools like a hammer.
 * */
 
public class Item : Asset
{
    //[Header("General")]
    //[SerializeField]
    //private string code;
    ////public string Code
    ////{
    ////    get { return code; }
    ////}

    //[SerializeField]
    //private string itemName;

    //[SerializeField]
    //private string description;

    [Header("Quantity")]
    [SerializeField]
    protected int slotMaxAmount = 1; // How many instance of this item each slot can hold?
    public int SlotMaxAmount
    {
        get { return slotMaxAmount; }
    }

    [SerializeField]
    protected bool noMultipleSlots; // Can I use more than one slot to store this item? Use slotMaxAmount=1 AND noMultipleSlot=true for tools.

    [Header("Equippment")]
    [SerializeField]
    bool canBeEquipped;
    public bool CanBeEquipped
    {
        get { return canBeEquipped; }
    }

    [SerializeField]
    GameObject equippmentPrefab;
    public GameObject EquippmentPrefab
    {
        get { return equippmentPrefab; }
    }
    //[Header("Graphics")]
    //[SerializeField]
    //private Sprite icon;
    //public Sprite Icon
    //{
    //    get { return icon; }
    //}

    //[SerializeField]
    //private GameObject sceneAsset;

    //[Header("Bricks")]
    //[SerializeField]
    //private List<ItemData> bricks; // Does the item need other items to be created?

    public bool IsUnique
    {
        get { return (slotMaxAmount == 1 && noMultipleSlots); }
    }
}
