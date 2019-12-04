using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Slot
{
    [SerializeField]
    Item item;
    public Item Item { get { return item; } }

    [SerializeField]
    int amount;
    public int Amount{
        get { return amount; }
        set { amount = value; }
    }

    public Slot() { }

    public Slot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }


    public override string ToString()
    {
        return string.Format("[Slot - item:{0}; count:{1}]", item, amount);
    }
}

public class Inventory: MonoBehaviour
{
    //public UnityAction OnOpen;
    //public UnityAction OnClose;

    public UnityAction OnBeltChanged;

    //[SerializeField]
    int numberOfSlots = 15;
    public int NumberOfSlots
    {
        get { return numberOfSlots; }
    }

    //[SerializeField]
    int numberOfQuickSlots = 5;
    public int NumberOfQuickSlots
    {
        get { return numberOfQuickSlots; }
    }
        

    //[SerializeField]
    List<Slot> slots;


    private void Awake()
    {
        //if (itemResources == null)
        //    LoadResources();

        if (numberOfQuickSlots > numberOfSlots)
            numberOfSlots = numberOfQuickSlots;

        // Init the slot list
        slots = new List<Slot>(numberOfSlots);
        for (int i = 0; i < numberOfSlots; i++)
        {
            slots.Add(null);
        }

       
    }

    private void Start()
    {
        CacheManager.Instance.OnSave += HandleOnSave;

        string data = CacheManager.Instance.GetValue(Constants.CacheKeyInventory);
        if(data != null)
        {
            string[] splits = data.Split(',');
            int count = splits.Length / 3;
            for (int i = 0; i < count; i++)
            {
                int index = i*3;
                int slotId = int.Parse(splits[index]);
                
                //Item item = Resources.Load<Item>(string.Format("Items/{0}", splits[i + 1]));
                Item item = ItemCollection.GetAssetByCode(splits[index+1]);
                
                int amount = int.Parse(splits[index+2]);
                AddItem(item, amount, slotId);
            }
        }
        
        
    }

 
    /**
     * Add a given quantity of an item into a given slot ( used when moving from bag to belt )
     * Return: the actual quantity added
     * */
    public int AddItem(Item item, int amount, int slotId)
    {
        // Check input
        if (item.IsUnique && amount > 1)
            throw new CustomException(ErrorCode.DuplicatedTool, "Can't add more than one tool of the same type.");
        if (item.IsUnique && slots.Exists(s => s!=null && s.Item == item))
            throw new CustomException(ErrorCode.DuplicatedTool, "Tool already exists.");
        if (slots[slotId] != null && slots[slotId].Item != item)
            throw new CustomException(ErrorCode.NotEmpty, "This slot is not empty.");


        // Get the current slot free space
        int freeSpace = item.SlotMaxAmount;
        if (slots[slotId] != null)
            freeSpace -= slots[slotId].Amount; // Left space


        // Add item or increase amount
        int count = 0; // Number of items actually added
        if (freeSpace > 0)
        {
            if (freeSpace >= amount)
                count = amount;
            else
                count = freeSpace;

            if (slots[slotId] == null)
                slots[slotId] = new Slot(item, count);
            else
                slots[slotId].Amount += count;
        }

        if (slotId < numberOfQuickSlots && count > 0)
            OnBeltChanged?.Invoke();

        return count;
    }

    /**
     * Add the given item to the inventory
     * Return: true if the item has been added
     * */
    public bool AddItem(Item item)
    {
        try
        {
            // Check input
            if (item.IsUnique && slots.Exists(s => s!=null && s.Item == item))
                throw new CustomException(ErrorCode.DuplicatedTool, "Tool already exists.");

            // Add
            return (AddItem(item, 1) == 1);
        }
        catch(CustomException)
        {
            throw;
        }
    }

    public int AddItem(Item item, int amount)
    {
        try
        { 
            // Check input
            if (item.IsUnique && amount > 1)
                throw new CustomException(ErrorCode.DuplicatedTool, "Can't add more than one tool of the same type.");

            if (item.IsUnique && slots.Exists(s => s!=null && s.Item == item))
                throw new CustomException(ErrorCode.DuplicatedTool, "Tool already exists.");
            

            // Loop through bag slots trying to add the right quantity
            int count = 0;
            for (int i = numberOfQuickSlots; i < slots.Count && amount > count; i++)
            {
                //Debug.Log(string.Format("Slot[{0}]:{1}", i, slots[i]));
                

                if ((slots[i] == null) || (slots[i].Item == item && slots[i].Amount < item.SlotMaxAmount))
                    count += AddItem(item, amount-count, i);

            }

            // Try to add remaining quantity to the belt
            if(count < amount)
            {
                for (int i = 0; i < numberOfQuickSlots && amount > count; i++)
                {
                    Debug.Log(string.Format("Slot[{0}]:{1}", i, slots[i]));


                    if ((slots[i] == null) || (slots[i].Item == item && slots[i].Amount < item.SlotMaxAmount))
                        count += AddItem(item, amount-count, i);

                }
            }

            return count;
        }
        catch (CustomException)
        {
            throw;
        }
            
    }

    public void RemoveItem(int quantity, int slotId)
    {
        if (slots[slotId] == null)
            throw new CustomException(ErrorCode.GenericError, "No item found in the selected slot.");

        if(slots[slotId].Amount < quantity)
            throw new CustomException(ErrorCode.GenericError, "Not enough item quantity in the current slot.");

        slots[slotId].Amount -= quantity;
        if(slots[slotId].Amount == 0)
        {
            slots[slotId] = null;
        }

        if (slotId < numberOfQuickSlots)
            OnBeltChanged?.Invoke();
        
    }

    public Slot GetSlotAtIndex(int index)
    {
        return slots[index];
    }

    public int GetTotalAmount(Item item)
    {
        int count = 0;
        foreach(Slot slot in slots)
        {
            if (item == slot.Item)
                count += slot.Amount;
        }

        return count;
    }

    public bool CheckItemQuantity(Item item, int neededQuantity)
    {
        foreach (Slot slot in slots)
        {
            if (slot != null && item == slot.Item)
                neededQuantity -= slot.Amount;

            if (neededQuantity <= 0)
                return true;
        }

        return false;
    }

    public bool NoRoomForItem(Item item)
    {
        if (item.IsUnique && ItemExists(item))
            return true;
        

        for(int i=0; i<numberOfSlots; i++)
        {
            if (slots[i] == null)
                return false;

            if (slots[i].Item == item && slots[i].Amount < item.SlotMaxAmount)
                return false;
        }

        return true;
    }

    public bool ItemExists(Item item)
    {
        return slots.Exists(s => s != null && s.Item == item);
    }

    void HandleOnSave()
    {
        string data = "";
        for(int i=0; i<slots.Count; i++)
        {
            if (slots[i] == null || slots[i].Item == null)
                continue;

            string str =  i +"," + slots[i].Item.Code + "," + slots[i].Amount; // SlotId,ItemCode,Amount
            if ("".Equals(data))
                data += str;
            else
                data += "," + str;
        }
        CacheManager.Instance.AddOrUpdate(Constants.CacheKeyInventory, data);
    }

    private void DebugInventory()
    {
        
        Debug.Log("---------------------------Inventory--------------------------");
        for(int i=0; i<slots.Count; i++)
            Debug.Log(string.Format("Slot[{0}]:{1}", i, slots[i]));
        Debug.Log("--------------------------------------------------------------");
    }
}
