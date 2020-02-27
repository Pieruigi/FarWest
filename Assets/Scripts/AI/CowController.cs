using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class CowController : MonoBehaviour
{
    enum State { Idle, Walk, Eat }

    [SerializeField]
    List<AudioClip> clips;

    State state = State.Idle;

    float idleMinTime = 10;
    float idleMaxTime = 20;
    float eatMinTime = 10;
    float eatMaxTime = 20;
    
    float time;
    System.DateTime lastCheck;

    float eatRate = 0.2f;

    Animator animator;
    NavMeshAgent agent;
    AudioSource source;

    float sqrStoppingDistance;

    float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance.SandboxMode)
            Destroy(gameObject);

        ComputeTime();
        lastCheck = System.DateTime.UtcNow;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        maxSpeed = agent.speed;
        sqrStoppingDistance *= agent.stoppingDistance * agent.stoppingDistance;

        Vector3 dest = Utility.GetRandomPointOnNavMesh();
        transform.position = dest;
        transform.eulerAngles = Vector3.up * Random.Range(0f, 360f);

        source = GetComponent<AudioSource>();
        StartCoroutine(PlayMuu());
    }

    // Update is called once per frame
    void Update()
    {
        if(state != State.Walk)
        {
            
            if ((System.DateTime.UtcNow - lastCheck).TotalSeconds > time)
            {
                lastCheck = System.DateTime.UtcNow;
                TakeDecision();
            }
        }
        else
        {
            //if (!agent.hasPath) // You might have to create a building along the cow path
            //{
            //    Debug.LogWarning("Cow doesn't have any path to move along... she should.");

            //    // Reset in idle 
            //    animator.speed = 1;
            //    animator.SetBool(state.ToString(), false);
            //    state = State.Idle;
            //}
            //else
            //{
                animator.speed = agent.velocity.magnitude / agent.speed;

                float sqrDist = (agent.destination - transform.position).sqrMagnitude;

                if (sqrDist <= sqrStoppingDistance || agent.speed == 0 || !agent.hasPath)
                {
                    lastCheck = System.DateTime.UtcNow;
                    TakeDecision();
                }
            //}
            
        }
    }

    void TakeDecision()
    {
        
        if (state != State.Idle)
        {
            animator.speed = 1;
            animator.SetBool(state.ToString(), false);
            state = State.Idle;
        }
        else
        {
            float r = Random.Range(0f, 1f);
            if(r < eatRate) 
            {
                state = State.Eat;
                animator.SetBool(state.ToString(), true);
            }
            else
            {
                r = Random.Range(0f, 1f);
                if (r < 0.5f)
                {
                    Vector3 dest = Utility.GetRandomPointOnNavMesh();

                    if (agent.SetDestination(dest))
                    {
                        state = State.Walk;
                        animator.SetBool(state.ToString(), true);
                    }
                    else
                    {
                        Debug.Log("Dest failed");
                    }
                }
            }
        }

        ComputeTime();
    }

    void ComputeTime()
    {
        switch (state)
        {
            case State.Idle:
                time = Random.Range(idleMinTime, idleMaxTime);
                break;
             case State.Eat:
                time = Random.Range(eatMinTime, eatMaxTime);
                break;
        }
    }

    IEnumerator PlayMuu()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            if(Random.Range(0f,1f) < 0.3f)
            {
                source.clip = clips[Random.Range(0, clips.Count)];
                source.Play();
            }
        }
    }

}
