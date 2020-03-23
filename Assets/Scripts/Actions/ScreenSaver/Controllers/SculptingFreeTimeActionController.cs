using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptingFreeTimeActionController : BaseFreeTimeActionController
{
    [SerializeField]
    GameObject minerShatPrefab;

    [SerializeField]
    GameObject rock;

    [SerializeField]
    GameObject plinth;

    [SerializeField]
    GameObject ladder;

    [SerializeField]
    List<GameObject> statuesPrefabs;

    [SerializeField]
    GameObject hammerPrefab;

    [SerializeField]
    GameObject chiselPrefab;

    [SerializeField]
    ParticleSystem particle;

    [SerializeField]
    List<AudioClip> chiselClips;

    GameObject statue;

    Transform handL, handR, head;
    Vector3 hatPositionDefault;
    Vector3 hatEulerAnglesDefault;

    GameObject cowboyHat;
    GameObject minerShat;

    Vector3 rockPositionDefault, rockEulerAnglesDefault;

    Vector3 ladderPositionDefault, ladderAnglesDefault;

    GameObject hammer, chisel;
    AudioSource source;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        handR = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        handL = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));

        cowboyHat = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(t => "Hat".Equals(t.tag)).gameObject;
        head = cowboyHat.transform.parent;
        hatPositionDefault = cowboyHat.transform.localPosition;
        hatEulerAnglesDefault = cowboyHat.transform.localEulerAngles;

        rockPositionDefault = rock.transform.localPosition;
        rockEulerAnglesDefault = rock.transform.eulerAngles;

        ladderPositionDefault = ladder.transform.localPosition;
        ladderAnglesDefault = ladder.transform.localEulerAngles;
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("SwitchHat".Equals(message))
        {
            StartCoroutine(SwitchHat());
        }

        if ("TakeRock".Equals(message))
        {
            TakeRock();
        }

        if ("DropRock".Equals(message))
        {
            DropRock();
        }

        if ("TakeLadder".Equals(message))
        {
            TakeLadder();
        }

        if ("DropLadder".Equals(message))
        {
            DropLadder();
        }

        if ("ShowStatue".Equals(message))
        {
            StartCoroutine(ShowStatue());
        }

        if ("ResetHat".Equals(message))
        {
            StartCoroutine(ResetHat());
        }

        if ("TakeChisel".Equals(message))
        {
            TakeChisel();
        }

        if ("TakeHammer".Equals(message))
        {
            TakeHammer();
        }

        if ("DropTools".Equals(message))
        {
            DropTools();
        }

        if ("PlayParticle".Equals(message))
        {
            PlayParticle();
        }
    }


    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);

        if (!rock.activeSelf)
        {
            rock.SetActive(true);
            rock.transform.localScale = Vector3.zero;
            rock.transform.localPosition = rockPositionDefault;
            rock.transform.localEulerAngles = rockEulerAnglesDefault;
            
            LeanTween.scale(rock, Vector3.one, 1f).setEaseInOutElastic();
        }

        if (statue)
        {
            Utility.ObjectPopOut(statue);
            statue = null;
        }
    }

    

    IEnumerator SwitchHat()
    {
        //hatPositionDefault = cowboyHat.transform.localPosition;
        //hatEulerAnglesDefault = cowboyHat.transform.localEulerAngles;
        cowboyHat.transform.parent = handR;
        

        //yield return new WaitForSeconds(0.5f);
        minerShat = Utility.ObjectPopIn(minerShatPrefab, new Vector3(-0.13f, -1.42f, -0.13f), new Vector3(0f, 36.21f, 0), Vector3.one, handL);
        LeanTween.scale(cowboyHat, Vector3.zero, 1).setEaseInOutElastic();
        
        yield return new WaitForSeconds(0.5f);
        minerShat.transform.parent = head;
        //minerShat.LeanMoveLocal(hatPositionDefault, 0.2f);
        LeanTween.moveLocal(minerShat, hatPositionDefault, 0.2f);
        LeanTween.rotateLocal(minerShat, hatEulerAnglesDefault, 0.2f);
        
    }

    void PlayParticle()
    {
        particle.Play();
        source.clip = chiselClips[Random.Range(0, chiselClips.Count)];
        source.Play();
    }

    void TakeRock()
    {
        rock.transform.parent = handR;
    }

    void DropRock() 
    {
        rock.transform.parent = plinth.transform;
        LeanTween.moveLocal(rock, Vector3.up * 0.42f, 1f).setEaseOutElastic();
        LeanTween.rotateLocal(rock, Vector3.zero, 1f).setEaseOutElastic();
    }

    void TakeLadder()
    {
        ladder.transform.parent = handR;
    }

    void DropLadder()
    {
        ladder.transform.parent = transform;
    }

    IEnumerator ResetLadder()
    {
        yield return new WaitForSeconds(1.5f);
        LeanTween.scale(ladder, Vector3.zero, 1).setEaseInOutElastic();
        yield return new WaitForSeconds(1);
        ladder.transform.localPosition = ladderPositionDefault;
        ladder.transform.localEulerAngles = ladderAnglesDefault;
        LeanTween.scale(ladder, Vector3.one, 1).setEaseInOutElastic();
    }

    IEnumerator ShowStatue()
    {
        LeanTween.scale(rock, Vector3.zero, 1).setEaseInOutElastic();
        yield return new WaitForSeconds(0.5f);
        statue = Utility.ObjectPopIn(statuesPrefabs[Random.Range(0, statuesPrefabs.Count)], Vector3.zero, Vector3.up*180, Vector3.one, plinth.transform);

        yield return new WaitForSeconds(2f);
        rock.SetActive(false);
    }

    IEnumerator ResetHat()
    {
        
        minerShat.transform.parent = handL;
        cowboyHat.transform.parent = handR;
        LeanTween.scale(cowboyHat, Vector3.one, 1).setEaseInOutElastic();
        Utility.ObjectPopOut(minerShat);
        minerShat = null;
        yield return new WaitForSeconds(0.5f);
        
        //yield return new WaitForSeconds(1f);
        cowboyHat.transform.parent = head;
        LeanTween.moveLocal(cowboyHat, hatPositionDefault, 0.2f);
        LeanTween.rotateLocal(cowboyHat, hatEulerAnglesDefault, 0.2f);
    }

    void TakeHammer()
    {
        hammer = Utility.ObjectPopIn(hammerPrefab, new Vector3(-.09f, .09f, -0.01f), new Vector3(-81.45f, -6.99f, -82.93f), Vector3.one, handR);
    }

    void TakeChisel()
    {
        chisel = Utility.ObjectPopIn(chiselPrefab, new Vector3(-0.09f, .09f, 0), new Vector3(0, 0, -91.74f), Vector3.one, handL);
    }

    void DropTools()
    {
        Utility.ObjectPopOut(hammer);
        Utility.ObjectPopOut(chisel);

        StartCoroutine(ResetLadder());
    }
}
