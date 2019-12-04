using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowingCamera : MonoBehaviour
{
    [SerializeField]
    Vector3 displacement;

    [SerializeField]
    bool keepAlignment;

    //[SerializeField]
    bool lookAtPlayer = true;

    PlayerScreenSaver player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScreenSaver>();
      
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnEnable()
    {
        if (player == null)
            return;

    }

    private void LateUpdate()
    {
        

        if (keepAlignment)
        {
            transform.position = player.transform.position + player.transform.right * displacement.x + player.transform.up * displacement.y + player.transform.forward * displacement.z;
            
        }
        else
        {
            transform.position = player.transform.position + displacement;
        }

        if(lookAtPlayer)
            transform.LookAt(player.LookAtTarget);

        Vector3 clippedPos = Vector3.zero;
        if (Utility.CameraIsClipped(player.LookAtTarget.position, transform.position, out clippedPos))
            transform.position = clippedPos;
    }

    
}
