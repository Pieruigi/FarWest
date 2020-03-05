using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AILabel : MonoBehaviour
{
    string can = "Chico can build by him self.\nClick to disable.";
    string cant = "Chico can't build by him self.\nClick to enable.";

    Text aiLable;

    // Start is called before the first frame update
    void Start()
    {
        aiLable = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int v;
        if(int.TryParse(ProfileCacheManager.Instance.GetValue(FreeTimeBuildManager.PlayerCacheAI), out v))
        {
            if (v == 0)
                aiLable.text = cant;
            else
                aiLable.text = can;
        }
        

    }
}
