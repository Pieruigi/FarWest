using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeTimeStateManager : MonoBehaviour
{
    [SerializeField]
    FreeTimeActionCollection actionCollection;
    
    StateCacher stateCacher;

    FreeTimeAction currentAction = null;
   
    PlayerScreenSaver playerSS;

    List<FreeTimeAction> actions;

    private void Awake()
    {


        actions = new List<FreeTimeAction>();
        foreach (FreeTimeAction fta in actionCollection.FreeTimeActions)
            actions.Add(fta);

        Destroy(actionCollection.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSS = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerScreenSaver>();

        stateCacher = GetComponentInParent<StateCacher>();

        UpdateCurrentAction();
        
        Debug.Log("State:"+stateCacher.State);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            stateCacher.State = Random.Range(0, 100);
            Debug.Log("Saving state:" + stateCacher.State);
            CacheManager.Instance.Save();
        }
#endif

        
    }

    public void UpdateCurrentAction()
    {

        if (currentAction != null)
            playerSS.RemoveFreetimeAction(currentAction);

        currentAction = actions[stateCacher.State];

        if (currentAction != null)
            playerSS?.AddFreeTimeAction(currentAction);
    }
}
