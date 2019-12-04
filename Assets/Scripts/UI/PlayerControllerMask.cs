using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        playerController.SetInputEnabled(true);
    }
}
