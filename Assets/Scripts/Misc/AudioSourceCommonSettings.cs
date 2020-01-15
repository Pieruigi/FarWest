using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceCommonSettings : MonoBehaviour
{
    AudioSource audioSource;

    float spatialBlend = 1.0f;

    AudioRolloffMode rollOffMode = AudioRolloffMode.Linear;

    float minDistance = 1f;
    float maxDistance = 80f;

    float spread = 180f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();


        audioSource.spatialBlend = spatialBlend;
        audioSource.rolloffMode = rollOffMode;

        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.spread = spread;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
