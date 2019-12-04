using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGroundAudio : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    List<AudioClip> clips;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!rb)
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer(Constants.LayerNameGround))
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = clips[Random.Range(0, clips.Count)];
            source.Play();

        }
    }
}
