using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class ColliderCreator : MonoBehaviour
{
    private void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer(Constants.LayerNameInteraction))
        {
            NavMeshObstacle o = GetComponent<NavMeshObstacle>();
            if (o)
            {
                if(o.shape == NavMeshObstacleShape.Capsule)
                {
                    CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();
                    c.center = o.center;
                    c.radius = o.radius;
                    c.height = o.height;
                }
                else
                {
                    BoxCollider c = gameObject.AddComponent<BoxCollider>();
                    c.center = o.center;
                    c.size = o.size;
                }
            }
            
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
