using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CursorClick { None, Left, Right, Both }

public class CursorController : MonoBehaviour
{
    [SerializeField]
    Sprite notAllowed;

    [SerializeField]
    Sprite left;

    [SerializeField]
    Sprite right;

    [SerializeField]
    Sprite both;

    PlayerController playerController;
    Image image;

    bool isVisible = false;

    bool forceNotVisible = false;
    public bool ForceNotVisible
    {
        set { forceNotVisible = value; }
    }

    private void Awake()
    {
        if (GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            Cursor.visible = false;
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerController>();
        playerController.OnLootStarted += HandleOnLootStarted;
        playerController.OnLootStopped += HandleOnLootStopped;
        image = GetComponentInChildren<Image>();
        image.color = new Color(1,1,1,0);
        isVisible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (forceNotVisible)
        {
            if (Cursor.visible)
                Cursor.visible = false;

            return;
        }

        if(playerController.IsInputEnabled && Input.GetMouseButton(0))
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                ResetPopUp();
            }
            return;
        }
        else
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }

            if (!playerController.IsInputEnabled)
            {
                ResetPopUp();
                return;
            }
        }
        
        GameObject selObj = playerController.SelectedObject;

        if (!selObj)
        {
            ResetPopUp();
        }
        else
        {
            if (selObj)
            {
                SS.Action action = selObj.GetComponent<SS.Action>();
                Destroyer destroyer = selObj.GetComponent<Destroyer>();
                if (!action && !destroyer)
                {
                    ResetPopUp();
                }
                else
                {
                    bool actionOk = false;
                    bool destroyOk = false;
                    if (action)
                    {
                        if (action.CanBeDone())
                        {
                            actionOk = true;
                        }
                    }
                    if (destroyer)
                    {
                        if(destroyer.CanBeDestroyed())
                            destroyOk = true;
                        

                    }

                    SetPopUp(actionOk, destroyOk);


                }

                
            }
        }

        transform.position = Input.mousePosition - 38f*Vector3.up + 20f*Vector3.right;


    }

    private void ResetPopUp()
    {
        if (isVisible)
        {
            isVisible = false;
            LeanTween.color((transform as RectTransform), new Color(1, 1, 1, 0), 0.1f);
        }
        

    }

    private void SetPopUp(bool actionOk, bool destroyerOk)
    {
        if (actionOk && destroyerOk)
        {
            image.sprite = both;
        }
        else
        {
            if (actionOk && !destroyerOk)
            {
                image.sprite = left;
            }
            else
            {
                if (!actionOk && destroyerOk)
                {
                    image.sprite = right;
                }
                else
                {
                    image.sprite = notAllowed;
                }

            }
        }
       
        if (!isVisible)
        {
            isVisible = true;
            LeanTween.color((transform as RectTransform), Color.white, 0.1f);
          
        }
        
    }

    void HandleOnLootStarted(SS.Action action)
    {
        Cursor.visible = false;
        ResetPopUp();
    }

    void HandleOnLootStopped()
    {
        Cursor.visible = true;
        //ResetPopUp();
    }
}
