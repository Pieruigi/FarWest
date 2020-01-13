using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluff : MonoBehaviour
{
    DayNightCycle dayNight;
    [SerializeField]
    GameObject moustache;

    [SerializeField]
    GameObject beard;

    float moustacheTime = ( 60 * 60 * 24 ) /*one day */ * 2;

    bool moustacheVisible = false;
    public bool HasMoustache
    {
        get { return moustacheVisible; }
    }

    float beardTime = ( 60 * 60 * 24 ) /*one day */ * 5;
    bool beardVisible = false;
    public bool HasBeard
    {
        get { return beardVisible; }
    }


    float fluffElapsed;

    // Start is called before the first frame update
    void Start()
    {
        dayNight = GameObject.FindObjectOfType<DayNightCycle>();

        CacheManager.Instance.OnSave += HandleOnSave;

        float.TryParse(CacheManager.Instance.GetValue(Constants.CacheKeyFluffElapsed), out fluffElapsed);

        if(fluffElapsed < moustacheTime)
        {
            moustache.transform.localScale = Vector3.zero;
        }

        if (fluffElapsed < beardTime)
        {
            beard.transform.localScale = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        fluffElapsed += Time.deltaTime * dayNight.SpeedMultiplier;

        if(fluffElapsed > moustacheTime)
        {
            if (!moustacheVisible)
            {
                moustacheVisible = true;
                LeanTween.scale(moustache, Vector3.one, 1f).setEaseOutElastic();
            }
        }

        if (fluffElapsed > beardTime)
        {
            if (!beardVisible)
            {
                beardVisible = true;
                LeanTween.scale(beard, Vector3.one, 1f).setEaseOutElastic();
            }
        }
    }

    public void Shave()
    {
        fluffElapsed = 0;
        if (moustacheVisible)
        {
            moustacheVisible = false;
            LeanTween.scale(moustache, Vector3.zero, 1f).setEaseOutElastic();
        }
        if (beardVisible)
        {
            beardVisible = false;
            LeanTween.scale(beard, Vector3.zero, 1f).setEaseOutElastic();
        }

    }

    void HandleOnSave()
    {
        CacheManager.Instance.AddOrUpdate(Constants.CacheKeyFluffElapsed, Mathf.RoundToInt(fluffElapsed).ToString());
    }
}
