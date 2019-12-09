using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> menuList;

    
    GameObject menuDefault;

    GameObject current;

    private PlayerController playerController;

    bool isOpened = false;

    MainManager mainManager;

    private void Awake()
    {
      
        
    }

    // Start is called before the first frame update
    void Start()
    {
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
                Open();
            
            
        }
    }

    public void Close()
    {
        isOpened = false;
        HideAll();
        playerController.SetInputEnabled(true);
    }

    public void Open()
    {
        playerController.SetInputEnabled(false);
        HideAll();
        Open(menuDefault);
        isOpened = true;
    }

    public void Open(GameObject menu)
    {
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
