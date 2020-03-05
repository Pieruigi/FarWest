using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondFreeTimeActionController : BaseFreeTimeActionController
{
    [SerializeField]
    GameObject fishingRod;

    [SerializeField]
    List<GameObject> objects;

    [SerializeField]
    AudioClip baitInWaterClip;

    [SerializeField]
    AudioClip outOfWaterClip;

    [SerializeField]
    List<AudioClip> pondStepsClips;

    GameObject taken;


    Vector3 posDefault;
    Vector3 eulAngDefault;

    AudioSource source;

    private void Awake()
    {
        posDefault = fishingRod.transform.localPosition;
        eulAngDefault = fishingRod.transform.localEulerAngles;

        
        fishingRod.SetActive(false);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for(int i=0;i<objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("TakeRod".Equals(message))
        {
            fishingRod.SetActive(true);
            fishingRod.transform.localScale = Vector3.zero;

            LeanTween.scale(fishingRod, Vector3.one, 1f).setEaseOutElastic();
        }

        if ("DropRod".Equals(message))
        {
            StartCoroutine(DropRod());
        }

        if ("TookSomething".Equals(message))
        {
            taken = objects[Random.Range(0, objects.Count)];
            taken.gameObject.SetActive(true);
        }

        if ("InWater".Equals(message))
        {
            source.clip = baitInWaterClip;
            source.Play();
        }

        if ("OutOfWater".Equals(message))
        {
            source.clip = outOfWaterClip;
            source.Play();
        }

        if ("PlaySteps".Equals(message))
        {
            Player.SendMessage("PlaySteps");
        }

        if ("PondSteps".Equals(message))
        {
            source.clip = pondStepsClips[Random.Range(0, pondStepsClips.Count)];
            source.Play();
        }
    }

    IEnumerator DropRod()
    {
        Animator anim = fishingRod.GetComponent<Animator>();
        anim.applyRootMotion = true;

        Rigidbody rb = fishingRod.AddComponent<Rigidbody>();

        yield return new WaitForSeconds(3);

        Destroy(rb);
        taken.gameObject.SetActive(false);
        taken = null;

        anim.applyRootMotion = false;
        anim.SetTrigger("Reset");
        fishingRod.transform.localPosition = posDefault;
        fishingRod.transform.localEulerAngles = eulAngDefault;
        fishingRod.SetActive(false);

        
    }

    void TookSomething()
    {
        
       
    }
    
}
