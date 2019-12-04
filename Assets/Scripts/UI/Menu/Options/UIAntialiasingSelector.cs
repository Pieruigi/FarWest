using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UIAntialiasingSelector : UISelector<int>
{
    AntialiasingSettings antialiasingSettings;

    protected override void Awake()
    {
        antialiasingSettings = GameObject.FindObjectOfType<AntialiasingSettings>();

        base.Awake();

        
    }

    public override void Commit()
    {
        switch (GetCurrentOption())
        {
            case 0:
                antialiasingSettings.Quality = (int)PostProcessLayer.Antialiasing.None;
                break;

            case 1:
                antialiasingSettings.Quality = (int)PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                break;

            case 2:
                antialiasingSettings.Quality = (int)PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                break;

            case 3:
                antialiasingSettings.Quality = (int)PostProcessLayer.Antialiasing.TemporalAntialiasing;
                break;
        }
    }

   
    protected override void InitOptionList()
    {
        Options.Add(0); // None 
        Options.Add(1); // Low
        Options.Add(2); // Medium
        Options.Add(3); // High
                
    }

    protected override string FormatOptionString(int option)
    {
        switch (option)
        {
            case 0:
                return "None";
                
            case 1:
                return "Low";
                
            case 2:
                return "Medium";
                
            case 3:
                return "High";

            default:
                return null;
                
        }
       
    }

}
