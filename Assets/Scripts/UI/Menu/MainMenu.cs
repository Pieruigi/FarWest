using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   
    MenuManager menuManager;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
        menuManager = GetComponentInParent<MenuManager>();
    }

    
    public void StartPlaying()
    {
        GameObject.FindObjectOfType<PlayerController>().SetInputEnabled(true);
        menuManager.Close();
    }

    
}
