using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageFullEmpty : MonoBehaviour
{
    [SerializeField]
    List<GameObject> levels;

    Storage storage;

    [SerializeField]
    int currentLevel = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        storage = GameObject.FindObjectOfType<Storage>();
        storage.OnStorageUpdated += HandleOnStorageUpdated;
        foreach(GameObject level in levels)
        {
            level.SetActive(false);
        }
        CheckStorage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckStorage()
    {
        int slotCount = storage.NumberOfSlots - storage.GetNumberOfFreeSlots();

        int oldLevel = currentLevel;

        if(slotCount == 0)
        {
            currentLevel = -1;
        }
        else if(slotCount < 13)
        {
            currentLevel = 0;
        }
        else if(slotCount < 25)
        {
            currentLevel = 1;
        }
        else if(slotCount < 37)
        {
            currentLevel = 2;
        }
        else
        {
            currentLevel = 3;
        }

        if (oldLevel != currentLevel)
        {
            if(currentLevel < 0)
            {
                for (int i = 0; i < levels.Count; i++)
                    levels[i].SetActive(false);
            }

            if(currentLevel > oldLevel)
            {
                for (int i = 0; i < currentLevel - oldLevel; i++)
                    levels[i + 1 + oldLevel].SetActive(true);
            }
            else if(currentLevel < oldLevel)
            {
                for (int i = 0; i < oldLevel - currentLevel; i++)
                    levels[currentLevel + 1 + i].SetActive(false);
            }
            
        }
            


    }

    void HandleOnStorageUpdated()
    {
        CheckStorage();
    }
}
