using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMeshSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Mesh mesh;

    [SerializeField]
    int amountMin;

    [SerializeField]
    int amountMax;

    [SerializeField]
    Vector3 scaleMin = Vector3.one;

    [SerializeField]
    Vector3 scaleMax = Vector3.one;

    [SerializeField]
    bool keepConstraints = false;

    [SerializeField]
    Vector3 anglesMin = Vector3.zero;

    [SerializeField]
    Vector3 anglesMax = Vector3.zero;



    // Start is called before the first frame update
    void Start()
    {
        if (amountMax < amountMin)
            amountMax = amountMin;


        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        int count = Random.Range(amountMin, amountMax);

        for(int i=0; i<count; i++)
        {
            // Get a random point
            Vector3 point = Utility.GetRandomPointOnMesh(mesh);

            GameObject g = GameObject.Instantiate(prefab);
            g.transform.parent = transform;
            g.transform.localPosition = point;
            g.transform.eulerAngles = new Vector3(Random.Range(anglesMin.x, anglesMax.x), Random.Range(anglesMin.y, anglesMax.y), Random.Range(anglesMin.z, anglesMax.z));

            float x, y, z;
            if (keepConstraints)
            {
                x = y = z = Random.Range(scaleMin.x, scaleMax.x);
            }
            else
            {
                x = Random.Range(scaleMin.x, scaleMax.x);
                y = Random.Range(scaleMin.y, scaleMax.y);
                z = Random.Range(scaleMin.z, scaleMax.z);
            }
            
            g.transform.localScale = new Vector3(x, y, z);

        }
    }
}
