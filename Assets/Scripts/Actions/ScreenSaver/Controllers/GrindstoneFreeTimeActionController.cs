using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindstoneFreeTimeActionController : BaseFreeTimeActionController
{
    [SerializeField]
    GameObject knifePrefab;

    Transform handL, handR;

    [SerializeField]
    List<AudioClip> grindClips;

    [SerializeField]
    AudioSource grindSource;

    [SerializeField]
    ParticleSystem particle;

   // [SerializeField]
    AudioSource source;




    GameObject knife;

    [SerializeField]
    Animator grindAnimator;



    protected override void Start()
    {
        base.Start();

        handR = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        handL = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));

        source = GetComponent<AudioSource>();
        
    }

    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);

       
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("TakeKnife".Equals(message))
        {
            Debug.Log("Take Knife");
            TakeKnife();
        }

        if ("ReleaseKnife".Equals(message))
        {
            ReleaseKnife();
        }

        if ("StartGrind".Equals(message))
        {
            StartGrind();
        }

        if ("StopGrind".Equals(message))
        {
            StopGrind();
        }

        if ("Grinding".Equals(message))
        {
            PlayGrinding();
        }

        if ("StopParticles".Equals(message))
        {
            StopParticles();
        }
        if ("StartParticles".Equals(message))
        {
            StartParticles();
        }
    }

    void StartParticles()
    {
        particle.Play();
    }

    void StopParticles()
    {
        particle.Stop();
    }

    void StartGrind()
    {
        grindAnimator.SetBool("Start", true);
        source.Play();

    }

    void StopGrind()
    {
        grindAnimator.SetBool("Start", false);
        source.Stop();
    }

    void PlayGrinding()
    {
        grindSource.clip = grindClips[Random.Range(0, grindClips.Count)];
        grindSource.Play();
    }

    void ReleaseKnife()
    {
        Utility.ObjectPopOut(knife);
    }

    void TakeKnife()
    {
        knife = Utility.ObjectPopIn(knifePrefab, Vector3.zero, Vector3.zero, Vector3.one, handR);

    }
   

   

    //void PlayClap()
    //{
    //    source.clip = clapClip;
    //    source.Play();
    //}
}
