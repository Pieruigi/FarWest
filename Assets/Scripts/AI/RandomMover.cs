using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMover : MonoBehaviour
{
    [SerializeField]
    float idleTime = 0;

    [SerializeField]
    float moveRange = 20;

    bool hasTarget = false;

    Vector3 target;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // In idle mode

            if (!hasTarget) // Get target
            {
                target = Utility.GetRandomPointOnNavMesh();
                hasTarget = true;
                agent.SetDestination(target);
                Debug.Log("Start moving...");
            }

            if (!agent.hasPath)
            {
                Debug.Log("Destination reached");
                hasTarget = false;
                                 
            }
      
        
    }

    private void OnEnable()
    {
        hasTarget = false;
    }

    private void OnDisable()
    {
        hasTarget = false;
    }
}
