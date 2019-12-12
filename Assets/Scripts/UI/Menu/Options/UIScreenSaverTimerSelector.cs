using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenSaverTimerSelector : UISelector<int>
{
    MainManager mainManager;

    protected override void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        base.Awake();
    }

    public override void Commit()
    {
        mainManager.SetScreenSaverTimeOut(GetCurrentOption() * 60);
    }

    protected override void InitOptionList()
    {
        for(int i=1; i<1000; i++)
        {
            Options.Add(i);
        }
    }

    protected override void Init()
    {
        if (mainManager.IsScreenSaver)
            return;

        int timeOut = mainManager.GetScreenSaverTimeOut();
        Debug.Log("TimeOut from regedit:" + timeOut);
        
        SetOption(timeOut/60 - 1);


    }
}
