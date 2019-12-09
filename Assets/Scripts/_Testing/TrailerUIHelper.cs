using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerUIHelper : MonoBehaviour
{
    bool isEnabled = true;

    GameObject belt;
    GameObject clock;


    // Start is called before the first frame update
    void Start()
    {
        belt = GameObject.FindObjectOfType<BeltUI>().gameObject;
        clock = GameObject.FindObjectOfType<TimeUITest>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAA");

            isEnabled = !isEnabled;

            Cursor.visible = isEnabled;
            belt.SetActive(isEnabled);
            clock.SetActive(isEnabled);

        }    
    }
}
