using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CauldronFreeTimeActionController : BaseFreeTimeActionController
{
    [SerializeField]
    LightController lightController;

    [SerializeField]
    Transform woodboxEmpty;

    [SerializeField]
    GameObject woodboxPrefab;

    [SerializeField]
    GameObject chefHatPrefab;

    [SerializeField]
    GameObject ladlePrefab;

    [SerializeField]
    GameObject splashParticlePrefab;

    [SerializeField]
    Transform splashParticleEmpty;

    [SerializeField]
    List<AudioClip> vegetableSplashClips;

    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    LightController boilingController;

    List<GameObject> prefabs;

    GameObject currentObject;
    GameObject player;

    GameObject cowboyHat;

    GameObject currentHat;
    GameObject woodbox;
    GameObject ladle;

    Transform handR;
    Transform handL;
    Transform head;

    bool isWearingChefHat = false;

    Vector3 hatPosDefault;
    Vector3 hatRotDefault;

    ChicoFXController playerFx;


    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);

        playerFx = player.GetComponent<ChicoFXController>();

        handR = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        handL = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));
        head = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "head.x".Equals(p.name));
        currentHat = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.gameObject.tag.ToLower().Equals(Constants.TagHat.ToLower())).gameObject;

        Debug.Log("CurrentHat:" + currentHat);

        hatPosDefault = currentHat.transform.localPosition;
        hatRotDefault = currentHat.transform.localEulerAngles;
        cowboyHat = currentHat;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        
        Debug.Log("ActionEnterSttart");
        StartCoroutine(DoLightForceOn());

        ShowWoodbox();

        base.ActionLoopCompleted(action, loopId);
    }

    public override void ActionLoopCompleted(FreeTimeAction action, int loopId)
    {
        StartCoroutine(DoLightStopForcing());
        base.ActionLoopCompleted(action, loopId);
    }

    

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("SwitchHat".Equals(message))
        {
            SwitchHat();
            return;
        }

        if ("TakeWoodenBox".Equals(message))
        {
            TakeWoodenBox();
            return;
        }

        if ("DropVegetables".Equals(message))
        {
            StartCoroutine(DropVegetables());
        }

        if ("DropWoodenBox".Equals(message))
        {
            StartCoroutine(DropWoodenBox());
        }

        if ("TakeLadle".Equals(message))
        {
            TakeLadle();
        }

        if ("DropLadle".Equals(message))
        {
            DropLadle();
        }

       
    }

    void ShowWoodbox()
    {
        Debug.Log("Show Woodbox...");
        woodbox = Utility.ObjectPopIn(woodboxPrefab, woodboxEmpty);
        woodbox.transform.localPosition = Vector3.zero;
        woodbox.transform.localRotation = Quaternion.identity;
    }

    void HideWoodbox()
    {
        Utility.ObjectPopOut(woodbox);
    }

    void TakeWoodenBox()
    {
        woodbox.transform.parent = handR;
    }

    void TakeLadle()
    {
        ladle = Utility.ObjectPopIn(ladlePrefab, handR);
    }

    void DropLadle()
    {
        ///Utility.ObjectPopOut(ladle);
        StartCoroutine(DoDropLadle());
    }

    IEnumerator DoDropLadle()
    {
        ladle.transform.parent = null;
        Rigidbody rb = ladle.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();

        rb.AddForce((Vector3.up + player.transform.right) * 1.5f, ForceMode.VelocityChange);
        //rb.AddRelativeTorque(Vector3.right * 5f);

        yield return new WaitForSeconds(0.25f);

        ladle.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();

        yield return new WaitForSeconds(2);
        Utility.ObjectPopOut(ladle);
    }

    IEnumerator DropVegetables()
    {
        
        bool playClip = true;
        Rigidbody[] rbl = woodbox.GetComponentsInChildren<Rigidbody>();
        StartCoroutine(PlaySplashParticle());
        for (int i=0; i<rbl.Length; i++)
        {
            rbl[i].isKinematic = false;
            if (playClip)
            {
                AudioSource source = rbl[i].gameObject.AddComponent<AudioSource>();
                rbl[i].gameObject.AddComponent<AudioSourceCommonSettings>();


                //UnityEngine.Audio.AudioMixerGroup mg = UnityEngine.Audio.AudioMixer.
                source.outputAudioMixerGroup = mixer.FindMatchingGroups("Fx")[0];
                source.playOnAwake = false;
                source.clip = vegetableSplashClips[Random.Range(0, vegetableSplashClips.Count)];
                source.PlayDelayed(0.1f);
            }
            playClip = !playClip;
            Destroy(rbl[i].gameObject, 0.35f);
            
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }
    }

    IEnumerator PlaySplashParticle()
    {
        GameObject ps = GameObject.Instantiate(splashParticlePrefab, splashParticleEmpty);
        ps.transform.localPosition = Vector3.zero;
        ps.transform.localEulerAngles = new Vector3(-90,0,0);

        ParticleScaler s = ps.AddComponent<ParticleScaler>();
        s.Scale = 0.75f;
        ParticleSystem pss = ps.GetComponent<ParticleSystem>();

        var main = pss.main;
        main.loop = true;
        pss.Play();

        yield return new WaitForSeconds(2);
        pss.Stop();

        Destroy(ps, 2);
        
    }

    IEnumerator DropWoodenBox()
    {
        woodbox.transform.parent = null;
        Rigidbody rb = woodbox.AddComponent<Rigidbody>();
        rb.AddForce( ( Vector3.up - player.transform.forward ) * 2.5f, ForceMode.VelocityChange);
        rb.AddRelativeTorque(Vector3.right * 5f);

        yield return new WaitForSeconds(0.5f);

        woodbox.AddComponent<BoxCollider>();

        yield return new WaitForSeconds(1.5f);
        Utility.ObjectPopOut(woodbox);
    }

    IEnumerator DoLightForceOn()
    {
        yield return new WaitForSeconds(0.5f);
        lightController.ForceOn();
        boilingController.ForceOn();
    }


    IEnumerator DoLightStopForcing()
    {
        yield return new WaitForSeconds(1);

        lightController.StopForcing();
        boilingController.StopForcing();
    }

    void SwitchHat()
    {
        GameObject toRemove = currentHat; // We always need to remove tue current hat
        GameObject toAdd;

        if (!isWearingChefHat)
        {
            toAdd = GameObject.Instantiate(chefHatPrefab, handL);// Utility.ObjectPopIn(chefHatPrefab, handL);
            toAdd.transform.localPosition = Vector3.zero;
            toAdd.transform.localEulerAngles = Vector3.zero;
            toRemove.transform.parent = handR;
        }
        else
        {
            //toAdd = Utility.ObjectPopIn(cowboyHatPrefab, handR);
            toAdd = cowboyHat;
            toRemove.transform.parent = handL;
        }
        
        isWearingChefHat = !isWearingChefHat;

        StartCoroutine(Hide(toRemove, 0.5f));
        StartCoroutine(Show(toAdd, 0.5f));

        currentHat = toAdd;


    }

    IEnumerator Hide(GameObject toRemove, float delay)
    {
        yield return new WaitForSeconds(delay);
        toRemove.GetComponentInChildren<FadeInOutAlpha>().FadeOut();

        if (!isWearingChefHat)
            Destroy(toRemove);
        else
        {
            yield return new WaitForSeconds(1);
            toRemove.SetActive(false);
        }    
    }
 
    IEnumerator Show(GameObject toShow, float delay)
    {
        if (!isWearingChefHat)
            toShow.SetActive(true);

        yield return new WaitForSeconds(delay);
        toShow.GetComponentInChildren<FadeInOutAlpha>().FadeIn();

        yield return new WaitForSeconds(0.5f);

        toShow.transform.parent = head;

        Vector3 pos = Vector3.zero, rot = Vector3.zero;
        if (!isWearingChefHat)
        {
            pos = hatPosDefault;
            rot = hatRotDefault;
        }

        LeanTween.moveLocal(toShow, pos, 0.2f);
        LeanTween.rotateLocal(toShow, rot, 0.2f);
    }
}
