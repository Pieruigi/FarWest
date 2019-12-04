using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorEnableDisable : MonoBehaviour
{
    
    [SerializeField]
    List<Image> images;

    bool isEnable = true;

    
    public void SetEnable(bool value)
    {
       
        if((value && !isEnable) || (!value && isEnable))
        {
            isEnable = value;

            gameObject.SendMessage("ForceInteractablesOff", !isEnable);

            //gameObject.SendMessage("ResetButtons");
        }
        
    }
}
