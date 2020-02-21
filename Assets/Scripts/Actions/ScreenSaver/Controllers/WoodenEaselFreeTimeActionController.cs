using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenEaselFreeTimeActionController : BaseFreeTimeActionController
{
    [SerializeField]
    GameObject palettePrefab;

    [SerializeField]
    Vector3 palettePosition;

    [SerializeField]
    Vector3 paletteEulerAngles;

    [SerializeField]
    GameObject brushPrefab;

    [SerializeField]
    Vector3 brushPosition;

    [SerializeField]
    Vector3 brushEulerAngles;

    [SerializeField]
    Material paintingMaterial;

    [SerializeField]
    List<Texture> paintingList;

    [SerializeField]
    AudioClip magicClip;

    [SerializeField]
    AudioClip clapClip;

    [SerializeField]
    AudioClip brushClip;

    Texture whiteTexture;

    Transform handL, handR;

    GameObject palette, brush;

    AudioSource source;

    protected override void Start()
    {
        base.Start();

        handR = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));
        handL = new List<Transform>(Player.GetComponentsInChildren<Transform>()).Find(p => "hand.l".Equals(p.name));

        whiteTexture = paintingMaterial.mainTexture;
        source = GetComponent<AudioSource>();
    }

    public override void ActionLoopStart(FreeTimeAction action, int loopId)
    {
        base.ActionLoopStart(action, loopId);

        paintingMaterial.mainTexture = whiteTexture;
    }

    public override void ActionMessage(string message)
    {
        base.ActionMessage(message);

        if ("TakePalette".Equals(message))
        {
            TakePalette();
        }
        if ("TakeBrush".Equals(message))
        {
            TakeBrush();
        }
        if ("DropAll".Equals(message))
        {
            ReleaseAll();
        }
        if ("ShowPainting".Equals(message))
        {
            ShowPainting();
        }
        if ("PlayMagic".Equals(message))
        {
            PlayMagic();
        }
        if ("PlayBrush".Equals(message))
        {
            PlayBrush();
        }
        if ("PlayClap".Equals(message))
        {
            PlayClap();
        }
    }

    void TakePalette()
    {
        palette = Utility.ObjectPopIn(palettePrefab, palettePosition, paletteEulerAngles, Vector3.one, handL);
    }

    void TakeBrush()
    {
        brush = Utility.ObjectPopIn(brushPrefab, brushPosition, brushEulerAngles, Vector3.one, handR);
    }

    void ReleaseAll()
    {
        Utility.ObjectPopOut(palette);
        Utility.ObjectPopOut(brush);
    }

    void ShowPainting()
    {
        paintingMaterial.mainTexture = paintingList[Random.Range(0, paintingList.Count)];
    }

    void PlayMagic()
    {
        source.clip = magicClip;
        source.Play();
    }

    void PlayBrush()
    {
        source.clip = brushClip;
        source.Play();
    }

    void PlayClap()
    {
        source.clip = clapClip;
        source.Play();
    }
}
