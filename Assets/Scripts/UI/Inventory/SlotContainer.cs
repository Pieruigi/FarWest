using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainer : MonoBehaviour
{
    [SerializeField]
    GameObject slotPrefab;

    [SerializeField]
    int numberOfSlots;

    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0; i<numberOfSlots; i++)
            GameObject.Instantiate(slotPrefab, transform, false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
