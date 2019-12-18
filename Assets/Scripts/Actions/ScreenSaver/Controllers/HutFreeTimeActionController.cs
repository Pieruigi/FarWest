using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject door;

    [SerializeField]
    AudioClip doorOpen;

    [SerializeField]
    AudioClip doorClose;

    [SerializeField]
    List<AudioClip> steps;

    [SerializeField]
    AudioClip wistle;

    [SerializeField]
    AudioClip bonesScreeck;

    [SerializeField]
    AudioClip floorCreaking;

    [SerializeField]
    List<AudioClip> squeeze;

    [SerializeField]
    List<AudioClip> floorKicks;

    [SerializeField]
    AudioClip handWork;

    [SerializeField]
    AudioClip hopJump;

    [SerializeField]
    AudioClip woodHit;

    GameObject player;

    float animSpeedOld;

    AudioSource doorSource;

    ChicoFXController playerFx;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        playerFx = player.GetComponent<ChicoFXController>();
        doorSource = door.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

  
    public override void ActionMessage(string message)
    {
        if(message == "DoorOpen")
        {
            LeanTween.rotateLocal(door, 60f * Vector3.up, 2f);
            doorSource.clip = doorOpen;
            doorSource.Play();
            return;
        }

        if (message == "DoorClose")
        {
            LeanTween.rotateLocal(door, Vector3.zero, 1f);
            doorSource.clip = doorClose;
            doorSource.Play();
            return;
        }

        if (message == "SetSpeedZero")
        {
            Animator anim = player.GetComponent<Animator>();
            animSpeedOld = anim.speed;
            anim.speed = 0;
            StartCoroutine(ResetAnimSpeed());
            return;
        }

        if(message == "PlayerStepsWood")
        {
            playerFx.PlayRandom(steps);
        }

        if (message == "Wistle")
        {
            playerFx.Play(wistle);
        }

        if (message == "BonesScreeck")
        {
            playerFx.Play(bonesScreeck);
        }

        if(message == "FloorCreaking")
        {
            playerFx.Play(floorCreaking);
        }

        if (message == "Squeeze")
        {
            playerFx.PlayRandom(squeeze);
        }

        if (message == "FloorKicks")
        {
            playerFx.PlayRandom(floorKicks);
        }

        if (message == "HandWork")
        {
            playerFx.Play(handWork);
        }

        if (message == "HopJump")
        {
            playerFx.Play(hopJump);
        }

        if (message == "WoodHit")
        {
            playerFx.Play(woodHit);
        }


}

    IEnumerator ResetAnimSpeed()
    {
        float r = Random.Range(10, 300);
        
        yield return new WaitForSeconds(r);

        Animator anim = player.GetComponent<Animator>();
        anim.speed = animSpeedOld;
    }

}
