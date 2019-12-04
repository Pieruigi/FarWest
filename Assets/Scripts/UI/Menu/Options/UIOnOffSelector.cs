using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIOnOffSelector : UISelector<bool>
{

    protected override void InitOptionList()
    {
        Options.Add(false);
        Options.Add(true);
    }

    protected override string FormatOptionString(bool option)
    {
        return option ? "Yes" : "No";
    }

    protected void SetOption(bool value)
    {
        if (!value)
            SetOption(0);
        else
            SetOption(1);
    }
}
