using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVideoOptionMenu : MonoBehaviour
{
    [SerializeField]
    Text txtResolution;

    List<string> resStringList;

    string resStringFormat = "{0}x{1}";

    // Start is called before the first frame update
    void Start()
    {
        resStringList = new List<string>();

        foreach (Resolution res in Screen.resolutions)
        {
            string tmp = string.Format(resStringFormat, res.width, res.height);
            if (!resStringList.Contains(tmp))
                resStringList.Add(tmp);
        }
            
    }

    private void OnEnable()
    {
        txtResolution.text = string.Format(resStringFormat, Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
