﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    GameObject seconds;

    [SerializeField]
    GameObject minutes;

    [SerializeField]
    GameObject hours;

    DayNightCycle dnc;
    Text txt;
    
    // Start is called before the first frame update
    void Start()
    {
        dnc = GameObject.FindObjectOfType<DayNightCycle>();
        //txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float s = dnc.DayTimeInSeconds % 60;
        seconds.transform.localEulerAngles = -Vector3.forward * s * 6;

        float m = (dnc.DayTimeInSeconds / 60) % 60;
        minutes.transform.localEulerAngles = -Vector3.forward * m * 6;

        float h = dnc.DayTimeInSeconds / 3600;
        hours.transform.localEulerAngles = -Vector3.forward * h * 30;

        //txt.text = string.Format("{0}:{1}:{2}", dnc.DayTimeInSeconds / 3600, (dnc.DayTimeInSeconds / 60) % 60, dnc.DayTimeInSeconds % 60);
    }
}