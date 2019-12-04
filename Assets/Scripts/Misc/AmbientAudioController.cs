using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioController : MonoBehaviour
{
    [SerializeField]
    AudioSource sourceDay;

    [SerializeField]
    AudioSource sourceNight;

    DayNightCycle dayNightCycle;

    int timeDay = 25200;
    int timeNight = 72000;

    int fadeTimeDisp = 3000;

    bool fading = false;
    float fadeSpeed = 10;
    //float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle = GameObject.FindObjectOfType<DayNightCycle>();
       
        if (dayNightCycle.DayTimeInSeconds > timeDay - fadeTimeDisp && dayNightCycle.DayTimeInSeconds < timeNight - fadeTimeDisp)
        {
            sourceNight.volume = 0;
            sourceDay.Play();
        }
        else
        {
            sourceDay.volume = 0;
            sourceNight.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!fading)
        {
            if (sourceDay.isPlaying)
            {
                if (dayNightCycle.DayTimeInSeconds > timeNight - fadeTimeDisp)
                    StartCoroutine(FadeAudio(dayNightCycle.DayTimeInSeconds));
            }
            else
            {
                if (dayNightCycle.DayTimeInSeconds < timeDay && dayNightCycle.DayTimeInSeconds > timeDay - fadeTimeDisp)
                    StartCoroutine(FadeAudio(dayNightCycle.DayTimeInSeconds));
            }
        }
    }

    IEnumerator FadeAudio(int dayTimeInSec)
    {
        fading = true;
        bool fromDayToNight = sourceDay.isPlaying ? true : false;
        if (fromDayToNight)
        {
            sourceNight.volume = 0;
            sourceNight.Play();
        }
        else
        {
            sourceDay.volume = 0;
            sourceDay.Play();
        }
            

        float timer = 2*fadeTimeDisp;
       
        while(timer > 0)
        {
            timer -= Time.deltaTime * fadeSpeed * dayNightCycle.SpeedMultiplier;
            if (timer < 0)
                timer = 0;

            float t = ( 2 * fadeTimeDisp - timer ) / (2*fadeTimeDisp);
          
            if (fromDayToNight)
            {
                sourceDay.volume = 1 - t;
                sourceNight.volume = t;
            }
            else
            {
                sourceDay.volume = t;
                sourceNight.volume = 1 - t;
            }

            yield return null;
        }

        if (fromDayToNight)
            sourceDay.Stop();
        else
            sourceNight.Stop();
        
        
        fading = false;

    }
}
