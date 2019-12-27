using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInventory : MonoBehaviour
{
    MenuManager menuManager;
    InventoryUI inventoryUI;
    PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {
        if (menuManager.IsOpened)
            return;

        playerController.SetInputEnabled(true);
        inventoryUI.OpenWithCraftingSystemEnabled(false);
    }
}
