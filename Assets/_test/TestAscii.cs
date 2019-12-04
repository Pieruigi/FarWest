using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAscii : MonoBehaviour
{
    Text input;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<Text>();

        char c = (char)0x2713; 
        //0x2714 Grassetto
        input.text = "pippo" + c.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
