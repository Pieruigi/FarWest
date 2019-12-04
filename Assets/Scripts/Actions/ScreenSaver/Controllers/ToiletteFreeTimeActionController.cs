using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletteFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject door;

    bool slamingDoor;

    float angleMax = 70;
    float angleMin = 30;

    float timeMin = 0.5f;
    float timeMax = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        if (message == "DoorOpen")
        {
            LeanTween.rotateY(door, 60, 0.6f);
            return;
        }

        if (message == "DoorClose")
        {
            LeanTween.rotateY(door, 0, 0.5f);
            return;
        }

        if (message == "SlamingDoorStart")
        {
            slamingDoor = true;
            StartCoroutine(SlamingDoor());
            return;
        }

        if (message == "SlamingDoorStop")
        {
            slamingDoor = false;
            return;
        }
    }

    IEnumerator SlamingDoor()
    {
        while (slamingDoor)
        {
            float r = Random.Range(0f, 1f);
            if(r <= 0.2f) // Slam
            {
                float angle = Random.Range(angleMin, angleMax);
                float time = Random.Range(timeMax, timeMax);

                LeanTween.rotate(door, new Vector3(0, -angle, 0), time).setEaseOutBack();

                time = Random.Range(1.5f, 4f);
                yield return new WaitForSeconds(time);

                time = Random.Range(timeMin, timeMax);
                LeanTween.rotate(door, new Vector3(0, 0, 0), time).setEaseOutBack();

                yield return new WaitForSeconds(4f);
            }
        }
    }
}
