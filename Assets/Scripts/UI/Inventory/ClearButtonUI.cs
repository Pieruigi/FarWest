using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearButtonUI : MonoBehaviour
{
    CraftingSystem craftingSystem;

    // Start is called before the first frame update
    void Start()
    {
        craftingSystem = GameObject.FindObjectOfType<CraftingSystem>();
        craftingSystem.OnSlotListChanged += HandleOnSlotListChanged;

        GetComponent<Button>().interactable = !craftingSystem.SlotListIsEmpty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleOnSlotListChanged()
    {
       
        GetComponent<Button>().interactable = !craftingSystem.SlotListIsEmpty();
    }

}
