using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenSaverActivateSelector : UIOnOffSelector
{
    [SerializeField]
    List<GameObject> children;

    MainManager mainManager;

    

    protected override void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        base.Awake();
    }

    public override void Commit()
    {
        if (mainManager.IsScreenSaver)
            return;

        if (GetCurrentOption())
            mainManager.EnableScreenSaver();
        else
            mainManager.DisableScreenSaver();

        CheckChildren();
    }

    protected override void Init()
    {
        if (mainManager.IsScreenSaver)
            return;

        bool ssEnabled = mainManager.IsScreenSaverEnabled();
        SetOption(ssEnabled);

        CheckChildren();
        
    }

    void CheckChildren()
    {
        foreach(GameObject child in children)
            child.GetComponent<SelectorEnableDisable>().SetEnable(GetCurrentOption());
        
    }
}
