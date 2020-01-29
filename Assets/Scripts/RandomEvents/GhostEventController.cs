using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostEventController : EventController
{
    [SerializeField]
    Mesh groundMesh;
    
    [SerializeField]
    GameObject ghostPrefab;

    GameObject ghost;


    protected override void Execute()
    {
        ghost = GameObject.Instantiate(ghostPrefab);
        ghost.GetComponentInChildren<FadeInOutAlpha>().FadeIn();

    }

}
