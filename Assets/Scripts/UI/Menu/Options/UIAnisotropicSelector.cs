using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UIAnisotropicSelector : UIOnOffSelector
{
    QualitySettings qualitySettings;

    protected override void Awake()
    {
        //postProcessLayers = GameObject.FindObjectsOfType<PostProcessLayer>();

        base.Awake();

        
    }

    public override void Commit()
    {
        if (GetCurrentOption())
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        else
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
    }


}
