using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBoardFreeTimeActionController : BaseFreeTimeActionController
{
    //[SerializeField]
    //AudioClip winClip;
    [SerializeField]
    GameObject dartPrefab;

    [SerializeField]
    Transform targetGroup;

    GameObject player;
    ChicoFXController fx;
    Transform handR;

    AudioSource source;

    GameObject[] darts;
    int count = 0;

    List<Transform> targets;
    Vector3 dir;
    bool fly;
    Transform nextTarget;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        fx = player.GetComponent<ChicoFXController>();
        handR = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.r"));
        source = GetComponent<AudioSource>();
        darts = new GameObject[3];
        targets = new List<Transform>();
        for (int i = 0; i < targetGroup.childCount; i++)
            targets.Add(targetGroup.GetChild(i));
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!fly)
            return;
        darts[count].transform.position += 10 * Time.deltaTime * dir;
        if ((nextTarget.position - darts[count].transform.position).sqrMagnitude < 0.04f)
        {
            fly = false;
            darts[count].transform.forward = dir;// + new Vector3(Random.Range(-5f,5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            source.Play();
            count++;
        }
            
    }



    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);
        for(int i=0; i<darts.Length; i++)
        {
            if(darts[i] != null)
                Utility.ObjectPopOut(darts[i]);
        }
        count = 0;

    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);
        if ("TakeDart".Equals(message))
        {
            TakeDart();
        }

        if ("ReleaseDart".Equals(message))
        {
            ReleaseDart();
        }

    }

    void TakeDart()
    {
        Vector3 pos = new Vector3(0f,0.09f,-0.02f);
        Vector3 angles = new Vector3(0f, -83.50f, 0f);
        darts[count] = Utility.ObjectPopIn(dartPrefab, pos, angles, Vector3.one, handR);  
    }


    void ReleaseDart() 
    {
        darts[count].transform.parent = null;

        nextTarget = targets[Random.Range(0, targets.Count)];
        dir = (nextTarget.position - darts[count].transform.position).normalized;
        fly = true;
        darts[count].GetComponent<AudioSource>().Play();
    }
}
