using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHelper : MonoBehaviour
{
    [SerializeField]
    Material[] materials;

    [SerializeField]
    Transform target;

    //[SerializeField]
    GameObject arrow;

    public Transform Target
    {
        get { return target; }
    }

    public bool Allowed
    {
        get { Debug.Log("AlloweProperty - Count:" + count); return count == 0; }
    }

    string buildingLayerName = "BuildingVolume";

    int count = 0;

    Color colorAllowed = new Color(1, 1, 1, 0.2f);
    Color colorNotAllowed = new Color(1, 0, 0, 0.2f);

    BuildingMaker buildingMaker;

    bool noRaytracing = false;
    public bool NoRaytracing
    {
        set { noRaytracing = value; }
    }

    private void Awake()
    {
        buildingMaker = GameObject.FindObjectOfType<BuildingMaker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set color
        for (int i = 0; i < materials.Length; i++)
            materials[i].color = colorAllowed;

        // Hide cursor
        Cursor.visible = false;

        // Get arrow
        arrow = GetComponentInChildren<SpriteRenderer>().gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (noRaytracing)
            return;

        // Adjust position depending on the mouse
        Ray ray = buildingMaker.BuildingCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(Constants.LayerNameGround)))
        {
            transform.position = hit.point;
        }
   
        
    }

    public void HideArrow()
    {
        arrow.SetActive(false);
    }

    private void OnDestroy()
    {
        if(!MainManager.Instance.IsScreenSaver)
            Cursor.visible = true;
    }
    


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer(buildingLayerName))
        {
            
            count++;
            if(count >= 1)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = colorNotAllowed;
                }
            }

            Debug.Log("TriggerEnter - count:" + count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer(buildingLayerName))
        {
            count--;
            if (count < 0) count = 0;
            if(count == 0)
            {   
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = colorAllowed;
                }
            }

            Debug.Log("TriggerExit - count:" + count);
        }
    }


}
