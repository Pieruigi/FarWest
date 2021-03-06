﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingMaker : MonoBehaviour
{
    public UnityAction OnEnabled;
    public UnityAction OnDisabled;
    public UnityAction<Recipe> OnBuildingCreated;
    public UnityAction<Recipe> OnBuildingCancelled;
    public UnityAction<Recipe> OnBuildingDestroyed;

    [SerializeField]
    ParticleSystem psBuildingDust;

    [SerializeField]
    Canvas keys;
    

    Recipe recipe; // Asset to build

    PlayerController player;

    //private static BuildingMaker instance;
    //public static BuildingMaker Instance
    //{
    //    get { return instance; }
    //}

    Inventory inventory;

    GameObject helper;

    bool isEnabled = false;
    public bool IsEnabled
    {
        get { return isEnabled; }
    }

    float rotSpeed = 50;

    bool workbenchEnabled = false;
    public bool WorkbenchEnabled
    {
        set { workbenchEnabled = value; }
    }

    Vector3 buildPos;
    Quaternion buildRot;

    bool isBuilding = false;

    AudioSource source;

    Camera buildingCamera;
    Camera gameCamera;
    FadeInOut fadeInOut;
    public Camera BuildingCamera
    {
        get { return buildingCamera; }
    }

    MainManager mainManager;

    

    private void Awake()
    {
        //if (!instance)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        if (MainManager.Instance.IsScreenSaver)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        inventory = GameObject.FindObjectOfType<Inventory>();
        source = GetComponentInChildren<AudioSource>();

        buildingCamera = GetComponentInChildren<Camera>();
        gameCamera = Camera.main;
        buildingCamera.gameObject.SetActive(false);
        fadeInOut = GameObject.FindObjectOfType<FadeInOut>();

        mainManager = GameObject.FindObjectOfType<MainManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled)
            return;

        if (isBuilding)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            if (!mainManager.SandboxMode)
            {
                foreach (Slot slot in recipe.Resources)
                {
                    inventory.AddItem(slot.Item, slot.Amount);
                    SetEnable(false);
                }
            }
            else
            {
                Recipe tmp = recipe;
                Init(null);
                ResetHelper();
                buildingCamera.GetComponent<BuildingCamera>().ZoomDisabled = true;

                keys.gameObject.SetActive(false);

                if (OnBuildingCancelled != null)
                    OnBuildingCancelled(tmp);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                if (helper)
                {
                    int sign = 1;
                    if (Input.GetKey(KeyCode.Q))
                        sign = -1;

                    helper.transform.Rotate(Vector3.up, sign * rotSpeed * Time.deltaTime);
                }
                
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (helper && helper.GetComponent<BuildingHelper>().Allowed)
                {
                    Build();
                }
            }
        }
    }

    public void ShowKeys(bool value)
    {
        keys.gameObject.SetActive(value);
    }

    public void Init(Recipe recipe)
    {
        this.recipe = recipe;
    }

    public void SetHelper()
    {
        helper = GameObject.Instantiate((recipe.Output as Building).CraftingHelper);
        buildingCamera.GetComponent<BuildingCamera>().Init(helper);
    }

    private void ResetHelper()
    {
        Destroy(helper);
        helper = null;
        buildingCamera.GetComponent<BuildingCamera>().Init(null);
    }

    public void SetEnable(bool value)
    {
 
        if (value)
        {
            isBuilding = false;
            player.SetInputEnabled(false);

            if (!mainManager.SandboxMode) 
            {
                // In building mode recipe has already been selected, but not yet in sandbox mode 
                helper = GameObject.Instantiate((recipe.Output as Building).CraftingHelper);
                buildingCamera.GetComponent<BuildingCamera>().Init(helper);

                // Show keys
                keys.gameObject.SetActive(true);
            }
            else
            {
                // Hide keys
                keys.gameObject.SetActive(false);
                buildingCamera.GetComponent<BuildingCamera>().ZoomDisabled = true;
            }

            //gameCamera.gameObject.SetActive(false);
            //buildingCamera.gameObject.SetActive(true);
            ShowBuildingCamera(true);

            isEnabled = true;

            OnEnabled?.Invoke();

        }
        else
        {
            //if (!gameCamera.gameObject.activeSelf)
            //    gameCamera.gameObject.SetActive(true);
            //if (buildingCamera.gameObject.activeSelf)
            //    buildingCamera.gameObject.SetActive(false);
            ShowBuildingCamera(false);

            if (helper)
                Destroy(helper);


            player.SetInputEnabled(true);

            recipe = null;
            isEnabled = false;

            buildingCamera.GetComponent<BuildingCamera>().ZoomDisabled = true;

            OnDisabled?.Invoke();


        }
      

    }

    public void ShowBuildingCamera(bool value)
    {
        if (!value)
        {
            if (!gameCamera.gameObject.activeSelf)
                gameCamera.gameObject.SetActive(true);
            if (buildingCamera.gameObject.activeSelf)
                buildingCamera.gameObject.SetActive(false);
        }
        else
        {
            gameCamera.gameObject.SetActive(false);
            buildingCamera.gameObject.SetActive(true);
        }
    }

    public void PsBuildingDustPlay(Vector3 pos)
    {
        psBuildingDust.transform.parent.position = pos;
        psBuildingDust.Play();
    }

    private void Build()
    {
        if (!mainManager.SandboxMode)
        {
            isBuilding = true;
            BuildingHelper bh = helper.GetComponent<BuildingHelper>();
            bh.HideArrow();

            bh.NoRaytracing = true;

            //StartCoroutine(DoBuild());
            buildPos = helper.transform.position;
            buildRot = helper.transform.rotation;

            fadeInOut.FadeOut();
            gameCamera.gameObject.SetActive(true);
            buildingCamera.gameObject.SetActive(false);
            fadeInOut.FadeIn();

            player.MoveToTarget(bh.Target.position, OnDestinationReached);
        }
        else
        {
            buildPos = helper.transform.position;
            buildRot = helper.transform.rotation;
            StartCoroutine(SpawnBuilding());

            if (OnBuildingCreated != null)
                OnBuildingCreated.Invoke(recipe);

            // Reset
            Init(null);
            ResetHelper();

        }
 

    }

    private IEnumerator DoBuild()
    {
        //source.transform.position = buildPos; // Set position for 3D audio

        // Play audio
        if(recipe.WorkbenchOnly)
            source.PlayDelayed(7f);
        else
            source.PlayDelayed(9.5f);

        // Play particle effect
        PsBuildingDustPlay(helper.transform.position);
        //psBuildingDust.transform.parent.position = helper.transform.position;
        //psBuildingDust.Play();

        player.transform.rotation = helper.GetComponent<BuildingHelper>().Target.rotation;
        Animator anim = player.GetComponent<Animator>();

        float time;
        if (recipe.WorkbenchOnly)
        {
            time = 9.5f;
            anim.SetTrigger(Constants.AnimationNameUseHammer);
        }
        else
        {
            time = 14f;
            anim.SetTrigger(Constants.AnimationNameUseHand);
        }
        
        yield return new WaitForSeconds(time);

        //
        // Built
        //

        //Destroy(helper);
        //GameObject obj = SpawnManager.Spawn((recipe.Output as Building).SceneObject);

        //obj.transform.position = buildPos;
        //obj.transform.rotation = buildRot;

        //obj.transform.localScale = Vector3.zero;

        //LeanTween.scale(obj, Vector3.one, 1f).setEaseOutElastic();

        //yield return new WaitForSeconds(1);

        //UnityEngine.AI.NavMeshObstacle obs = obj.GetComponent<UnityEngine.AI.NavMeshObstacle>();
        //if (obs)
        //{
        //    obs.enabled = false;
        //    yield return new WaitForSeconds(0.5f);
        //    obs.enabled = true;
        //}

        yield return SpawnBuilding();
        SetEnable(false);
      
    }

    void OnDestinationReached(bool succeed)
    {
        if (!succeed)
            return;
        StartCoroutine(DoBuild());
    }

    IEnumerator SpawnBuilding()
    {
        Destroy(helper);
        GameObject obj = SpawnManager.Spawn((recipe.Output as Building).SceneObject);

        obj.transform.position = buildPos;
        obj.transform.rotation = buildRot;

        obj.transform.localScale = Vector3.zero;

        LeanTween.scale(obj, Vector3.one, 1f).setEaseOutElastic();

        yield return new WaitForSeconds(1);

        UnityEngine.AI.NavMeshObstacle obs = obj.GetComponent<UnityEngine.AI.NavMeshObstacle>();
        if (obs)
        {
            obs.enabled = false;
            yield return new WaitForSeconds(0.5f);
            obs.enabled = true;
        }
    }
  
}
