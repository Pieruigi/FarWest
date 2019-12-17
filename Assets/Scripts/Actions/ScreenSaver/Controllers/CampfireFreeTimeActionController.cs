using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    LightController lightController;

    [SerializeField]
    List<GameObject> prefabs;

    //[SerializeField]
    //List<AudioClip> clips;

    GameObject currentObject;
    GameObject player; 



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ActionEnterStart(FreeTimeAction action)
    {
        StartCoroutine(DoLightForceOn());
    }

    public override void ActionExitStart(FreeTimeAction action)
    {
        StartCoroutine(DoLightStopForcing());
    }


    public override void ActionMessage(string message)
    {
        if ("CreateBanjo".Equals(message))
        {
            CreateBanjo();
            return;
        }

        if ("DestroyBanjo".Equals(message))
        {
            DestroyBanjo();
            return;
        }

        if ("CreateMarshmallow".Equals(message))
        {
            CreateMarshmallow();
            return;
        }

        if ("ThrowMarshmallow".Equals(message))
        {
            StartCoroutine(ThrowMarshmallow());
            return;
        }

        if ("StartPlayingBanjo".Equals(message))
        {
            StartPlayingBanjo();
        }

        if ("StopPlayingBanjo".Equals(message))
        {
            StopPlayingBanjo();
        }
    }

    IEnumerator DoLightForceOn()
    {
        yield return new WaitForSeconds(4);
        lightController.ForceOn();
    }

    IEnumerator DoLightStopForcing()
    {
        yield return new WaitForSeconds(1);

        lightController.StopForcing();
    }

    void CreateBanjo()
    {
        Transform parent = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "spine_01.x".Equals(p.name));
        currentObject = Utility.ObjectPopIn(prefabs[0], parent);
    }

    private void DestroyBanjo()
    {
        Utility.ObjectPopOut(currentObject);
        currentObject = null;
    }

    void CreateMarshmallow()
    {
        Transform parent = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        currentObject = Utility.ObjectPopIn(prefabs[1], parent);
    }

    IEnumerator ThrowMarshmallow()
    {
        Rigidbody rb = currentObject.AddComponent<Rigidbody>();

        rb.AddForce((2 * Vector3.up - currentObject.transform.root.forward).normalized * 1.75f, ForceMode.VelocityChange);
        rb.AddTorque(-3 * currentObject.transform.root.right, ForceMode.VelocityChange);

        yield return new WaitForSeconds(1);
        Utility.ObjectPopOut(currentObject);
        currentObject = null;
    }

    void StartPlayingBanjo()
    {
        AudioSource source = currentObject.GetComponent<AudioSource>();
        //source.clip = clips[0];
        source.Play();
    }

    void StopPlayingBanjo()
    {
        AudioSource source = currentObject.GetComponent<AudioSource>();
        source.Stop();
    }
}
