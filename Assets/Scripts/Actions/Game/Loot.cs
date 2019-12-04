using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField]
    Item item;

    float pickRange = 4;

    GameObject player;

    bool isInside = false;

    Inventory inventory;

    float sqrPickRange;

    bool isPicking = false;

    LTDescr pickMove;

    float upOffset = 1;

    // Start is called before the first frame update
    void Start()
    {
        sqrPickRange = pickRange * pickRange;

        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPicking)
        {
            float sqrDist = (player.transform.position - transform.position).sqrMagnitude;

            if (sqrDist < sqrPickRange)
                isInside = true;
            else
                isInside = false;

            if (isInside)
                TryPickUp();
        }
        else
        {
            if (pickMove != null)
            {
                pickMove.to = player.transform.position + upOffset * Vector3.up;
            }
        }
        
    }

    private void TryPickUp()
    {
        if (inventory.NoRoomForItem(item))
        {
            NoRoomForItem();
        }
        else
        {
            StartCoroutine(PickUp());
        }
    }

    private void NoRoomForItem()
    {

    }

    IEnumerator PickUp()
    {
        isPicking = true;

        inventory.AddItem(item);
        
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        pickMove = LeanTween.move(gameObject, player.transform.position + Vector3.up * upOffset, 1).setEaseInOutElastic();
        LeanTween.scale(gameObject, Vector3.zero, 0.9f);
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
    }
}
