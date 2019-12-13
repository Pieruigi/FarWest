using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutFreeTimeActionController : FreeTimeActionController
{
    [SerializeField]
    GameObject door;

    GameObject player;

    float animSpeedOld;

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
        if(message == "DoorOpen")
        {
            //LeanTween.rotateY(door, 60, 2);
            LeanTween.rotateLocal(door, -60f * Vector3.up, 2f);
            return;
        }

        if (message == "DoorClose")
        {
            LeanTween.rotateLocal(door, Vector3.zero, 1f);
            //LeanTween.rotateY(door, 0, 1);
            return;
        }

        if (message == "SetSpeedZero")
        {
            Animator anim = player.GetComponent<Animator>();
            animSpeedOld = anim.speed;
            anim.speed = 0;
            StartCoroutine(ResetAnimSpeed());
            return;
        }
    }

    IEnumerator ResetAnimSpeed()
    {
        float r = Random.Range(10, 20);

        yield return new WaitForSeconds(r);

        Animator anim = player.GetComponent<Animator>();
        anim.speed = animSpeedOld;
    }

}
