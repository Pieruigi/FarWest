using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeltUI : MonoBehaviour
{
    List<BeltSlotUI> beltSlotList;

    Inventory inventory;

    int disableCount = 0;

    MenuManager menuManager;
       
    private void Awake()
    {
        MainManager mainManager = GameObject.FindObjectOfType<MainManager>();
        if (mainManager.IsScreenSaver || mainManager.SandboxMode)
        {
            Destroy(gameObject);
        }
        else
        {
            beltSlotList = new List<BeltSlotUI>(GetComponentsInChildren<BeltSlotUI>());
            inventory = GameObject.FindObjectOfType<Inventory>();
            inventory.OnBeltChanged += HandleOnBeltChanged;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        InventoryUI iUI = GameObject.FindObjectOfType<InventoryUI>();

        iUI.OnOpen += SetDisable;
        iUI.OnClose += SetEnable;

        BuildingMaker buildingMaker = GameObject.FindObjectOfType<BuildingMaker>();

        buildingMaker.OnEnabled += SetDisable;
        buildingMaker.OnDisabled += SetEnable;

        menuManager = GameObject.FindObjectOfType<MenuManager>();
        menuManager.OnActionOpen += SetDisable;
        menuManager.OnActionClose += SetEnable;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuManager.IsOpened)
        {
            if (gameObject.activeSelf)
                SetDisable();
        }

    }


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
