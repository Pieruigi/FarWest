using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventController : MonoBehaviour
{
    [SerializeField]
    float minLength = 0; // Zero to avoid timer

    [SerializeField]
    float maxLength = 0; // Leave it to zero if you don't want to randomize

    RandomEvent owner;

    float currentLenght = 10;

    protected abstract void Execute();


    protected virtual void Awake()
    {
        if (maxLength < minLength)
            maxLength = minLength;

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void Start(RandomEvent owner)
    {
        this.owner = owner;

        currentLenght = Random.Range(minLength, maxLength);

        if (currentLenght > 0)
        {
            new Timer(currentLenght, Callback).Start(this);
        }
       
        Execute();
    }

    /**
     * This is called by the parent class when timer has been using, otherwise call it in the child class.
     * You can implement this class to perform specific tasks before you exit the event.
     * */
    protected virtual void Stop()
    {
        owner.StopEvent();
    }

    protected virtual void Callback(Timer timer)
    {
        Debug.Log("Timer callback");
        Stop();
    }

   
}
