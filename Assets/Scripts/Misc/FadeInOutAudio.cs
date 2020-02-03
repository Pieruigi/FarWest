using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutAudio : MonoBehaviour
{
    [SerializeField]
    float fadeTime = 1;
    
    AudioSource source;

    float volumeDefault;

    

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        volumeDefault = source.volume;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        LeanTween.value(gameObject, OnFadeOutUpdate, source.volume, 0, fadeTime);
    }

    public void FadeIn()
    {
        LeanTween.value(gameObject, OnFadeInUpdate, 0, volumeDefault, fadeTime);
    }

    void OnFadeOutUpdate(float value)
    {
        source.volume = value;
    }

    void OnFadeInUpdate(float value)
    {
        source.volume = value;
    }
}
