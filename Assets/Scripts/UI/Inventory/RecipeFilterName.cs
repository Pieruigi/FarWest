using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeFilterName : MonoBehaviour
{

    InputField input;

    bool noEvent = false;

    private void Awake()
    {
        input = GetComponent<InputField>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        input.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        noEvent = true;
        input.text = "";
        noEvent = false;
    }

    private void OnValueChanged(string value)
    {
        if (noEvent)
            return;

        SendMessageUpwards("FilterByName", value);
    }
}
