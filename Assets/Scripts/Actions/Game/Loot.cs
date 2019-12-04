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

    float lifeTime = 10;

    bool isDying = false;

    // Start is called before the first frame update
    void Start()
    {
        sqrPickRange = pickRange * pickRange;

        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
        inventory = GameObject.FindObjectOfType<Inventory>();

        StartCoroutine(LifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying)
            return;

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
            PickUp();
        }
    }

    private void NoRoomForItem()
    {

    }

    void PickUp()
    {
        isPicking = true;

        inventory.AddItem(item);
        
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        pickMove = LeanTween.move(gameObject, player.transform.position + Vector3.up * upOffset, 1).setEaseInOutBounce();
        //LeanTween.scale(gameObject, Vector3.zero, 0.9f).setEaseInOutElastic();
        //yield return new WaitForSeconds(1f);
        //GameObject.Destroy(gameObject);
        StartCoroutine(Kill());
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        if (isPicking)
            yield break;

        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        isDying = true;
        LeanTween.scale(gameObject, Vector3.zero, 0.9f).setEaseInOutElastic();
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(gameObject);
    }
}
