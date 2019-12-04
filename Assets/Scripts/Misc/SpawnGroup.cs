using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnGroup : MonoBehaviour
{
    [SerializeField]
    GameObject groupPrefab;

    [SerializeField]
    List<GameObject> prefabs;

    [SerializeField]
    float sizeMin;

    [SerializeField]
    float sizeMax;

    [SerializeField]
    int amountMin;

    [SerializeField]
    int amountMax;

    

    public void Spawn()
    {
       
            Validate();

        // First spawn the basic prefab ( example the green to put the trees in )
        if (groupPrefab)
        {
            
            GameObject groupObj = SpawnManager.Spawn(groupPrefab);
            groupObj.transform.position = transform.position;
           
            groupObj.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        }
        

        // Get spawn points
        List<Transform> spawnPoints = new List<Transform>(GetComponentsInChildren<Transform>());
            spawnPoints.Remove(transform);
            if (spawnPoints.Count < amountMin)
                throw new System.Exception("Configuration error - spawnPoints.Count < amountMin.");

            // The amount of objects to spawn
            int count = Random.Range(amountMin, amountMax + 1);

            for (int i = 0; i < count; i++)
            {
                // Choose the spawn point
                //Debug.Log("SPonts.Count:" + transform.childCount);
                Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
                spawnPoints.Remove(point);


                // Create object with random rotation and size
                int prefabId = i < prefabs.Count ? i : Random.Range(0, prefabs.Count);
                GameObject prefab = prefabs[prefabId];
                GameObject obj = SpawnManager.Spawn(prefab);
                obj.transform.position = point.position;
                obj.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
                obj.transform.localScale *= Random.Range(sizeMin, sizeMax);

        }

    }

    private void Validate()
    {
        if (prefabs == null || prefabs.Count == 0)
            throw new System.Exception("Configuration error - the prefab list is empty.");

        if(amountMin < prefabs.Count)
            throw new System.Exception("Configuration error - amountMin < prefabs.Count.");

        if (sizeMin == 0 && sizeMax == 0)
        {
            sizeMin = 1;
            sizeMax = 1;
        }
        else
        {
            if (sizeMin > sizeMax)
                sizeMin = sizeMax;
        }

        if (amountMax < amountMin)
            amountMax = amountMin;
    }
}
