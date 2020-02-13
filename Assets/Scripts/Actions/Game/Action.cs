using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SS
{

    public enum ReachingMode { Wherever, Itself, Point }

    [System.Serializable]
    public class NeededResource
    {
        [SerializeField]
        Item item;

        [SerializeField]
        int quantity;
    }


    
    public abstract class Action : MonoBehaviour
    {
        //[Header("Resources")]
        //[SerializeField]
        //List<NeededResource> neededResources;

        //[SerializeField]
        //Item neededTool; // You can have ony one tool of each type

        [Header("Reaching Mode")]
        [SerializeField]
        ReachingMode reachingMode;

        [SerializeField]
        List<Transform> targets;
        protected IList<Transform> Targets
        {
            get { return targets; }
        }

        [Header("Interaction")]
        [SerializeField]
        bool keepPressed;
        public bool KeepPressed
        {
            get { return keepPressed; }
        }

        [SerializeField]
        float doSomethingDelay;
        protected float DoSomethingDelay
        {
            get { return doSomethingDelay; }
        }
        //float doSomethingDelayDefault;

        [SerializeField]
        bool repeating; // If true the action will be repeated until the execution stops


        [Header("Animation")]
        //[SerializeField]
        //bool animateOnEnter = false;

        [SerializeField]
        private int onEnterAnimId = -1;
        protected int OnEnterAnimationId
        {
            get { return onEnterAnimId; }
            set { onEnterAnimId = value; }
        }

        
        //[Header("On Exit")]
        //[SerializeField]
        //bool animateOnExit = false;

        //[SerializeField]
        //int onExitAnimId = -1;


        public abstract bool DoSomething();

        bool isExecuting = false;

        float elapsed = 0;
        protected float Elapsed
        {
            get { return elapsed; }
        }

        float speedMultiplier = 1;
        protected float SpeedMultiplier
        {
            get { return speedMultiplier; }
            set { speedMultiplier = value; }
        }

        bool didSomething;

        string lootAnimStr = "Loot";
        string lootAnimIdStr = "LootId";

        Transform lastTarget;

        #region PRIVATE
        PlayerController playerController;
        protected PlayerController PlayerController
        {
            get { return playerController; }
        }

        PlayerScreenSaver playerScreenSaver;
        protected PlayerScreenSaver PlayerScreenSaver
        {
            get { return playerScreenSaver; }
        }
        #endregion

        protected virtual void Awake() 
        {
            //doSomethingDelayDefault = doSomethingDelay;
            //MainManager mainManager = GameObject.FindObjectOfType<MainManager>();
            //if (mainManager.SandboxMode)
            //    Destroy(gameObject);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            playerController = GameObject.FindObjectOfType<PlayerController>();
            playerScreenSaver = GameObject.FindObjectOfType<PlayerScreenSaver>();
        }

        protected virtual void Update()
        {
            if (isExecuting)
            {
                elapsed += Time.deltaTime * speedMultiplier;

                // Check if the child method need to be called
                if (elapsed > doSomethingDelay && (repeating || !didSomething))
                {
                    elapsed = 0;
                    didSomething = true;
                    DoSomething();
                }

                // If I did what's needed and keepPressed is false then I need to leave the execution
                if (didSomething && !keepPressed && !repeating)
                    StopExecuting();
            }


        }

        public virtual void StartExecuting()
        {
            
            float angle = Vector3.SignedAngle (playerController.transform.forward, lastTarget.forward, Vector3.up);
            LeanTween.rotateAround(playerController.gameObject, Vector3.up, angle, 0.5f);

            //elapsed = 0;
            didSomething = false;
            isExecuting = true;
            //doSomethingDelay = doSomethingDelayDefault;
            speedMultiplier = 1;

            if (onEnterAnimId >= 0)
            {
              
                playerController.GetComponent<Animator>().SetFloat(lootAnimIdStr, onEnterAnimId);
                playerController.GetComponent<Animator>().SetBool(lootAnimStr, true);
            }
            
        }

        //public Transform GetActualTarget()
        //{
        //    switch (reachingMode)
        //    {
        //        case ReachingMode.Point:
        //            lastTarget = GetClosestTarget();
        //            return lastTarget;

        //        default:
        //            lastTarget = transform;
        //            return transform;

        //    }
        //}

        public virtual Transform GetActualTarget(Vector3 source)
        {
            switch (reachingMode)
            {
                case ReachingMode.Point:
                    lastTarget = GetClosestTarget(source);
                    return lastTarget;

                default:
                    lastTarget = transform;
                    return transform;

            }
        }

        /**
         * Returns true if all prerequisites are satisfied, otherwise false; for example you can't cut down a tree without having an axe.
         * */
        public virtual bool CanBeDone()
        {
            return true;
        }

        public void StopExecuting()
        {
            isExecuting = false;
            playerController.ResetIsDoingAction();
            
            if(onEnterAnimId >= 0)
                playerController.GetComponent<Animator>().SetBool(lootAnimStr, false);


        }

        //private Transform GetClosestTarget()
        //{
        //    Transform ret = null;
        //    float sqrDist = 0;

        //    foreach(Transform target in targets)
        //    {
        //        if (!ret)
        //        {
        //            ret = target;
        //            if(playerController)
        //                sqrDist = (ret.position - PlayerController.transform.position).sqrMagnitude;
        //            else
        //                sqrDist = (ret.position - PlayerScreenSaver.transform.position).sqrMagnitude;
        //        }
        //        else
        //        {
        //            float tmp;
        //            if(playerController)
        //                tmp = (target.position - PlayerController.transform.position).sqrMagnitude;
        //            else
        //                tmp = (target.position - PlayerScreenSaver.transform.position).sqrMagnitude;

        //            if (tmp < sqrDist)
        //            {
        //                ret = target;
        //                sqrDist = tmp;
        //            }
        //        }
        //    }

        //    return ret;
            

            
        //}
        protected void ReplaceTargets(List<Transform> targets)
        {
            this.targets = targets;
        }

        private Transform GetClosestTarget(Vector3 source)
        {
            Transform ret = null;
            float sqrDist = 0;

            foreach (Transform target in targets)
            {
                if (!ret)
                {
                    ret = target;
                    sqrDist = (ret.position - source).sqrMagnitude;
                    
                }
                else
                {
                    float tmp = (target.position - source).sqrMagnitude;
                    

                    if (tmp < sqrDist)
                    {
                        ret = target;
                        sqrDist = tmp;
                    }
                }
            }

            return ret;



        }

        public virtual void ActionMessage(string message)
        {
            
        }

    }


}

