using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class GameCamera : MonoBehaviour
{
    [SerializeField]
    Vector3 displacement;

    PlayerController player;

    float mouseSpeed = 1f;

    Vector3 mousePosLast;
    bool isDragging = false;

    float yaw = 0;
    float pitch = 0;
    float pitchMax = 35;
    float pitchMin = -35;
    

    float playerVerticalOffset = 1;
    float defaultRotX;

    float fovMax = 60;
    float fovMin = 30;
    float fovCurr;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            player = GameObject.FindObjectOfType<PlayerController>();
            defaultRotX = transform.eulerAngles.x;
            fovCurr = Camera.main.fieldOfView;
        }


        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = player.transform.position + displacement;
        transform.LookAt(player.transform.position + playerVerticalOffset*Vector3.up);

        // Check input
        if (player.IsInputEnabled)
        {
            if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                mousePosLast = Input.mousePosition;
                isDragging = true;
            }
            else
            {
                if (Input.GetMouseButtonUp(2) || Input.GetKeyUp(KeyCode.LeftControl))
                {
                    isDragging = false;
                    
                    //LeanTween.value(gameObject, OnPitchUpdate, pitch, 0, 0.1f); // Manca call on update
                    
                }
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                LeanTween.value(gameObject, OnPitchUpdate, pitch, 0, 0.1f); // Manca call on update
            }
        }
        else
        {
            isDragging = false;
        }



        // Rotate
        if (isDragging)
        {
            yaw += mouseSpeed * (Input.mousePosition.x - mousePosLast.x);
            pitch += mouseSpeed / 2 * (Input.mousePosition.y - mousePosLast.y);
            if (pitch > pitchMax)
            {
                pitch = pitchMax;
            }
            else
            {
                if (pitch < pitchMin)
                    pitch = pitchMin;
            }


            mousePosLast = Input.mousePosition;
        }

        transform.RotateAround(player.transform.position, Vector3.up, yaw);
        
        transform.RotateAround(player.transform.position + playerVerticalOffset * Vector3.up, transform.right, pitch);

        if (player.IsInputEnabled)
        {
            if (Input.mouseScrollDelta != Vector2.zero)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    fovCurr += 5f;
                    if (fovCurr > fovMax)
                        fovCurr = fovMax;
                }
                else
                {
                    fovCurr -= 5f;
                    if (fovCurr < fovMin)
                        fovCurr = fovMin;
                }
            }
        }


        if (fovCurr != Camera.main.fieldOfView)
            Camera.main.fieldOfView = fovCurr;

        // Clipping 
        Vector3 clippedPos = Vector3.zero;
        Vector3 targetPos = player.transform.position + playerVerticalOffset * Vector3.up;
        if (Utility.CameraIsClipped(targetPos, transform.position, out clippedPos))
            transform.position = clippedPos;
    }

    void OnPitchUpdate(float value)
    {
        pitch = value;
    }

}
