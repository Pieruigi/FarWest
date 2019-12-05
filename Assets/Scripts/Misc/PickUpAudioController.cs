using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAudioController : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        if (source.isPlaying)
            return;

        source.clip = clip;
        source.loop = false;
        source.Play();
    }
}
