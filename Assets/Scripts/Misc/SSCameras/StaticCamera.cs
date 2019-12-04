using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    [SerializeField]
    bool lookAtPlayer;
    
    PlayerScreenSaver player;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScreenSaver>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(lookAtPlayer)
            transform.LookAt(player.LookAtTarget);

    }
}
