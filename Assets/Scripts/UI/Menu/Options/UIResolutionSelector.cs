
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResolutionSelector : UISelector<Resolution>
{
    string formatStr = "{0}x{1}";

    protected override void Init()
    {
        int id = Options.FindIndex(r => r.width == Screen.width && r.height == Screen.height);
        if (id >= 0)
        {
            SetOption(id);
        }
            
    }

    public override void Commit()
    {
        Resolution opt = GetCurrentOption();
        Screen.SetResolution(opt.width, opt.height, Screen.fullScreen, 0);
    }

    protected override void InitOptionList()
    {
        foreach (Resolution res in Screen.resolutions)
        {
            if(Options.FindIndex(r=>r.width == res.width && r.height == res.height) < 0)
            {
                Resolution newRes = new Resolution();
                newRes.width = res.width;
                newRes.height = res.height;
                Options.Add(res);
            }
                
        }
    }

    protected override string FormatOptionString(Resolution option)
    {
        return string.Format(formatStr, option.width, option.height);
    }

}
