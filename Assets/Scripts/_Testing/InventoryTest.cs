using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Test()
    {
        Item stone = Resources.Load<Item>("Items/Stone");
        Item water = Resources.Load<Item>("Items/Water");

        Item rope = Resources.Load<Item>("Items/Rope");
        Item wood = Resources.Load<Item>("Items/Wood");
        Item axe = Resources.Load<Item>("Items/Axe");
        Item hammer = Resources.Load<Item>("Items/Hammer");
        Item torch = Resources.Load<Item>("Items/Torch");
        Item pickaxe = Resources.Load<Item>("Items/Pickaxe");
        Item shovel = Resources.Load<Item>("Items/Shovel");
        Item knife = Resources.Load<Item>("Items/Knife");
        Item mud = Resources.Load<Item>("Items/Mud");
        Item dirt = Resources.Load<Item>("Items/Dirt");

        Inventory inv = GameObject.FindObjectOfType<Inventory>();


        inv.AddItem(hammer, 1, 0);
        inv.AddItem(pickaxe, 1, 1);
        inv.AddItem(knife, 1, 2);


        inv.AddItem(wood, 40);
        inv.AddItem(rope, 30);
        inv.AddItem(stone, 40);
        inv.AddItem(dirt, 30);
        inv.AddItem(water, 40);
        
    }
}
