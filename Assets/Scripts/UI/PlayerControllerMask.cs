using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        playerController.SetInputEnabled(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exit...");
        playerController.SetInputEnabled(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pointer click...");
        playerController.SetInputEnabled(true);
    }
}
