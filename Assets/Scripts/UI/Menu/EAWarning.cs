using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAWarning : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    [SerializeField]
    GameObject menu;

    MenuManager menuMan;

    // Start is called before the first frame update
    void Start()
    {
        menuMan = GameObject.FindObjectOfType<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(menuMan.IsOpened && menuMan.Current == menu)
        {
            if (!panel.activeSelf)
                panel.SetActive(true);
        }
        else
        {
            if (panel.activeSelf)
                panel.SetActive(false);
        }
    }
}
