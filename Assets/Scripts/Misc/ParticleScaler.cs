using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
    [SerializeField]
    ParticleSystemScalingMode scalingMode;

    [SerializeField]
    float scale = 1;

    ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var main = ps.main;
        main.scalingMode = scalingMode;//ParticleSystemScalingMode.Hierarchy;
        
        ps.transform.parent.localScale = scale*Vector3.one;
    }
}
