using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    public UnityAction OnActionOpen;
    public UnityAction OnActionClose;
    
    [SerializeField]
    List<GameObject> menuList;

    GameObject menuDefault;

    GameObject current;

    private PlayerController playerController;

    bool isOpened = false;
    public bool IsOpened
    {
        get { return isOpened; }
    }

    MainManager mainManager;

    InventoryUI inventoryUI;

    private void Awake()
    {
      
        
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        mainManager = GameObject.FindObjectOfType<MainManager>();

        playerController = GameObject.FindObjectOfType<PlayerController>();

        if (GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            menuDefault = menuList[0]; // Set some screensaver menu
            Close();
        }
        else
        {
            menuDefault = menuList[0];
            Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainManager.IsScreenSaver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (isOpened)
                Close();
            else
                if(!inventoryUI.IsOpended)
                    Open();
            
            
        }
    }

    public void Close()
    {
        isOpened = false;
        HideAll();
        playerController.SetInputEnabled(true);

        if (OnActionClose != null)
            OnActionClose.Invoke();
    }

    public void Open()
    {
        playerController.SetInputEnabled(false);
        HideAll();
        isOpened = true;
        Open(menuDefault);
        
        if (OnActionOpen != null)
            OnActionOpen.Invoke();
    }

    public void Open(GameObject menu)
    {
        if(!isOpened)
        {
            Debug.LogWarning("The menu is closed; call open() first.");
            return;
        }

        if (current != null)
            current.SetActive(false);

        current = menu;
        current.SetActive(true);
    }

    
    void HideAll()
    {
        foreach (GameObject menu in menuList)
            menu.SetActive(false);
    } 
}
