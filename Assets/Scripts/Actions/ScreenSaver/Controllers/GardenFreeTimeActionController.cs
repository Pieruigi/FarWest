using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GardenFreeTimeActionController : BaseFreeTimeActionController
{

    [SerializeField]
    GameObject tomatoPrefab;

    [SerializeField]
    GameObject potatoPrefab;

    [SerializeField]
    GameObject carrotPrefab;

    [SerializeField]
    Transform vegetableGroup;

    [SerializeField]
    GameObject canPrefab;

    [SerializeField]
    GameObject boxPrefab;

    [SerializeField]
    AudioClip whistleClip;

    [SerializeField]
    AudioClip waterFallingClip;

    [SerializeField]
    AudioMixer mixer;

    GameObject can;

    GameObject box;

    StateCacher cacher;

    FreeTimeStateManager stateManager;

    List<Transform> vegetableEmptyList;

    float destroyDelayMin = 44, destroyDelayMax = 53;

    float createDelayMin = 18, createDelayMax = 24f;

    Vector3 defaultCanPosition;
    Vector3 defaultCanAngles;
    Transform defaultCanParent;

    Transform handR, handL;

    float showVegetablesInBoxMin = 5, showVegetablesInBoxMax = 43;

    ChicoFXController chicoFx;

    protected override void Start()
    {
        
        base.Start();

        chicoFx = Player.GetComponent<ChicoFXController>();

        handR = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        handL = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));

        vegetableEmptyList = new List<Transform>();
        for (int i = 0; i < vegetableGroup.childCount; i++)
            vegetableEmptyList.Add(vegetableGroup.GetChild(i));
        
        cacher = GetComponentInParent<StateCacher>();
        Debug.Log("Cacher:" + cacher);
        stateManager = GetComponent<FreeTimeStateManager>();

        if(cacher.State > 0)
        {
            CreateVegetables(false);
        }
    }

    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);

        PlayWhistle();

        if (cacher.State == 0)
            CreateVegetables(true);
        else
            DestroyVegetables();
    }

    public override void ActionExitCompleted(FreeTimeAction action)
    {
        base.ActionExitCompleted(action);

        cacher.State++;
        if (cacher.State > 1)
            cacher.State = 0;

        stateManager.UpdateCurrentAction();
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("TakeCan".Equals(message))
        {
            TakeCan();
        }

        if ("DropCan".Equals(message))
        {
            DropCan();
        }

        if ("TakeBox".Equals(message))
        {
            TakeBox();
        }

        if ("DropBox".Equals(message))
        {
            DropBox();
        }

        if ("PlaySteps".Equals(message))
        {
            chicoFx.PlaySteps();
        }

        if ("StartWater".Equals(message))
        {
            PlayWater(true);
        }

        if ("StopWater".Equals(message))
        {
            PlayWater(false);
        }
    }

    void CreateVegetables(bool usingDelay)
    {
        foreach(Transform t in vegetableGroup)
        {
            GameObject prefab = GetVegetablePrefab(t);

            // Random rotation
            float angle = Random.Range(0f, 360f);

            // Random size
            float sizeMul = Random.Range(0.8f, 1f);

            float delay = 0;
            if (usingDelay)
                delay = Random.Range(createDelayMin, createDelayMax);

            StartCoroutine(CreateVegetable(prefab, Vector3.zero, Vector3.up * angle, Vector3.one * sizeMul, t, delay));
        }

   
    }

    void DestroyVegetables()
    {
        foreach (Transform t in vegetableGroup)
        {
            StartCoroutine(DestroyVegetable(t.GetChild(0).gameObject, Random.Range(destroyDelayMin, destroyDelayMax)));
        }
    }

    GameObject GetVegetablePrefab(Transform t)
    {
        if ("Tomato".Equals(t.name))
            return tomatoPrefab;

        if ("Potato".Equals(t.name))
            return potatoPrefab;

        return carrotPrefab;
    }

    void PlayWater(bool value)
    {
        if(value)
            can.GetComponentInChildren<ParticleSystem>().Play();
        else
            can.GetComponentInChildren<ParticleSystem>().Stop();
    }

    IEnumerator DestroyVegetable(GameObject vegetable, float delay)
    {
        yield return new WaitForSeconds(delay);

        Utility.ObjectPopOut(vegetable);
    }

    IEnumerator CreateVegetable(GameObject prefab, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale, Transform parent, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        Utility.ObjectPopIn(prefab, localPosition, localEulerAngles, localScale, parent);
    }

    void TakeCan()
    {
        can = Utility.ObjectPopIn(canPrefab, Vector3.zero, Vector3.zero, Vector3.one, handL);
        PlayWaterFalling();
    }

    void DropCan()
    {
        can.transform.parent = null;
        Utility.ObjectPopOut(can);
    }

    void TakeBox()
    {
        box = Utility.ObjectPopIn(boxPrefab, new Vector3(0, 0.1761f, -0.2186f), new Vector3(174.926f, 2.89f, 0), Vector3.one, handR);

        for(int i=0; i<box.transform.childCount; i++)
        {
            GameObject v = box.transform.GetChild(i).gameObject;
            Vector3 scale = v.transform.localScale;
            v.transform.localScale = Vector3.zero;
            StartCoroutine(ShowVegetableInBox(v, scale, Random.Range(showVegetablesInBoxMin, showVegetablesInBoxMax)));
        }
    }

    void DropBox()
    {
        box.transform.parent = null;
        LeanTween.moveLocalY(box, 0, 0.05f);
        Utility.ObjectPopOut(box);
    }

    IEnumerator ShowVegetableInBox(GameObject vegetable, Vector3 localScale, float delay)
    {
        yield return new WaitForSeconds(delay);

        LeanTween.scale(vegetable, localScale, 1f).setEaseOutElastic();
    }

    void PlayWhistle()
    {
        GameObject g = new GameObject("AudioSourceHelper");
        AudioSource s = g.AddComponent<AudioSource>();
        g.AddComponent<AudioSourceCommonSettings>();
        g.transform.parent = Player.transform;
        g.transform.localPosition = Vector3.zero;
        s.outputAudioMixerGroup = mixer.FindMatchingGroups("Fx")[0];
        s.clip = whistleClip;

        float delay = 4;
        if (cacher.State == 1)
            delay = 20;

        s.PlayDelayed(delay);
        Destroy(g, delay+s.clip.length+3);
    }

    void PlayWaterFalling()
    {
        
        GameObject g = new GameObject("AudioSourceHelper");
        AudioSource s = g.AddComponent<AudioSource>();
        g.AddComponent<AudioSourceCommonSettings>();
        g.transform.parent = can.transform;
        g.transform.localPosition = Vector3.zero;
        s.outputAudioMixerGroup = mixer.FindMatchingGroups("Fx")[0];
        s.clip = waterFallingClip;
        s.loop = true;

        float stopTime = 20;

        s.PlayDelayed(1.5f);
        
        Destroy(g, stopTime);
    }
}
