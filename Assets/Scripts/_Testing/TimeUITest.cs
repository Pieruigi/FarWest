using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUITest : MonoBehaviour
{
    DayNightCycle dnc;
    Text txt;
    
    // Start is called before the first frame update
    void Start()
    {
        dnc = GameObject.FindObjectOfType<DayNightCycle>();
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        //txt.text = string.Format("{0}:{1}:{2}", dnc.DayTimeInSeconds / 3600, (dnc.DayTimeInSeconds / 60)%60 , dnc.DayTimeInSeconds % 60);
        txt.text = string.Format("{0}:{1}", dnc.DayTimeInSeconds / 3600, (dnc.DayTimeInSeconds / 60) % 60);
    }
}
