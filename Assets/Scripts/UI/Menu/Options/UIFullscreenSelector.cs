
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFullscreenSelector : UIOnOffSelector
{

   
    protected override void Init()
    {
        int id = Options.FindIndex(o => o == Screen.fullScreen);

        if (id >= 0)
            SetOption(id);
    }

    public override void Commit()
    {
        Screen.fullScreen = GetCurrentOption();
    }


}
