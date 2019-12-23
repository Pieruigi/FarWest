using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionUI : MonoBehaviour
{
    Text vText;

    // Start is called before the first frame update
    void Start()
    {
        vText = GetComponent<Text>();
        vText.text = "v." + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
