using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnerType { None, Tree, Rock, Cactus, Soil } // Remember to adjust the SpawnerTypeHigherIndex when adding a new type

public class Spawner : MonoBehaviour
{

    public const int SpawnerTypeHigherIndex = 4;

    List<Transform> spawnPoints;

    int minimum = 2;
    int maximum = 3;

    bool isActive = false;
    public bool IsActive
    {
        get { return isActive; }
    }

    SpawnerType type = SpawnerType.None;
    public SpawnerType Type
    {
        get { return type; }
    }


    private static Transform parent;

    GameObject spawnGroup;


    private void Awake()
    {
        if (!parent)
            parent = transform.parent;
    }

    #region STATIC
    public static Spawner[] GetSpawnersAll()
    {
        return parent.GetComponentsInChildren<Spawner>();
    }
    #endregion

    public void Activate(SpawnerType type)
    {
      
        this.type = type;
        isActive = true;

        // Spawn objects
        spawnGroup = GameObject.Instantiate(GameObject.FindObjectOfType<SceneManager>().GetSpawnGroupPrefab(type), transform);
        spawnGroup.GetComponent<SpawnGroup>().Spawn();


    }


}

