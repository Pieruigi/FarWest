using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : MonoBehaviour
{
    public UnityAction OnStorageUpdated;

    List<Slot> slots;

    int numberOfSlots = 48;
    public int NumberOfSlots
    {
        get { return numberOfSlots; }
    }

    private void Awake()
    {
        slots = new List<Slot>(numberOfSlots);
        for (int i = 0; i < numberOfSlots; i++)
            slots.Add(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        CacheManager.Instance.OnSave += HandleOnSave;

        string data = CacheManager.Instance.GetValue(Constants.CacheKeyStorage);
        if (data != null)
        {
            string[] splits = data.Split(',');
            int count = splits.Length / 3;
            for (int i = 0; i < count; i++)
            {
                int index = i * 3;
                int slotId = int.Parse(splits[index]);

                //Item item = Resources.Load<Item>(string.Format("Items/{0}", splits[i + 1]));
                Item item = ItemCollection.GetAssetByCode(splits[index + 1]);

                int amount = int.Parse(splits[index + 2]);
                AddItem(item, amount, slotId);
            }
        }
    }

    protected void OnDestroy()
    {
        CacheManager.Instance.OnSave -= HandleOnSave;

        // Remove from cache
        CacheManager.Instance.Delete(Constants.CacheKeyStorage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Slot GetSlotAtIndex(int index)
    {
        return slots[index];
    }

    /**
    * Return: the actual quantity added
    * */
    public int AddItem(Item item, int amount, int slotId)
    {
        // Check input
        if (item.IsUnique && amount > 1)
            throw new CustomException(ErrorCode.DuplicatedTool, "Can't add more than one tool of the same type.");
        if (item.IsUnique && slots.Exists(s => s != null && s.Item == item))
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

        OnStorageUpdated?.Invoke();

        return count;
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

        OnStorageUpdated?.Invoke();
    }

    public int GetNumberOfFreeSlots()
    {
        int count = 0;
        foreach(Slot slot in slots)
        {
            if (slot == null)
                count++;
        }

        return count;
    }

    void HandleOnSave()
    {
        string data = "";
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null || slots[i].Item == null)
                continue;

            string str = i + "," + slots[i].Item.Code + "," + slots[i].Amount; // SlotId,ItemCode,Amount
            if ("".Equals(data))
                data += str;
            else
                data += "," + str;
        }
        CacheManager.Instance.AddOrUpdate(Constants.CacheKeyStorage, data);
    }
}
