using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject bucket;

    [SerializeField]
    List<AudioClip> fatigueClips;

    [SerializeField]
    List<AudioClip> drinkingClips;

    [SerializeField]
    AudioClip exhaustedClip;

    [SerializeField]
    AudioClip fallingClip;

    [SerializeField]
    AudioClip waterClip;

    GameObject player;
    ChicoFXController fx;
    Transform handL;

    Vector3 bucketPosDefault;
    Vector3 bucketAngDefault;
    Transform bucketParentDefault;

    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        fx = player.GetComponent<ChicoFXController>();
        handL = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.l"));
        //bucketPosDefault = bucket.transform.position;
        //bucketAngDefault = bucket.transform.eulerAngles;
        //bucketParentDefault = bucket.transform.parent;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        if ("TakeBucket".Equals(message))
        {
            TakeBucket();
        }

        if ("ReleaseBucket".Equals(message))
        {
            ReleaseBucket();
        }

        if ("PlayFatigue".Equals(message))
        {
            PlayFatigue();
        }

        if ("PlayFalling".Equals(message))
        {
            Play(fallingClip);
        }

        if ("PlayWater".Equals(message))
        {
            Play(waterClip);
        }

        if ("PlayExhausted".Equals(message))
        {
            PlayExhausted();
        }

        if ("PlayDrinking".Equals(message))
        {
            PlayDrinking();
        }
    }

    void TakeBucket()
    {
        bucketPosDefault = bucket.transform.position;
        bucketAngDefault = bucket.transform.eulerAngles;
        bucketParentDefault = bucket.transform.parent;
        bucket.transform.parent = handL;
    }

    void ReleaseBucket()
    {
        bucket.transform.parent = bucketParentDefault;
        LeanTween.move(bucket, bucketPosDefault, 0.2f);
        LeanTween.rotate(bucket, bucketAngDefault, 0.2f);
        StartCoroutine(ForceRelease());
    }

    IEnumerator ForceRelease()
    {
        yield return new WaitForSeconds(0.5f);
        bucket.transform.parent = bucketParentDefault;
        bucket.transform.position = bucketPosDefault;
        bucket.transform.eulerAngles = bucketAngDefault;
    }

    void PlayFatigue()
    {
        fx.PlayRandom(fatigueClips);
    }

    void PlayExhausted()
    {
        fx.Play(exhaustedClip);
    }

    void PlayDrinking()
    {
        fx.PlayRandom(drinkingClips);
    }

    void Play(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

}
