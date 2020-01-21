using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCamera : MonoBehaviour
{
    GameObject buildingHelper;

    Vector3 posDefault;
    Quaternion rotDefault;

    float zoom = 0;

    float fovDefault;
    float fovMin = 20f;

    Camera cam;

    float zoomSpeed = 10;
    float moveSpeedMax = 10;

    MainManager mainManager;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        posDefault = transform.position;
        rotDefault = transform.rotation;
        fovDefault = cam.fieldOfView;

        mainManager = GameObject.FindObjectOfType<MainManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (!mainManager.SandboxMode)
        {
            if (GameObject.FindObjectOfType<CursorController>())
                GameObject.FindObjectOfType<CursorController>().ForceNotVisible = true;
        }
        else
        {
            zoom = 0;
            cam.fieldOfView = fovDefault;
        }
    }

    private void OnDisable()
    {
        if(GameObject.FindObjectOfType<CursorController>())
            GameObject.FindObjectOfType<CursorController>().ForceNotVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float sY = Input.mouseScrollDelta.y;
        if (sY != 0)
        {
            // Compute zoom
            if(sY > 0)
                zoom += Time.deltaTime * zoomSpeed;
            else
                zoom -= Time.deltaTime * zoomSpeed;

            zoom = Mathf.Clamp01(zoom);

            // Adjust fov
            cam.fieldOfView = (fovMin - fovDefault) * zoom + fovDefault;
            
        }

        // Move camera
        float width = Screen.width;
        float height = Screen.height;
        float dispY = 0, dispX = 0; 
       
        float yMin = height * 0.2f;
        float xMin = width * 0.2f;
        if (Input.mousePosition.y < yMin)
        {
            // Move down
            dispY = -Time.deltaTime * (-moveSpeedMax / yMin * Input.mousePosition.y + moveSpeedMax);
            
        }
        else
        {
            if(Input.mousePosition.y > height - yMin)
            {
                dispY = Time.deltaTime * ( ( moveSpeedMax / yMin * Input.mousePosition.y ) - ( moveSpeedMax / yMin * (height - yMin) ));
                
            }
        }
        if(Input.mousePosition.x < xMin)
        {
            dispX = -Time.deltaTime * (-moveSpeedMax / xMin * Input.mousePosition.x + moveSpeedMax);
        }
        else
        {
            if (Input.mousePosition.x > width - xMin)
            {
                dispX = Time.deltaTime * ((moveSpeedMax / xMin * Input.mousePosition.x) - (moveSpeedMax / xMin * (width - xMin)));

            }
        }
        transform.Translate(dispX, 0, dispY, Space.World);

        // Adjust displacement Z
        float zDispMax = 20;
        float absMaxZ = zDispMax * zoom;
        if(zoom == 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, posDefault.z);
        }
        else
        {
            if (transform.position.z > absMaxZ)
                transform.position = new Vector3(transform.position.x, transform.position.y, absMaxZ);
            else
            if (transform.position.z < -absMaxZ)
                transform.position = new Vector3(transform.position.x, transform.position.y, -absMaxZ);
        }

        // Adjust displacement X
        float zoomMin = 0.6f;
        float xDispMax = 10;
        float absMaxX = xDispMax * ( zoom - zoomMin ) / (1 - zoomMin);
        if(zoom < zoomMin)
        {
            transform.position = new Vector3(posDefault.x, transform.position.y, transform.position.z);
        }
        else
        {
            if (transform.position.x > absMaxX)
                transform.position = new Vector3(absMaxX, transform.position.y, transform.position.z);
            else
            if (transform.position.x < -absMaxX)
                transform.position = new Vector3(-absMaxX, transform.position.y, transform.position.z);
        }
        

    }


    public void Init(GameObject buildingHelper)
    {
        Reset();
        this.buildingHelper = buildingHelper;
    }

    private void Reset()
    {
        transform.position = posDefault;
        transform.rotation = rotDefault;
        cam.fieldOfView = fovDefault;
        zoom = 0;
    }
}
