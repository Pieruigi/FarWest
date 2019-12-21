using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashbasinFreeTimeActionController : FreeTimeActionController
{

    Transform hat;
    Transform handL;
    Transform head;

    Vector3 hatPosDefault;
    Vector3 hatRotDefault;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        hat = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.gameObject.tag.ToLower().Equals(Constants.TagHat.ToLower()));
        handL = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.l"));
        
        head = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("head.x"));
        hatPosDefault = hat.localPosition;
        hatRotDefault = hat.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {

        if (message == "TakeHat")
            WashbasinUtility.TakeHat(hat, handL);

        if (message == "ReleaseHat")
            WashbasinUtility.ReleaseHat(hat);

        if (message == "PutHatOnHead")
            StartCoroutine(WashbasinUtility.DoPutOnHead(hat, head, hatPosDefault, hatRotDefault));
    }
}
