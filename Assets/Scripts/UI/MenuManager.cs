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
    protected IList<GameObject> MenuList
    {
        get { return menuList.AsReadOnly(); }
    }

    GameObject menuDefault;
    protected GameObject MenuDefault
    {
        get { return menuDefault; }
        set { menuDefault = value; }
    }

    GameObject current;

    private PlayerController playerController;

    bool isOpened = false;
    public bool IsOpened
    {
        get { return isOpened; }
        protected set { isOpened = value; }
    }

    public GameObject Current
    {
        get { return current; }
    }

    MainManager mainManager;
    protected MainManager MainManager
    {
        get { return mainManager; }
        set { mainManager = value; }
    }
    
    InventoryUI inventoryUI;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
        mainManager = GameObject.FindObjectOfType<MainManager>();



        playerController = GameObject.FindObjectOfType<PlayerController>();

        if (mainManager.IsScreenSaver || mainManager.SandboxMode)
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
            if (!mainManager.SandboxMode)
            {
                if (isOpened)
                    Close();
                else
                    if (!inventoryUI.IsOpended)
                        Open();
            }
            
            
        }
    }

    public virtual void Close()
    {
        isOpened = false;
        HideAll();

        playerController.SetInputEnabled(true);

        if (OnActionClose != null)
            OnActionClose.Invoke();
    }

    public virtual void Open()
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

    
    protected void HideAll()
    {
        foreach (GameObject menu in menuList)
            menu.SetActive(false);
    } 


}
