using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletteFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject door;

    [SerializeField]
    GameObject journalPrefab;

    bool slamingDoor;

    float angleMax = 70;
    float angleMin = 30;

    float timeMin = 0.5f;
    float timeMax = 1.5f;

    GameObject player;

    GameObject journal;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionMessage(string message)
    {
        if (message == "DoorOpen")
        {
            LeanTween.rotateLocal(door, -75f * Vector3.up, 0.6f);
            //LeanTween.rotateY(door, 60, 0.6f);
            return;
        }

        if (message == "DoorClose")
        {
            LeanTween.rotateLocal(door, Vector3.zero, 0.5f);
            //LeanTween.rotateY(door, 0, 0.5f);
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

        if(message == "TakeJournal")
        {
            TakeJournal(true);
            return;
        }

        if (message == "LeaveJournal")
        {
            TakeJournal(false);
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

                LeanTween.rotateLocal(door, Vector3.up * -angle, time).setEaseOutBack();

                time = Random.Range(1.5f, 4f);
                yield return new WaitForSeconds(time);

                time = Random.Range(timeMin, timeMax);
                LeanTween.rotateLocal(door, Vector3.zero, time).setEaseOutBack();

                yield return new WaitForSeconds(4f);
            }
        }
    }

    void TakeJournal(bool value)
    {
        Transform parent = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(p => "hand.r".Equals(p.name));

        if (value)
        {
            journal = GameObject.Instantiate(journalPrefab, parent, false);
            journal.transform.localPosition = Vector3.zero;
            journal.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Destroy(journal);
        }
    }
}
