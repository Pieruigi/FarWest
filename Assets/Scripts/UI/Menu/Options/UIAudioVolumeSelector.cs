using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIAudioVolumeSelector : UISelector<int>
{
    enum MixerType { Music, FX }

    [Header("Audio Volume Section")]
    [SerializeField]
    MixerType mixerType;

    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    bool isScreensaver = false;

    [SerializeField]
    AudioMixerInitializer mixerInitializer; // Since setting audio mixer exposed params doesn't work, we init this objet that will 
                                            // take care of the initialization process on start and then is deleted

    MainManager mainManager;

    protected override void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        DefaultOptionId = 5;
        base.Awake();
    }

    protected override void InitOptionList()
    {
        for (int i = 0; i < 11; i++)
            Options.Add(i);
    }

    public override void Commit()
    {
        SetVolume();
    }

    private void SetVolume()
    {
        if ((mainManager.IsScreenSaver && !isScreensaver) || (!mainManager.IsScreenSaver && isScreensaver))
            return;

        int v = 8 * GetCurrentOption() - 60;
        
        
        switch (mixerType)
        {
            case MixerType.Music:
                if (mixerInitializer)
                    mixerInitializer.MusicVolume = v;
                else
                    mixer.SetFloat("MusicVolume", v);
                break;

            case MixerType.FX:
                if (mixerInitializer)
                    mixerInitializer.FXVolume = v;
                else
                    mixer.SetFloat("FXVolume", v);
                break;
        }
    }
}
