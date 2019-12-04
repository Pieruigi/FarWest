using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraTypeSelector : UISelector<int>
{
    [SerializeField]
    ScreenSaverCameraManager screenSaverCameraManager;

    List<Camera> cameras;

    // Start is called before the first frame update
    protected override void Awake()
    {
        cameras = new List<Camera>(screenSaverCameraManager.Cameras);
        base.Awake();
    }

    public override void Commit()
    {
        
    }

    protected override void InitOptionList()
    {
        Options.Add(-1);
        
        for (int i = 0; i < cameras.Count; i++)
            Options.Add(i);
    }


    protected override string FormatOptionString(int option)
    {
        if (option < 0)
            return "All";
        else
            return cameras[option].name;
    }
}
