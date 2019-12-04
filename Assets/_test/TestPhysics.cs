using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysics : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Test());
        }
    }

    IEnumerator Test()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.isKinematic = false;
        transform.parent = null;
        //rb.AddTorque(transform.forward, ForceMode.VelocityChange);

        Color c = GetComponent<MeshRenderer>().material.color;
        c.a = 0;
        LeanTween.color(gameObject, c, 1);
        yield return new WaitForSeconds(1.1f);
        //rb.AddForceAtPosition(transform.forward, transform.position + new Vector3(0.1f,0,0), ForceMode.VelocityChange);

        c = GetComponent<MeshRenderer>().material.color;
        c.a = 1;
        LeanTween.color(gameObject, c, 1f);

        Vector3 tmp = transform.localScale;
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, tmp, 0.5f).setEaseOutElastic();
    }
}
