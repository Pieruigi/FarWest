using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletteFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject door;

    [SerializeField]
    GameObject journalPrefab;

    [SerializeField]
    AudioClip painClip;

    [SerializeField]
    List<AudioClip> fartClips;

    [SerializeField]
    AudioClip doorOpen;

    [SerializeField]
    AudioClip doorClose;

    [SerializeField]
    AudioClip doorCreaking;

    bool slamingDoor;

    float angleMax = 70;
    float angleMin = 30;

    float timeMin = 0.5f;
    float timeMax = 1.5f;

    GameObject player;

    GameObject journal;

    ChicoFXController fxCtrl;

    bool fartEnable = false;

    float fartTime = 5;
    float fartRate = 0.35f;
    float fartElapsed = 0;

    AudioSource doorSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        fxCtrl = player.GetComponent<ChicoFXController>();
        doorSource = door.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fartEnable)
            return;

        fartElapsed += Time.deltaTime;
        if(fartElapsed > fartTime)
        {
            fartElapsed = 0;
            TryFart();
        }
    }

    public override void ActionMessage(string message)
    {
        if (message == "DoorOpen")
        {
            LeanTween.rotateLocal(door, -75f * Vector3.up, 0.6f);
            doorSource.clip = doorOpen;
            doorSource.Play();
            return;
        }

        if (message == "DoorClose")
        {
            LeanTween.rotateLocal(door, Vector3.zero, 0.5f);
            doorSource.clip = doorClose;
            doorSource.Play();
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

        if(message == "PlayPainClip")
        {
            PlayPainClip();
        }

        if (message == "StartFarting")
        {
            fartEnable = true;
            fartElapsed = 0;
        }

        if (message == "StopFarting")
        {
            fartEnable = false;
        }
    }

    IEnumerator SlamingDoor()
    {
        while (slamingDoor)
        {
            float r = Random.Range(0f, 1f);
            if(r <= 0.2f) // Slam
            {
                doorSource.clip = doorCreaking;
                doorSource.Play();

                float angle = Random.Range(angleMin, angleMax);
                float time = Random.Range(timeMax, timeMax);

                LeanTween.rotateLocal(door, Vector3.up * -angle, time).setEaseOutBack();

                time = Random.Range(1.5f, 4f);
                yield return new WaitForSeconds(time);

                doorSource.clip = doorCreaking;
                doorSource.Play();

                time = Random.Range(timeMin, timeMax);
                LeanTween.rotateLocal(door, Vector3.zero, time).setEaseOutBack();

                yield return new WaitForSeconds(8f);
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

    void PlayPainClip()
    {
        fxCtrl.Play(painClip, false);
    }

    void TryFart()
    {
        float r = Random.Range(0f, 1f);
        if(r < fartRate)
        {
            int rc = Random.Range(0, fartClips.Count);
            fxCtrl.Play(fartClips[rc], false);
        }
    }
}
