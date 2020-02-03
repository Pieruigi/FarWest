using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer: ScriptableObject
{
    UnityAction<Timer> callbackFunc;
    float length;
    
    public static  Timer Create(float length, UnityAction<Timer> callbackFunc)
    {
        Timer t = ScriptableObject.CreateInstance(typeof(Timer)) as Timer;

        t.length = length;
        t.callbackFunc = callbackFunc;
        return t;
    }

    //public Timer(float length, UnityAction<Timer> callbackFunc)
    //{
    //    this.length = length;
    //    this.callbackFunc = callbackFunc;
    //}

    void Start() { }

    public void Start(MonoBehaviour owner)
    {
        owner.StartCoroutine(Countdown());
    }

    //void Start(float length, UnityAction callbackFunc)
    //{
        
    //}

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(length);

        callbackFunc?.Invoke(this);
        

    }
}
