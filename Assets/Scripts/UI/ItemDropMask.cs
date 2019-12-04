using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SendMessageUpwards("DropDisabled", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SendMessageUpwards("DropDisabled", false);
    }
}
