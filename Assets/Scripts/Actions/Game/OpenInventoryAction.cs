using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class OpenInventoryAction : Action
    {
        

        public override bool DoSomething()
        {
            if (GetComponentInParent<Storage>())
                GameObject.FindObjectOfType<InventoryUI>().OpenFromStorage();
            else
                GameObject.FindObjectOfType<InventoryUI>().OpenWithCraftingSystemEnabled(true); // Open from workbench
                


            return true;
        }


        public override bool CanBeDone()
        {
            if (!GetComponentInParent<Storage>() && PlayerController.Equipped != ItemCollection.GetAssetByCode("ItemHammer"))
                return false;
            
            return true;
        }
    }

    
}

