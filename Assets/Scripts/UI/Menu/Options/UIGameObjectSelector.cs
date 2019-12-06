using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameObjectSelector : UIOnOffSelector
{
    [Header("Game Object Section")]
    [SerializeField]
    GameObject gameObj;

    public override void Commit()
    {
        if(GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
            gameObj.SetActive(GetCurrentOption());
    }


}
