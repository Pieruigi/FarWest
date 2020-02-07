using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Setting audio mixer exposed parameters on awake is not allowed, so I use a script only for initialization purpose; the script 
// will be destroyed on start completed
public class AudioMixerInitializer : MonoBehaviour
{

    [SerializeField]
    AudioMixer mixer;
    public AudioMixer Mixer
    {
        get { return mixer; }
    }

    int fxVolume, musicVolume;
 
    public int FXVolume
    {
        set { fxVolume = value; }
    }

    public int MusicVolume
    {
        set { musicVolume = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        mixer.SetFloat("MusicVolume", musicVolume);
        mixer.SetFloat("FXVolume", fxVolume);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
