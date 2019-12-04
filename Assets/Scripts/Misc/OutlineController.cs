using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    float range = 6;
    float rangeSqr;

    PlayerController player;

    Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        

        outline = GetComponent<Outline>();
        if (GameObject.FindObjectOfType<MainManager>().IsScreenSaver)
        {
            GameObject.Destroy(outline);
            GameObject.Destroy(this);
            return;
        }

        player = GameObject.FindObjectOfType<PlayerController>();
        rangeSqr = range * range;
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - player.transform.position).sqrMagnitude < rangeSqr)
        {
         
            if (!outline.enabled)
                outline.enabled = true;
        }
        else
        {
            if (outline.enabled)
                outline.enabled = false;
        }
        
    }
}
