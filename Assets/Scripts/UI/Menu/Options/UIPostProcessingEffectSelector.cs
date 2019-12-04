using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

enum PostProcessingEffect { DepthOfField, Blur, Bloom, AmbientOcclusion }

public class UIPostProcessingEffectSelector : UIOnOffSelector
{
    [Header("DOF Section")]
    [SerializeField]
    PostProcessVolume ppv;

    [SerializeField]
    PostProcessingEffect postProcessingEffect;

    public override void Commit()
    {
        switch (postProcessingEffect)
        {
            case PostProcessingEffect.DepthOfField:
                ppv.profile.settings.Find(s => s.GetType() == typeof(UnityEngine.Rendering.PostProcessing.DepthOfField)).active = GetCurrentOption();
                break;

            case PostProcessingEffect.Blur:
                ppv.profile.settings.Find(s => s.GetType() == typeof(UnityEngine.Rendering.PostProcessing.MotionBlur)).active = GetCurrentOption();
                break;
            case PostProcessingEffect.Bloom:
                ppv.profile.settings.Find(s => s.GetType() == typeof(UnityEngine.Rendering.PostProcessing.Bloom)).active = GetCurrentOption();
                break;
            case PostProcessingEffect.AmbientOcclusion:
                ppv.profile.settings.Find(s => s.GetType() == typeof(UnityEngine.Rendering.PostProcessing.AmbientOcclusion)).active = GetCurrentOption();
                break;

        }

        
            
    }
}
