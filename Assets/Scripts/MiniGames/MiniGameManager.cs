using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{

    bool paused = true;
    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }

    bool started = false;

    private void Awake()
    {
        paused = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        started = true;
        paused = false;
    }
}
