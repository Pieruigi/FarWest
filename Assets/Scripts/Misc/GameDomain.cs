using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDomain : MonoBehaviour
{
    private void Awake()
    {
        if (MainManager.Instance.IsScreenSaver)
            Destroy(gameObject);
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
