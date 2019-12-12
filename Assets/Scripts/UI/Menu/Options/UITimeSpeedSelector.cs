using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeSpeedSelector : UISelector<int>
{
    [Header("Time Speed Section")]
    [SerializeField]
    bool isScreensaver = false;

    [SerializeField]
    DayNightCycle dayNightCycle;

    MainManager mainManager;
    protected override void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        //dnc = GameObject.FindObjectOfType<DayNightCycle>();
        base.Awake();
    }

    public override void Commit()
    {
        if ((mainManager.IsScreenSaver && !isScreensaver) || (!mainManager.IsScreenSaver && isScreensaver))
            return;

        dayNightCycle.SpeedMultiplier = GetCurrentOption();

    }

    protected override void InitOptionList()
    {
        for(int i=0; i<5; i++)
        {
            Options.Add((int)Mathf.Pow(2, i));
        }
    }

    protected override string FormatOptionString(int option)
    {
        return "x" + option;
    }
}
