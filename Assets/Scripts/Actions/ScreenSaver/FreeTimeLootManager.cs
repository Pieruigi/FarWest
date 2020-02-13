using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS;
using System;


public class FreeTimeLootManager : MonoBehaviour
{
    [SerializeField]
    LootableType type;
    public LootableType LootableType
    {
        get { return type; }
    }

    [SerializeField]
    FreeTimeActionCollection actionCollection;

    List<LootAction> lootableList;

    List<LootAction> lootableAll;

    float updateRate = 5;
    DateTime lastUpdate;
    PlayerScreenSaver playerSS;

    private LootAction lootAction;
    public LootAction CurrentLootAction
    {
        get { return lootAction; }
    }

    private void Awake()
    {
        if (!MainManager.Instance.IsScreenSaver)
            Destroy(gameObject);

        lootableList = new List<LootAction>();
        
        actionCollection.gameObject.SetActive(false);
        lastUpdate = DateTime.Now;
    }

    private void Start()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag(type.ToString());
        lootableAll = new List<LootAction>();
        foreach (GameObject g in tmp)
            lootableAll.Add(g.GetComponentInChildren<LootAction>());

        playerSS = GameObject.FindObjectOfType<PlayerScreenSaver>();
    }

    // Update is called once per frame
    void Update()
    {
        if((DateTime.Now - lastUpdate).TotalSeconds > updateRate)
        {
            if (!playerSS.IsDoingSomething)
            {
                lastUpdate = DateTime.Now;
                UpdateList();
                UpdateActionCollection();
                UpdateAction();
            }
            
        }
    }

    void UpdateList()
    {
        lootableList.Clear();
        
        foreach(LootAction l in lootableAll)
        {
            if (l.LootCurrent > 0)
                lootableList.Add(l);
        }
    }

    //void UpdateActionCollection()
    //{
    //    if (lootableList.Count > 0)
    //    {
    //        if (!actionCollection.gameObject.activeSelf)
    //        {
    //            // Activate and add to action collection
    //            actionCollection.gameObject.SetActive(true);
    //            foreach(FreeTimeAction fta in actionCollection.FreeTimeActions)
    //                playerSS.AddFreeTimeAction(fta);
    //        }
            
    //    }
    //    else
    //    {
    //        if (actionCollection.gameObject.activeSelf)
    //        {
    //            // Disable and remove from the collection
    //            //playerSS.RemoveFreetimeAction(action);
    //            foreach (FreeTimeAction fta in actionCollection.FreeTimeActions)
    //                playerSS.RemoveFreetimeAction(fta);
    //            actionCollection.gameObject.SetActive(false);
    //        }
    //    }
    //}

    void UpdateActionCollection()
    {
        if (lootableList.Count > 0)
        {
                // Update free action list ( AddFreeTimeAction() inserts the action also if it doesn't exist ) 
                foreach (FreeTimeAction fta in actionCollection.FreeTimeActions)
                    playerSS.AddFreeTimeAction(fta);
            
        }
        else
        {
                
                // Remove actions 
                foreach (FreeTimeAction fta in actionCollection.FreeTimeActions)
                    playerSS.RemoveFreetimeAction(fta);
                
            
        }
    }

    void UpdateAction()
    {
        if (lootableList.Count == 0)
            return;

        lootAction = lootableList[UnityEngine.Random.Range(0, lootableList.Count)];

        // Update target
        for(int i=0; i< 2; i++)
        {
            //lootAction = lootableList[i];
            Transform target = lootAction.GetActualTarget(playerSS.transform.position, i==0?false:true);
            (actionCollection.FreeTimeActions[i] as FreeTimeAction).Target = target;
        }

    }
}
