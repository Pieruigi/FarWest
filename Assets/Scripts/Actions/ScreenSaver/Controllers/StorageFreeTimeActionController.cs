using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    List<AudioClip> mumbleClips;

    [SerializeField]
    List<AudioClip> countClips;

    [SerializeField]
    AudioClip claimClip;

    ChicoFXController ctrl;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        ctrl = player.GetComponent<ChicoFXController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {

        if (message == "PlayMumble")
            ctrl.Play(mumbleClips[Random.Range(0, mumbleClips.Count)]);

        if (message == "PlayClaim")
            ctrl.Play(claimClip);

        if (message == "PlayCount1")
            ctrl.Play(countClips[0]);

        if (message == "PlayCount2")
            ctrl.Play(countClips[1]);

        if (message == "PlayCount3")
            ctrl.Play(countClips[2]);


    }

}
