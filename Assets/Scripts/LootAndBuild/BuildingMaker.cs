using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingMaker : MonoBehaviour
{
    public static UnityAction OnEnabled;
    public static UnityAction OnDisabled;

    [SerializeField]
    ParticleSystem psBuildingDust;

    Recipe recipe; // Asset to build

    PlayerController player;

    private static BuildingMaker instance;

    Inventory inventory;

    GameObject helper;

    bool isEnabled = false;

    float rotSpeed = 50;

    bool workbenchEnabled = false;
    public static bool WorkbenchEnabled
    {
        set { instance.workbenchEnabled = value; }
    }

    Vector3 buildPos;
    Quaternion buildRot;

    bool isBuilding = false;

    AudioSource source;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        inventory = GameObject.FindObjectOfType<Inventory>();
        source = GetComponentInChildren<AudioSource>();         
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
            Debug.Log("Building interrupted");
            foreach (Slot slot in recipe.Resources)
            {
                inventory.AddItem(slot.Item, slot.Amount);
                SetEnable(false);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E))
            {
                int sign = 1;
                if(Input.GetKey(KeyCode.Q))
                    sign = -1;

                helper.transform.Rotate(Vector3.up, sign * rotSpeed * Time.deltaTime);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (helper.GetComponent<BuildingHelper>().Allowed)
                {
                    Build();
                }
            }
        }
    }

    public static void Init(Recipe recipe)
    {
        instance.recipe = recipe;
        
    }

    public static void SetEnable(bool value)
    {
        if (value)
        {
            instance.isBuilding = false;
            instance.player.SetInputEnabled(false);
            
            instance.helper = GameObject.Instantiate((instance.recipe.Output as Building).CraftingHelper);
            OnEnabled?.Invoke();
            instance.isEnabled = true;
                         
        }
        else
        {
            instance.player.SetInputEnabled(true);

            if (instance.helper)
                Destroy(instance.helper);
            instance.recipe = null;
            instance.isEnabled = false;

            OnDisabled?.Invoke();
        }
    }

    public void PsBuildingDustPlay(Vector3 pos)
    {
        psBuildingDust.transform.parent.position = pos;
        psBuildingDust.Play();
    }

    private void Build()
    {
        instance.isBuilding = true;
        BuildingHelper bh = helper.GetComponent<BuildingHelper>();

        bh.NoRaytracing = true;

        //StartCoroutine(DoBuild());
        buildPos = helper.transform.position;
        buildRot = helper.transform.rotation;


        player.MoveToTarget(bh.Target.position, OnDestinationReached);

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
            time = 12f;
            anim.SetTrigger(Constants.AnimationNameUseHand);
        }
        
        yield return new WaitForSeconds(time);

        

        GameObject obj = SpawnManager.Spawn((instance.recipe.Output as Building).SceneObject);


        Debug.Log("BuildPos:" + buildPos);
        Debug.Log("BuildRot:" + buildRot);

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

        SetEnable(false);
      
    }

    void OnDestinationReached(bool succeed)
    {
        if (!succeed)
            return;
        StartCoroutine(DoBuild());
    }
}
