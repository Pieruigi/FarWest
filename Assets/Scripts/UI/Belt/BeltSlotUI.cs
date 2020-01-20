using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeltSlotUI : MonoBehaviour
{
    [SerializeField]
    Image icon;    

    Slot slot;

    bool noEvent = false;

    Toggle toggle;

    PlayerController playerController;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(HandleOnValueChanged);
        toggle.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSlot(Slot slot)
    {
        
        if (this.slot == slot)
            return;

        this.slot = slot;

        // Unequip
        playerController?.EquipItem(null);

        if (toggle.isOn)
        {
            noEvent = true;
            toggle.isOn = false;
            noEvent = false;
        }

        if(slot == null)
        {
            icon.sprite = null;
            icon.color = new Color32(255, 255, 255, 20); 
            toggle.interactable = false;
        }
        else
        {
            icon.sprite = slot.Item.Icon;
            icon.color = new Color32(255,255,255,255);
            if (slot.Item.CanBeEquipped)
            {
                toggle.interactable = true;
            }
        }
    }

    void HandleOnValueChanged(bool value)
    {
        GetComponentInParent<MouseClick>().Play();

        if (value)
        {
            playerController.EquipItem(slot.Item);
        }
        else
        {
            if(!toggle.group.AnyTogglesOn())
                playerController.EquipItem(null);
        }
    }
}
