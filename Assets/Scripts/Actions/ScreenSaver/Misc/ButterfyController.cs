using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterfyController : MonoBehaviour
{
    //Animator animator;
    [SerializeField]
    Mesh mesh;

    float minHeight = 0.4f;

    float maxHeight = 2f;

    float speed = 20f;

    int dir = 1;
    float time = 0.035f;
    float timeDef;
    float dist = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        timeDef = time;
        transform.position = GetNewPosition();

        //LeanTween.moveLocalY(gameObject, 0.01f, 1.1f).setLoopPingPong();

        StartCoroutine(Fly());
    }

    // Update is called once per frame
    void Update()
    {
            
        transform.position  += Vector3.up * Time.deltaTime * dir * dist;
        time -= Time.deltaTime;
        if (time < 0)
        {
            time = timeDef;
            dir *= -1;
        }
            
        
    }

    IEnumerator Fly()
    {
        while (true)
        {
            // Wait in place
            yield return new WaitForSeconds(Random.Range(3f,6f));

            // Get new position
            Vector3 p = GetNewPosition();

            // Go
            float currSpeed = speed * Random.Range(0.8f, 1.2f);
            Vector3 v = p - transform.position;
            LeanTween.move(gameObject, p, v.magnitude / currSpeed);
            transform.forward = v.normalized;
        }
    }

    Vector3 GetNewPosition()
    {
        Vector3 p = transform.root.position + Utility.GetRandomPointOnMesh(mesh);

        p.y += Random.Range(minHeight, maxHeight);
        return p;
    }
}
