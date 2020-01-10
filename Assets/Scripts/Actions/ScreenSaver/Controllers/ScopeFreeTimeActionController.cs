using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject scopePrefab;

    [SerializeField]
    List<AudioClip> humingClips;

    Transform hand;

    GameObject player;

    GameObject scope;

    ChicoFXController chicoFx;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        hand = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));
        chicoFx = player.GetComponent<ChicoFXController>();
    }

    
    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if(message == "TakeScope")
        {
            TakeScope();
        }

        if (message == "ReleaseScope")
        {
            ReleaseScope();
        }

        if (message == "PlayHum")
        {
            PlayHum();
        }
    }

    void TakeScope()
    {
        scope = Utility.ObjectPopIn(scopePrefab, hand);
    }

    void ReleaseScope()
    {
        Utility.ObjectPopOut(scope);
        scope = null;
    }

    void PlayHum()
    {
        chicoFx.PlayRandom(humingClips);
    }
}
