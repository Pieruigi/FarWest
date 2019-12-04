﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    [SerializeField]
    List<Light> lights;

    [SerializeField]
    List<ParticleSystem> particleSystems;

    [SerializeField]
    AudioClip clip;

    int timeOffInSec = 25200;
    int timeOnInSec = 72000;

    DayNightCycle dayNight;

    bool isOn = false;

    bool isForced = false;

    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        dayNight = GameObject.FindObjectOfType<DayNightCycle>();
        isOn = dayNight.DayTimeInSeconds > timeOnInSec || dayNight.DayTimeInSeconds < timeOffInSec;
        if (isOn)
            LightOn();
        else
            LightOff();
    }

    // Update is called once per frame
    void Update()
    {
        if (isForced)
            return;

        if (dayNight.DayTimeInSeconds > timeOnInSec || dayNight.DayTimeInSeconds < timeOffInSec )
        {
            if(!isOn)
            {
                isOn = true;
                LightOn();
                //foreach(Light l in lights)
                //    l.enabled = true;

                //foreach(ParticleSystem ps in particleSystems)
                //    ps.Play();
            }
        }
        else
        {
            if (isOn)
            {
                isOn = false;
                LightOff();
                //foreach (Light l in lights)
                //    l.enabled = false;

                //foreach (ParticleSystem ps in particleSystems)
                //    ps.Stop();
            }
        }

    }

    public void ForceOn()
    {
        isForced = true;

        if (!isOn)
        {
            isOn = true;
            LightOn();
        }
            
        
    }

    public void StopForcing()
    {
        isForced = false;
    }

    void LightOn()
    {
        foreach (Light l in lights)
            l.enabled = true;

        foreach (ParticleSystem ps in particleSystems)
            ps.Play();

        // Start clip
        if (clip)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
    }

    void LightOff()
    {
        foreach (Light l in lights)
            l.enabled = false;

        foreach (ParticleSystem ps in particleSystems)
            ps.Stop();

        // Stop clip
        if (clip)
        {
            source.loop = false;
            source.Stop();
        }
    }
}
