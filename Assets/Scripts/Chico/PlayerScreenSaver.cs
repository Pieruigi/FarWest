using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScreenSaver : MonoBehaviour
{

    [SerializeField]
    Transform lookAtTarget;
    public Transform LookAtTarget
    {
        get { return lookAtTarget; }
    }

    List<FreeTimeAction> freeTimeActions;

    Animator animator;
    NavMeshAgent agent;
    ScreenSaverCameraManager cameraManager;
    
    bool isBusy = false;
    bool isMoving = false;
    bool alreadyThere = false;

    //bool initialized = false;

    // Free time action
    FreeTimeAction currentAction = null; // The current selected free time action
    int currentLoopId;
    int currentLoopCount = 0;
    int loopCount = 0; // The actual loop id

    // Idle
#if FORCE_SS
    float idleRate = 0f; // From 0 to 1
    int testLoopId = 0;
    int testActionId = 1;
#else
    float idleRate = 0.5f; // From 0 to 1
#endif
    int idleAnimationCount = 1; // The number of available idle animations
    int idleAnimationId = -1; // The current idle animation
    float minIdleLength = 4; // The minimum length in seconds
    float maxIdleLength = 7; // The maximum length in seconds
    bool forceIdle = false;

    const string animIdParameter = "SSId";
    const string animEnterParameter = "SSEnter";
    const string animExitParameter = "SSExit";
    const string animLoopParameter = "SSLoop";
    const string animLoopExitParameter = "SSLoopExit";

    const string actionEnter = "ScreenSaverActionEnter";
    const string actionExit = "ScreenSaverActionExit";
    const string setObjectCreationTime = "CreationTime";

    private void Awake()
    {
        if (!GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            this.enabled = false;
            return;
        }

        // Configure animation controller
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(animator.GetLayerIndex(Constants.AnimationLayerScreenSaver), 1);

        // Get agent
        agent = GetComponent<NavMeshAgent>();

        // Get camera manager
        cameraManager = GameObject.FindObjectOfType<ScreenSaverCameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (freeTimeActions == null)
        {
            LoadFreeTimeActions();
            return;
        }

        if (isBusy)
        {
            if (!agent.hasPath) // Destination reached, stop moving
            {
                if (isMoving || alreadyThere) 
                {
                    isMoving = false;
                    alreadyThere = false;
                    if (currentAction == null) // Just idle
                    {
                        animator.SetFloat("IdleId", Random.Range(0, idleAnimationCount));
                        animator.SetBool("Walk", false);
                        StartCoroutine(IdleTimer()); // Start timer to end idle
                    }
                    else // Start action
                    {
                       
                        animator.SetFloat("IdleId", Random.Range(0, idleAnimationCount));
                        animator.SetBool("Walk", false);
                        agent.enabled = false;

                        // Align player if needed
                        if (currentAction.Target)
                        {
                            LeanTween.rotate(gameObject, currentAction.Target.eulerAngles, 0.5f);
                            LeanTween.move(gameObject, currentAction.Target.position, 0.2f);
                        }
                      
                        animator.applyRootMotion = true;

                        // Switch camera if needed
                        if (currentAction.CameraCloseDisabled && Constants.TagCameraClose.Equals(cameraManager.CurrentCamera.tag))
                            cameraManager.ForceSwitchCamera();

                        // Check for any dedicated camera
                        currentAction.CameraController?.UpdateCameraList();
                            

                        if (currentAction.EnterAnimationId >= 0) // There is an enter animation
                        {
                            animator.SetFloat(animIdParameter, currentAction.EnterAnimationId);
                            animator.SetTrigger(animEnterParameter);
                        
                            // Start the action controller
                            currentAction.FreeTimeActionController?.ActionEnterStart(currentAction);
                        }
                        else // No enter animation, loop directly
                        {
                            currentLoopId = currentAction.LoopAnimationIds[Random.Range(0, currentAction.LoopAnimationIds.Count)];
#if FORCE_SS
                            currentLoopId = currentAction.LoopAnimationIds[testLoopId];
#endif

                            animator.SetFloat(animIdParameter, currentLoopId);
                        
                            animator.SetTrigger(animLoopParameter);

                            currentAction.FreeTimeActionController?.ActionLoopStart(currentAction, currentLoopId);
                            loopCount++; // I'm already inside the first and last loop
                        }
                       
                    }
                    
                }
            }
        }
        else
        {
            TakeDecision();
        }

    }

    void LoadFreeTimeActions()
    {
        // Get free time action collection
        freeTimeActions = new List<FreeTimeAction>();
        FreeTimeActionCollection[] tmp = GameObject.FindObjectsOfType<FreeTimeActionCollection>();
        for (int i = 0; i < tmp.Length; i++)
        {
            List<FreeTimeAction> ftal = new List<FreeTimeAction>(tmp[i].FreeTimeActions as IList<FreeTimeAction>);
            foreach (FreeTimeAction fta in ftal)
                freeTimeActions.Add(fta);
        }
        
    }

    void TakeDecision()
    {
      
        isBusy = true;
        if (forceIdle)
        {
            forceIdle = false;
            StartDoingNothing();
        }
        else
        {
            float r = Random.Range(0f, 1f);
            if (r <= idleRate)
                StartDoingNothing();
            else
                StartDoingSomething();
        }
        
    }

    void StartDoingNothing()
    {
        
        currentAction = null;
        Vector3 target = GetRandomTarget();

        MoveTo(target);
    }

    //    void StartDoingSomething()
    //    {

    //        // Get randam action
    //        if (freeTimeActions == null || freeTimeActions.Count == 0)
    //        {
    //            isBusy = false;
    //            return;
    //        }


    //        currentAction = freeTimeActions[Random.Range(0, freeTimeActions.Count)];

    //#if FORCE_SS
    //        currentAction = freeTimeActions[testActionId]; 
    //#endif

    //        // We don't switch the camera yet ( we keep the current one just in case it has been switched recently )
    //        if (currentAction.CameraCloseDisabled) 
    //            cameraManager.CameraCloseDisabled = true;

    //        currentLoopCount = Random.Range(currentAction.MinLoopCount, currentAction.MaxLoopCount + 1);
    //        loopCount = 0;


    //        if (currentAction.Target)
    //            MoveTo(currentAction.Target.position);
    //        else
    //            alreadyThere = true;
    //    }

    void StartDoingSomething()
    {

        // Get randam action
        if (freeTimeActions == null || freeTimeActions.Count == 0)
        {
            isBusy = false;
            return;
        }


        currentAction = freeTimeActions[Random.Range(0, freeTimeActions.Count)];

#if FORCE_SS
        currentAction = freeTimeActions[testActionId]; 
#endif

        // We don't switch the camera yet ( we keep the current one just in case it has been switched recently )
        if (currentAction.CameraCloseDisabled)
            cameraManager.CameraCloseDisabled = true;

        // Avoid to switch camera while setting sketches dedicaded camera
        if (currentAction.CameraController)
            cameraManager.SwitchingDisabled = true; 

        currentLoopCount = Random.Range(currentAction.MinLoopCount, currentAction.MaxLoopCount + 1);
        loopCount = 0;

        Vector3 targetPos;
        if (currentAction.Target)
            targetPos = currentAction.Target.position; // For building free time action
        else
            targetPos = GetRandomTarget(); // For no building free time action

        MoveTo(targetPos);

        //if (currentAction.Target)
        //    MoveTo(currentAction.Target.position);
        //else
        //    alreadyThere = true;
    }

    void MoveTo(Vector3 target)
    {
       
        NavMeshHit hit;
        if(NavMesh.SamplePosition(target, out hit, 10, NavMesh.AllAreas))
        {
            if((hit.position - transform.position).sqrMagnitude > 4)
            {
       
                isMoving = true;
                agent.SetDestination(hit.position);
                animator.SetBool("Walk", true);
            }
            else
            {
                alreadyThere = true;
            }
        }
       
    }

    IEnumerator IdleTimer()
    {
        float time = Random.Range(minIdleLength, maxIdleLength);

        yield return new WaitForSeconds(time);
        isBusy = false;
    }

    Vector3 GetRandomTarget()
    {
        Vector3 ret = transform.position;
        float radius = Random.Range(6f, 28f);
        Vector3 source = transform.position + Random.insideUnitSphere * radius;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(source, out hit, radius, NavMesh.AllAreas))
        {
            ret = hit.position;
        }


        return ret;
    }

    void OnAnimationEnterCompleted()
    {
        
        currentAction.FreeTimeActionController?.ActionEnterCompleted(currentAction);

        currentLoopId = currentAction.LoopAnimationIds[Random.Range(0, currentAction.LoopAnimationIds.Count)];
#if FORCE_SS
        currentLoopId = currentAction.LoopAnimationIds[testLoopId];
#endif

        animator.SetFloat(animIdParameter, currentLoopId);
        animator.SetTrigger(animLoopParameter);

        currentAction.FreeTimeActionController?.ActionLoopStart(currentAction, currentLoopId);

        loopCount++;
    }


    void OnAnimationLoopCompleted()
    {
        currentAction.FreeTimeActionController?.ActionLoopCompleted(currentAction, currentLoopId);

        if (loopCount < currentLoopCount)
        {
            Reloop();
        }
        else
        {
            if(currentAction.ExitAnimationId >= 0)
            {
                animator.SetFloat(animIdParameter, currentAction.ExitAnimationId);
                animator.SetTrigger(animExitParameter);
                currentAction.FreeTimeActionController?.ActionExitStart(currentAction);
            }
            else
            {
                animator.SetTrigger(animLoopExitParameter);
                OnAnimationExitCompleted();
                
            }
            
        
            
        }
            

    }

    void Reloop()
    {
        
        currentLoopId = currentAction.LoopAnimationIds[Random.Range(0, currentAction.LoopAnimationIds.Count)];
        animator.SetFloat(animIdParameter, currentLoopId);

        loopCount++;
        animator.Play(0,animator.GetLayerIndex(Constants.AnimationLayerScreenSaver),0);

        currentAction.FreeTimeActionController?.ActionLoopStart(currentAction, currentLoopId);
    }

    void OnAnimationExitCompleted()
    {
        
        isBusy = false;
        forceIdle = true;
        agent.enabled = true;
        animator.applyRootMotion = false;

        // Reset camera configuration
        cameraManager.CameraCloseDisabled = false;

        // Reset default camera list if needed
        currentAction.CameraController?.ResetCameraList();

        currentAction.FreeTimeActionController?.ActionExitCompleted(currentAction);

        currentAction = null;
        
    }

    void OnActionMessage(string message)
    {
        currentAction.FreeTimeActionController?.ActionMessage(message);
    }
}
