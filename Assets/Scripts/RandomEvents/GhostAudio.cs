using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAudio : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    [SerializeField]
    List<AudioClip> clips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayBuh()
    {
        source.clip = clips[Random.Range(0, clips.Count)];
        source.Play();
    }


}
