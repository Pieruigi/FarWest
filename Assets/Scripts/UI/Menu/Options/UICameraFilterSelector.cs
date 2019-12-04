using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraFilterSelector : UISelector<string>
{
    [SerializeField]
    CameraFilters cameraFilters;

    MainManager mainManager;

    string noFilterOptionName = "None";

    protected override void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        base.Awake();
    }

    public override void Commit()
    {
        cameraFilters.ActivateFilter(GetCurrentOption());
    }

    protected override void InitOptionList()
    {
        Options.Add(noFilterOptionName);
        List<string> filters = cameraFilters.GetFilterNameList();
        foreach (string filter in filters)
            Options.Add(filter);
    }

    private void OnDisable()
    {
        if (!mainManager.IsScreenSaver)
            cameraFilters.TestFilter(false);
    }

    private void OnEnable()
    {
        if (!mainManager.IsScreenSaver)
            cameraFilters.TestFilter(true);
    }
}
