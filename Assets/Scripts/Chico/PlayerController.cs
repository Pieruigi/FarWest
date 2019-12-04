using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SS;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityAction<LootAction> OnLootStarted;
    public UnityAction OnLootStopped;

    [SerializeField]
    Transform rightHand;

    private bool isInputEnabled = false;
    public bool IsInputEnabled
    {
        get { return isInputEnabled; }
    }
    int inputDisableCount = 0;

    private NavMeshAgent navAgent;

    GameObject selectedObject;
    public GameObject SelectedObject
    {
        get { return selectedObject; }
    }
    private GameObject clickedObject; // The object I want to interact with

    private bool isMovingAlongPath = false;

    private bool isDoingAction = false;


    private bool isActionKeyDown = false;
    private bool isDestroyKeyDown = false;
    bool alreadyThere = false;

    bool actionDestroy = false;

    private Item equipped = null;
    public Item Equipped
    {
        get { return equipped; }
    }

    GameObject equippedObject;

    Animator animator;

    UnityAction<bool> moveCallback;

    float cursorSize = 16f;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
      
        if (GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            this.enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        animator.SetLayerWeight(animator.GetLayerIndex(Constants.AnimationLayerScreenSaver), 0);
        navAgent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInputEnabled)
        {
           

            //
            // Check player input
            //
            RaycastHit info;
            if (RayCastFromMouse(out info))
            {
                GameObject hit = info.collider.gameObject;


                if (LayerMask.LayerToName(hit.layer) == Constants.LayerNameInteraction)// Store the object I want to interact with ( except for the ground )
                {
                    selectedObject = hit;
                }
                else // Ground interaction
                {
                    selectedObject = null;
                }

                Vector3 targetPos = info.point;
                bool actionAllowed = false;
                //bool exists = false;
                if (selectedObject)
                {
                    //Debug.Log("Clicked:" + clickedObject);
                    //target = GetPointFromObject(); 
                    Action selAction = selectedObject.GetComponent<Action>();
                    Destroyer destroyer = selectedObject.GetComponent<Destroyer>();
                    if ((selAction != null && selAction.CanBeDone()) || (destroyer != null && equipped == ItemCollection.GetAssetByCode("ItemHammer")))
                    {
                        actionAllowed = true;
                        if (selAction)
                            targetPos = selAction.GetActualTarget().position;
                        else
                            targetPos = destroyer.Target.position;
                        
                    }
                    

                }
                
                NavMeshPath path = new NavMeshPath();
                bool exists = false;
                if (!selectedObject || actionAllowed)
                {
                    exists = NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path);
                    if (true)//!exists)
                    {
                        NavMeshHit nmHit;
                        if (exists = NavMesh.SamplePosition(targetPos, out nmHit, 10, NavMesh.AllAreas))
                            targetPos = nmHit.position;

                    }
                }



                if (exists)
                {


                    //Cursor.SetCursor(cursorLeft, cursorSize*Vector2.one, CursorMode.ForceSoftware);
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                    {
                        
                        if (Input.GetMouseButtonDown(0))
                        {
                            isActionKeyDown = true;
                            actionDestroy = false;
                        }
                        else
                        {
                            isDestroyKeyDown = true;
                            actionDestroy = true;
                        }
                        
                        if (isDoingAction)
                        {
                            if (clickedObject == null)
                                isDoingAction = false;
                            else
                                clickedObject.SendMessage(Constants.MethodStopExecuting);
                        }
                            
                        if(isActionKeyDown || (isDestroyKeyDown && selectedObject != null && selectedObject.GetComponent<Destroyer>()))
                        {
                            clickedObject = selectedObject;
                            navAgent.destination = targetPos;

                            //Debug.Log("PathEndPos:" + navAgent.remainingDistance);
                            //Debug.Log("PathNextPos:" + navAgent.nextPosition);
                            //Debug.Log("PathDest:" + navAgent.destination);
                            //Debug.Log("PlayerPos:" + transform.position);
                            //Debug.Log("TargetPos:" + targetPos);

                            //Debug.Log("Mag:" + (new Vector2(targetPos.x, targetPos.z) - new Vector2(transform.position.x, transform.position.z)).magnitude);
                            //Debug.Log("StopDist:" + navAgent.stoppingDistance);

                            float mag = (new Vector2(targetPos.x, targetPos.z) - new Vector2(transform.position.x, transform.position.z)).magnitude;
                            if (mag <= 0.001f)
                                mag = 0;

                            if (mag <= navAgent.stoppingDistance)
                            {
                                alreadyThere = true;
                                //Debug.Log("AlreadyThere:" + alreadyThere);
                            }
                                
                        }
                        else
                        {
                            clickedObject = null;
                            selectedObject = null;
                        }
                        //Debug.Log("Clicked:" + clickedObject);

                    }
                }
                else
                {
                   // Cursor.SetCursor(cursorNotAllowed, cursorSize * Vector2.one, CursorMode.ForceSoftware);

                }


            }
        }

       

        //
        // Check if player is moving along a path
        //
        if (navAgent.hasPath)
        {
            if (!isMovingAlongPath)
            {
                isMovingAlongPath = true;

                // Start walk animation
                animator.SetBool("Walk", true);
            }
            
        }

        if ( (isMovingAlongPath && navAgent.remainingDistance <= navAgent.stoppingDistance) || alreadyThere)// || (navAgent.transform.position.x == navAgent.destination.x && navAgent.transform.position.z == navAgent.destination.z))
        {
            moveCallback?.Invoke(true);
            moveCallback = null;

            alreadyThere = false;

            if(animator.GetBool("Walk"))
                animator.SetBool("Walk", false);

            Debug.Log("Destination reached");
            if (isMovingAlongPath)
            {
                isMovingAlongPath = false;
                
            }
            

            if (clickedObject && !actionDestroy) 
            {
                // Do some action
                if (!clickedObject.GetComponent<Action>().KeepPressed || isActionKeyDown)
                {
                    isDoingAction = true;
                    clickedObject.SendMessage(Constants.MethodExecute);

                    if (clickedObject.GetComponent<LootAction>())
                        OnLootStarted?.Invoke(clickedObject.GetComponent<LootAction>()); 

                    //if(!clickedObject.GetComponent<Action>().KeepPressed)
                    //    clickedObject = null;
                }
            }

            if (clickedObject && actionDestroy)
            {
                actionDestroy = false;
                Destroyer d = clickedObject.GetComponent<Destroyer>();
                if (d != null)
                    d.Destroy();
                clickedObject = null;
                selectedObject = null;
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {

            isActionKeyDown = false;
            if (isDoingAction)
            {
                if(clickedObject == null)
                {
                    isDoingAction = false;
                }
                else
                {
                    if (clickedObject.GetComponent<Action>().KeepPressed)
                    {
                        if (clickedObject.GetComponent<LootAction>())
                            OnLootStopped?.Invoke();

                        clickedObject.SendMessage(Constants.MethodStopExecuting);

                    }
                }
                
            }

           
                
        }
       
        
        
    }

    //public void ReachTarget(Vector3 position)
    //{
        
    //    NavMeshHit nmHit;
    //    if (NavMesh.SamplePosition(position, out nmHit, 10, NavMesh.AllAreas))
    //    {
    //        targetPos = nmHit.position;
    //        //Debug.Log("Closest:" + target);
    //    }
    //}

    public void MoveToTarget(Vector3 target, UnityAction<bool> callback)
    {
        
        NavMeshPath path = new NavMeshPath();
        if(!NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path))
        {
            callback?.Invoke(false);
            return;
        }

        moveCallback = callback;
        navAgent.path = path;
    }

    public void SetInputEnabled(bool value)
    {
        if (!value)
        {
            inputDisableCount++;
        }
        else
        {
            inputDisableCount--;
            if (inputDisableCount < 0)
                inputDisableCount = 0;
        }

        Debug.Log("SetInputEnabled:" + value);
        Debug.Log("InputDisabledCount:" + inputDisableCount);
        if (inputDisableCount>0)
            isInputEnabled = false;
        else
            isInputEnabled = true;

    }

    public void ResetIsDoingAction()
    {
        if (clickedObject?.GetComponent<LootAction>())
            OnLootStopped?.Invoke();

        isDoingAction = false;
        clickedObject = null;
        selectedObject = null;

    }

    public void Reset()
    {
        if (navAgent.hasPath)
            navAgent.ResetPath();

        isMovingAlongPath = false;
        clickedObject = null;
        isDoingAction = false;
        isActionKeyDown = false;
        isDestroyKeyDown = false;
        inputDisableCount = 0;
        alreadyThere = false;
    }

    public void EquipItem(Item item)
    {
        if (equipped == item)
            return;


        Item old = equipped;
        equipped = item;

        if (old != null)
        {
            GameObject.Destroy(equippedObject);

            animator.SetBool("EquipTool", false);
            animator.SetBool("EquipTorch", false);
        }

        

        
        if(item != null)
        {
            equippedObject = GameObject.Instantiate(item.EquippmentPrefab);
            equippedObject.transform.parent = rightHand;
            equippedObject.transform.localPosition = Vector3.zero;
            equippedObject.transform.localEulerAngles = Vector3.zero;

            animator.SetBool("EquipTool", true);

            if (equipped == ItemCollection.GetAssetByCode("ItemTorch"))
            {
                animator.SetBool("EquipTorch", true); 
            }

        }
        else
        {
            animator.SetBool("EquipTool", false);
            animator.SetBool("EquipTorch", false);
        }
        
        // Animation
    }

    private bool RayCastFromMouse(out RaycastHit hitInfo)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int mask = LayerMask.GetMask(new string[] { Constants.LayerNameGround, Constants.LayerNameInteraction });
        //mask = ~mask;

        if (Physics.Raycast(ray, out hitInfo, 10000, mask))
        //if (Physics.Raycast(ray, out hitInfo, 10000))
        {
            return true;
        }

        return false;
    }

    void OnPickUpEnter()
    {
        SetInputEnabled(false);
    }

    void OnPickUpExit()
    {
        SetInputEnabled(true);
    }
}
