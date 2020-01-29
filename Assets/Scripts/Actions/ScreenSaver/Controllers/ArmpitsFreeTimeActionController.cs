using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmpitsFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    List<AudioClip> sniffingClips;

    [SerializeField]
    AudioClip bleahClip;

    [SerializeField]
    AudioClip moaningClip;

    [SerializeField]
    AudioClip fallClip;

    GameObject player;

    ChicoFXController playerFx;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        playerFx = player.GetComponent<ChicoFXController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        if ("PlaySniffing".Equals(message))
        {
            playerFx.PlayRandom(sniffingClips);
        }

        if ("PlayBleah".Equals(message))
        {
            playerFx.Play(bleahClip);
        }

        if ("PlayMoaning".Equals(message))
        {
            playerFx.Play(moaningClip);
        }

        if ("PlayFall".Equals(message))
        {
            playerFx.Play(fallClip);
        }

    }

   
}
