using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    List<AudioClip> sufferingClips;

    [SerializeField]
    AudioClip hatShakeClip;

    [SerializeField]
    AudioClip puffClip;
         
    GameObject player;

    Transform handR, head, hat, hatParentDefault;

    Vector3 hatPosDefault;
    Vector3 hatRotDefault;

    ChicoFXController playerFx;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        hat = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.gameObject.tag.ToLower().Equals(Constants.TagHat.ToLower()));
        handR = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.r"));
        head = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("head.x"));
        playerFx = player.GetComponent<ChicoFXController>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        if ("PlaySuffering".Equals(message))
        {
            PlaySuffering();
        }

        if ("PlayPuff".Equals(message))
        {
            PlayPuff();
        }

        if ("PlayHatShake".Equals(message))
        {
            PlayHatShake();
        }

        if ("TakeHat".Equals(message))
        {
            TakeHat();
        }

        if ("ReleaseHat".Equals(message))
        {
            ReleaseHat();
        }
    }

    void PlaySuffering()
    {
        playerFx.PlayRandom(sufferingClips);
    }

    void PlayPuff()
    {
        playerFx.Play(puffClip);
    }

    void PlayHatShake()
    {
        transform.position = player.transform.position;
        source.clip = hatShakeClip;
        source.Play();
    }

    void TakeHat()
    {
        hatPosDefault = hat.localPosition;
        hatRotDefault = hat.localEulerAngles;
        hatParentDefault = hat.parent;
        hat.parent = handR;
    }

    void ReleaseHat()
    {
        hat.parent = head;
        LeanTween.moveLocal(hat.gameObject, hatPosDefault, 0.2f);
        LeanTween.rotateLocal(hat.gameObject, hatRotDefault, 0.2f);
    }
}
