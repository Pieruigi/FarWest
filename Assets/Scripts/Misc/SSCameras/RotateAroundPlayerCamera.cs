using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RotateAroundPlayerCamera : MonoBehaviour
{
    [SerializeField]
    bool clockwise; // From the top

    [SerializeField]
    float speed;

    //[SerializeField]
    bool lookAtPlayer = true;

    PlayerScreenSaver player;

    float dir;

    Vector3 camLocalPosDefault;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScreenSaver>();
        dir = clockwise ? 1 : -1;
        camLocalPosDefault = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.parent.position = player.transform.position; // Set pivot position
        transform.localPosition = camLocalPosDefault; // Reset camera position

        transform.RotateAround(transform.parent.position, Vector3.up, dir * speed * Time.deltaTime);

        if(lookAtPlayer)
            transform.LookAt(player.LookAtTarget);

        Vector3 clippedPos = Vector3.zero;
        if (Utility.CameraIsClipped(player.LookAtTarget.position, transform.position, out clippedPos))
            transform.position = clippedPos;
    }
}
