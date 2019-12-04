using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeltUI : MonoBehaviour
{
    List<BeltSlotUI> beltSlotList;

    Inventory inventory;

    int disableCount = 0;

    private void Awake()
    {
        if (GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("BeltUI awaking........");
            beltSlotList = new List<BeltSlotUI>(GetComponentsInChildren<BeltSlotUI>());
            inventory = GameObject.FindObjectOfType<Inventory>();
            inventory.OnBeltChanged += HandleOnBeltChanged;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //inventory = GameObject.FindObjectOfType<Inventory>();
        //inventory.OnBeltChanged += HandleOnBeltChanged;
        Debug.Log("Starting BeltUI........");

        InventoryUI iUI = GameObject.FindObjectOfType<InventoryUI>();

        iUI.OnOpen += SetDisable;
        iUI.OnClose += SetEnable;

        BuildingMaker.OnEnabled += SetDisable;
        BuildingMaker.OnDisabled += SetEnable;

        //string data = CacheManager.GetValue(Constants.CacheKeyInventory);
        //if(data != null)
        //{
        //    for(int i=0; i<5)
        //}

        //beltSlotList = new List<BeltSlotUI>(GetComponentsInChildren<BeltSlotUI>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void AddBeltSlotUI(BeltSlotUI slotUI)
    //{
    //    beltSlotList.Add(slotUI);
    //}

    void HandleOnBeltChanged()
    {
        for(int i=0; i<inventory.NumberOfQuickSlots; i++)
        {
            Slot slot = inventory.GetSlotAtIndex(i);
            beltSlotList[i].SetSlot(slot);
        }
    }

    void SetEnable()
    {
        disableCount--;
        if (disableCount < 0) disableCount = 0;
        if (disableCount == 0) gameObject.SetActive(true);
    }

    void SetDisable()
    {
        disableCount++;
        gameObject.SetActive(false);
    }
}
