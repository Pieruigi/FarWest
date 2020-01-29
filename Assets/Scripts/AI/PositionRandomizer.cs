using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionRandomizer : MonoBehaviour
{
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get a valid position within the navmesh
        Mesh groundMesh = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>().sharedMesh;
        Vector3 source = Utility.GetRandomPointOnMesh(groundMesh);
        NavMeshHit hit;
        float radius = 5f;
        if (NavMesh.SamplePosition(source, out hit, radius, NavMesh.AllAreas))
        {
            source = hit.position;
        }
        transform.position = source;

        Destroy(this);
    }
      
}
