using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject hammerPrefab;

    [Header("Audio Section")]
    [SerializeField]
    AudioSource source;
    
    [SerializeField]
    List<AudioClip> hammerClips;

    [SerializeField]
    List<AudioClip> painClips;

    [SerializeField]
    AudioClip ideaClip;

    [SerializeField]
    List<AudioClip> mumbleClips;

    [SerializeField]
    AudioClip sufferingClip;



    GameObject currentObject;
    GameObject player;
    Transform hand;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        hand = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.r"));
    }

    // Update is called once per frame
    void Update()
    {

    }

  
    public override void ActionMessage(string message)
    {
        if ("TakeHammer".Equals(message))
        {
            TakeHammer();
            return;
        }

        if ("ReleaseHammer".Equals(message))
        {
            ReleaseHammer();
            return;
        }

        if ("PlayHammer".Equals(message))
        {
            PlayRandom(hammerClips);
        }

        if ("PlayPain".Equals(message))
        {
            PlayRandom(painClips);
        }

        if ("PlayIdea".Equals(message))
        {
            source.clip = ideaClip;
            source.Play();
        }

        if ("PlayMumble".Equals(message))
        {
            PlayRandom(mumbleClips);
        }

        if ("PlaySuffering".Equals(message))
        {
            source.clip = sufferingClip;
            source.Play();
        }
    }


    void TakeHammer()
    {
        currentObject = Utility.ObjectPopIn(hammerPrefab, hand);
    }

    void ReleaseHammer()
    {
        Utility.ObjectPopOut(currentObject);
    }

    void PlayRandom(List<AudioClip> clips)
    {
        source.clip = clips[Random.Range(0, clips.Count)];
        source.Play();
    }
    
}
