using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class RecipeFilterToggle : MonoBehaviour
{
    public enum AssetFilter { Tool, Building }

    [SerializeField]
    AssetFilter filter;

    bool noEvent = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        noEvent = true;
        GetComponent<Toggle>().isOn = false;
        noEvent = false;
    }

    //private void OnDisable()
    //{
    //    noEvent = true;
    //    GetComponent<Toggle>().isOn = false;
    //    noEvent = false;
    //}

    void OnValueChanged(bool value)
    {

        if (noEvent)
            return;

        
        System.Type type = null;
        
        switch (filter)
        {
            case AssetFilter.Tool:
                type = typeof(Item);
                break;

            case AssetFilter.Building:
                type = typeof(Building);
                break;

            
        }

        if (value)
        {
            SendMessageUpwards("FilterEnabled", type);
        }

        else
        {
            SendMessageUpwards("FilterDisabled", type);
        }
            
    }
}
