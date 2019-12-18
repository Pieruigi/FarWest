using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaver : MonoBehaviour
{
    [SerializeField]
    float angle = 20f;

    [SerializeField]
    float time = 20f;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(Vector3.up, -angle / 2f, Space.World);
        LeanTween.rotateAround(gameObject, Vector3.up, angle, time).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
