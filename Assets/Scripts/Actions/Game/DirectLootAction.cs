using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SS
{
    public class DirectLootAction : LootAction
    {

        public UnityAction<DirectLootAction, GameObject> OnResourceTaken;

        
        [Header("Extended Section")]
        [SerializeField]
        List<GameObject> objectList;

        [SerializeField]
        Item lootItem;

        Inventory inventory;

        List<bool> takenFlags;

        bool initialized = false;

        protected override void Start()
        {
            base.Start();

            //if (objectList.Count != LootMax)
            //    throw new System.Exception("Configuration error - objectList.Count != lootMax.");

            inventory = GameObject.FindObjectOfType<Inventory>();

            //// Init the taken flags
            //takenFlags = new List<bool>();
            //for(int i=0;i<objectList.Count; i++)
            //    takenFlags.Add(false);


            //OnRestored += HandleOnRestored;

            //// Init graphycs
            //int count = ( LootMax / LootUnit ) - ( LootCurrent / LootUnit );

            //for (int i = 0; i < count; i++)
            //{
            //    takenFlags[i] = true;
            //    objectList[i].SetActive(false);
            //}

            StartCoroutine(Init());


        }

        IEnumerator Init()
        {
            yield return null;
            // Init the taken flags
            takenFlags = new List<bool>();
            for (int i = 0; i < objectList.Count; i++)
                takenFlags.Add(false);


            OnRestored += HandleOnRestored;

            // Init graphycs
            int count = (LootMax / LootUnit) - (LootCurrent / LootUnit);

            for (int i = 0; i < count; i++)
            {
                takenFlags[i] = true;
                objectList[i].SetActive(false);
            }
        }

        protected override void Loot(int count)
        {
            
            inventory.AddItem(lootItem, count);

            // Remove object to be picked
            bool found = false;
            for(int i=0; i<takenFlags.Count && !found; i++)
            {
                if (!takenFlags[i])
                {
                    found = true;
                    takenFlags[i] = true;

                    
                    OnResourceTaken?.Invoke(this, objectList[i]);
                }
            }
        }

        void HandleOnRestored(LootAction lootAction)
        {
            // Reset the taken flags
            for (int i = 0; i < takenFlags.Count; i++)
                takenFlags[i] = false;
        }
    }

}
