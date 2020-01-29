using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRandomizer : MonoBehaviour
{
    [SerializeField]
    float min;

    [SerializeField]
    float max;

    public float GetRandomValue()
    {
        if (max < min)
            max = min;

        return Random.Range(min, max);
    }


}
