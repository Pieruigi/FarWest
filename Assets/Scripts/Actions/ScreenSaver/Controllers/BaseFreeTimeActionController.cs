using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    List<AudioClip> clips;
   
    ChicoFXController fx;
   
    public GameObject Player
    {
        get { return fx.gameObject; }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log("ASSSSSSSSSSSSSSSSSS");
        fx = GameObject.FindObjectOfType<ChicoFXController>();
    }


    public override void ActionMessage(string message)
    {
        string[] s = message.Split('_');

        Debug.Log("message:" + message);

        // param: "start_count"
        if ("PlayRandomRange".Equals(s[0]))
        {
            PlayRandomRange(int.Parse(s[1]), int.Parse(s[2]));
        }

        // param: id
        if ("Play".Equals(s[0]))
        {
            Play(int.Parse(s[1]));
        }
    }

    void PlayRandomRange(int start, int count)
    {
        Debug.Log("Start:" + start);
        Debug.Log("Count:" + count);
        Debug.Log("Clips.Length:" + clips.Count);
        fx.PlayRandom(clips.GetRange(start, count));
    }

    void Play(int clipId)
    {
        fx.Play(clips[clipId]);
    }
}
