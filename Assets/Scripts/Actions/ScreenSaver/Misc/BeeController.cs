using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    float randDisp = 0.6f;

    float flyDispY = 0.05f;
    float flyDir = 1;
    float currFlyDispY;
    float flyDispYDef;
    float flySpeed = 1;

    bool idle = true;
    Vector3 target;
    float waitTimeMin = 2f;
    float waitTimeMax = 6f;
    float waitTime;
    bool waiting = false;


    bool attacking = false;
    float attackDist = 0.2f;
    float sqrAttackDist;
    float attackSpeed = 6;

    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        sqrAttackDist = attackDist * attackDist;
        flyDispYDef = flyDispY;
        transform.position = transform.parent.position + new Vector3(Random.Range(-randDisp, randDisp), Random.Range(-randDisp, randDisp), Random.Range(-randDisp, randDisp));
        ComputeFlyDispY();
        target = GetNextTarget();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //    Attack();

        float y = Time.deltaTime * flySpeed;

        //        transform.tran
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + flyDir * Vector3.up * y, flyDispY);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + flyDir * Vector3.up * currFlyDispY, y);
        flyDispY -= y;

        if (flyDispY <= 0)
        {
            ComputeFlyDispY();
            flyDir = (flyDir == 1 ? -1 : 1);
        }

        if (idle)
        {
            if (!waiting)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, 0.005f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position, Vector3.up), 10);
                if (transform.position == target)
                {
                    waiting = true;
                    waitTime = Random.Range(waitTimeMin, waitTimeMax);
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
                if(waitTime < 0)
                {
                    waiting = false; 
                    target = GetNextTarget();
                }

                
            }

            return;
        }

        if (attacking)
        {
            float delta = Time.deltaTime * attackSpeed;
            Vector3 target = player.transform.position + Vector3.up;
            transform.position = Vector3.MoveTowards(transform.position, target, delta);

            if((transform.position - target).sqrMagnitude < sqrAttackDist)
            {
                attacking = false;
                idle = true;
            }

            return;
        }
    }

    public void Attack()
    {
        idle = false;
        StartCoroutine(CoroutineAttack());
    }

    IEnumerator CoroutineAttack()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        attacking = true;
        
    }

    void ComputeFlyDispY()
    {
        currFlyDispY = flyDispYDef * Random.Range(0.8f, 1.2f);
        flyDispY = currFlyDispY;
    }

    Vector3 GetNextTarget()
    {
        float d = randDisp;
        return transform.parent.position + new Vector3(Random.Range(-d, d), Random.Range(-d, d), Random.Range(-d, d));
    }
}
