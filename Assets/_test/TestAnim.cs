using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    Animator anim;

    Vector3 bodyPos;
    Quaternion bodyRot;

    bool looping;

    Vector3 pivotPos;
    Vector3 pivotRot;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        pivotPos = new Vector3(2.3f, 0.3093f, 0.93f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
     //       transform.position = pivotPos;
            anim.SetTrigger("Start");
        }
    }

    //void OnAnimationEnterCompleted()
    //{
    //    Debug.Log("EnterCompleted()");
    //    Debug.Log(anim.pivotPosition);
    //    bodyPos = anim.bodyPosition;
    //    bodyRot = anim.bodyRotation;
    //    //anim.playbackTime
        
    //}

    //void OnAnimationLoopStart()
    //{
    //    Debug.Log("LoopStart()");
    //    looping = true;
    //    anim.bodyPosition = bodyPos;
    //    anim.bodyRotation = bodyRot;
    //}
}
