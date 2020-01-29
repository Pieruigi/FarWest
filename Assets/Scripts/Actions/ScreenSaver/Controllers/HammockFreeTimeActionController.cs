using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammockFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    AudioClip sleepClip;

    [SerializeField]
    AudioClip wakeUpClip;

    [SerializeField]
    Transform hammockParentNode;

    GameObject player;
    ChicoFXController fx;
    Transform handL, head;

    AudioSource source;
    Animator playerAnim, hammockAnim;

    float minLoop = 20;
    float maxLoop = 60;

    Transform hat;

    Vector3 hatPosDefault;
    Vector3 hatRotDefault;

    bool putHatDefault = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        fx = player.GetComponent<ChicoFXController>();
        handL = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.l"));
        source = GetComponent<AudioSource>();
        playerAnim = player.GetComponent<Animator>();
        head = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("head.x"));

        hat = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.gameObject.tag.ToLower().Equals(Constants.TagHat.ToLower()));

        hatPosDefault = hat.localPosition;
        hatRotDefault = hat.localEulerAngles;

        hammockAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        if ("StopAnim".Equals(message))
        {
            StartCoroutine(StopAnim());
            //playerAnim.StartPlayback();
        }

        if ("TakeHat".Equals(message))
        {
            TakeHat();
            //playerAnim.StartPlayback();
        }

        if ("ReleaseHat".Equals(message))
        {
            ReleaseHat();
            //playerAnim.StartPlayback();
        }

    }

    IEnumerator StopAnim()
    {
        putHatDefault = false;
        float playerAnimSpeed = playerAnim.speed;
        playerAnim.speed = 0;

        player.transform.parent = hammockParentNode;

        hammockAnim.SetBool("Sway", true);

        fx.Play(sleepClip, true);

        yield return new WaitForSeconds(Random.Range(minLoop, maxLoop));
        
        hammockAnim.SetBool("Sway", false);
        
        
        playerAnim.speed = playerAnimSpeed;


        putHatDefault = true;

        fx.Play(wakeUpClip, false);

        yield return new WaitForSeconds(1);
        player.transform.parent = null;
    }

    void TakeHat()
    {
        hat.parent = handL;
    }

    void ReleaseHat()
    {
        hat.parent = head;

        if(putHatDefault)
        {
            putHatDefault = false;
            StartCoroutine(DoPutOnHead(hat, head, hatPosDefault, hatRotDefault));
        }
    }

    public static IEnumerator DoPutOnHead(Transform hat, Transform head, Vector3 hatPosition, Vector3 hatEulerAngles)
    {
        
        yield return null;
        LeanTween.moveLocal(hat.gameObject, hatPosition, 0.2f);
        LeanTween.rotateLocal(hat.gameObject, hatEulerAngles, 0.2f);


    }
}
