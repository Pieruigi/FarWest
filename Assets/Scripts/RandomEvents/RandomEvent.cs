using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    [SerializeField]
    bool unbusyPlayerNeeded = false;

    [SerializeField]
    float checkTime = 0; // Do check every X seconds

    [SerializeField]
    float rate = 0.1f;

    [SerializeField]
    EventController eventController;

    bool isHappening = false;

    bool isWaitingForUnbusyPlayer = false;

    float elapsed = 0;

    PlayerScreenSaver playerSS;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        playerSS = player.GetComponent<PlayerScreenSaver>();
    }

    // Update is called once per frame
    void Update()
    {
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            StartEvent();
            
//        }
//        return;
//#endif

        if (isHappening)
            return;

        elapsed += Time.deltaTime;

        if (elapsed > checkTime)
        {
            elapsed = 0;

            if (Check())
                StartEvent();
        }
    }

    /**
     * Call this method from the event controller once it is completed.
     * */
    public void StopEvent()
    {
        // Player can take decision again
        if (unbusyPlayerNeeded)
            playerSS.TakeDecisionDisabled = false;

        isHappening = false;
    }

    bool Check()
    {
        if (Random.Range(0f, 1f) < rate)
            return true;

        return false;

    }

    void StartEvent()
    {
        StartCoroutine(DoStart());
    }

    IEnumerator DoStart()
    {
        isHappening = true;

        // If we need the player to be unbusy we just avoid him to do something else once finished what he's doing now
        if (unbusyPlayerNeeded)
            playerSS.TakeDecisionDisabled = true;
    
        // We just wait for the player to finish what's doing if we need him unbusy
        while (unbusyPlayerNeeded && playerSS.IsDoingSomething)
            yield return null;


        eventController.Start(this);
    }



    //IEnumerator DoTest()
    //{
    //    Debug.Log("Event is happening now...");

    //    yield return new WaitForSeconds(10);



    //    StopEvent();
    //}
}
