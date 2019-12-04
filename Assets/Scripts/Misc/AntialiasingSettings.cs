using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AntialiasingSettings : MonoBehaviour
{
    public UnityAction<int> OnAntialiasingChanged;
    
    int quality;

    public int Quality
    {
        get { return quality; }
        set { quality = value; OnAntialiasingChanged?.Invoke(value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
