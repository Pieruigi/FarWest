using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustStormController : MonoBehaviour
{

    [SerializeField]
    ParticleSystem psDustStorm;

    float rate = 0.2f;

    float checkEveryInSec = 600f; // Very ten minutes

    float lengthMinInSec = 30;
    float lengthMaxInSec = 120;

    float elapsed = 0;


    // Update is called once per frame
    void Update()
    {
        if (psDustStorm.isPlaying)
            return;

        elapsed += Time.deltaTime;

        if(elapsed > checkEveryInSec)
        {
            elapsed = 0;

            if(Random.Range(0f,1f) < rate)
            {
                psDustStorm.transform.Rotate(Vector3.up, Random.Range(0f, 360f));

                ParticleSystem.MainModule main = psDustStorm.main;

                main.duration = Random.Range(lengthMinInSec, lengthMaxInSec);
                

                psDustStorm.Play();
            }
        }
    }
}
