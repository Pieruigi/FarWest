using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerUIHelper : MonoBehaviour
{
    [SerializeField]
    GameObject cursorUI;

    bool isEnabled = true;

    GameObject belt;
    GameObject clock;
    GameObject health;

    // Start is called before the first frame update
    void Start()
    {
        belt = GameObject.FindObjectOfType<BeltUI>().gameObject;
        clock = GameObject.FindObjectOfType<TimeUITest>().gameObject;
        health = GameObject.FindObjectOfType<Health>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            isEnabled = !isEnabled;

            Cursor.visible = isEnabled;
            belt.SetActive(isEnabled);
            clock.SetActive(isEnabled);
            health.SetActive(isEnabled);
            cursorUI.SetActive(isEnabled);
        }    
    }
}
