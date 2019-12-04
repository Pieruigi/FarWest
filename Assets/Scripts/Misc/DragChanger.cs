using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragChanger : MonoBehaviour
{
    Rigidbody rb;

    private float newDrag = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (!rb)
            return;
        if(collision.gameObject.layer == LayerMask.NameToLayer(Constants.LayerNameGround))
        {
            rb.drag = newDrag;
         
        }
    }
}
