using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SS
{
    public class LootAction : Action
    {
        public UnityAction<LootAction> OnExhausted;
        public UnityAction<LootAction> OnRestored;

        [Header("Loot Section")]
        [SerializeField]
        GameObject lootObject;

        [SerializeField]
        float lootVerticalOffset = 0.5f;

        [SerializeField]
        Item equipment;

        [SerializeField]
        float equipmentMultiplier = 0.5f;

        [SerializeField]
        int equipmentAnimationId;

        [SerializeField]
        int lootMax = 10; // Quantity of loot left each time the action is done
        protected int LootMax
        {
            get { return lootMax; }
        } 

        //[SerializeField]
        int lootCurrent;
        public int LootCurrent
        {
            get { return lootCurrent; }
            set { lootCurrent = value; }
        }

        [SerializeField]
        int lootUnit = 1;
        public int LootUnit
        {
            get { return lootUnit; }
        }

        [SerializeField]
        float growingTime = 10; // How much time it needs to a loot unit to grow up ( in seconds )

        [Header("Loot Spawner Section")]
        [SerializeField]
        Vector3 forceMul = Vector3.one;

        [SerializeField]
        Vector3 torqueMul = Vector3.one;

        float growingElapsed = 0;
        public float GrowingElapsed
        {
            get { return growingElapsed; }
            set { growingElapsed = value; }
        }

        //int equipmentMultiplier = 4;

        protected override void Awake()
        {
            lootCurrent = lootMax;
            //parentColl = transform.parent.GetComponent<Collider>();
            base.Awake();

        }

        protected override void Start()
        {
            base.Start();
        }


        protected override void Update()
        {
            // Grow up
            if (lootCurrent == 0)
            {
                growingElapsed += Time.deltaTime;

                if (growingElapsed > growingTime)
                {
                    growingElapsed = 0;
                    lootCurrent = lootMax;

                    OnRestored?.Invoke(this);
                }
            }

            base.Update();
        }

        public float GetHealthNormalized()
        {
            float health = 0;
            
            int step = LootMax / lootUnit - LootCurrent / lootUnit;

            health = ( DoSomethingDelay * LootMax / lootUnit ) - ( step * DoSomethingDelay ) - Elapsed;

            health = health / (DoSomethingDelay * LootMax / lootUnit);

            return health;
        }

        public override void StartExecuting()
        {
            int animIdDef = OnEnterAnimationId;
            if (PlayerController.Equipped == equipment)
                OnEnterAnimationId = equipmentAnimationId;
            base.StartExecuting();
            if (PlayerController.Equipped == equipment)
            {
                OnEnterAnimationId = animIdDef;
                //DoSomethingDelay *= equipmentMultiplier;
                SpeedMultiplier *= equipmentMultiplier;
            }

          
        }

        public override bool CanBeDone()
        {
            if (lootCurrent <= 0)
                return false;

            if (PlayerController.Equipped != null && PlayerController.Equipped != equipment)
                return false;

            return true;
        }

        public override bool DoSomething()
        {
            if (lootCurrent > 0)
            {

                // Try to loot
                int toLoot = lootUnit;
                //if (equipment != null && PlayerController.Equipped == equipment)
                //{
                //    toLoot *= equipmentMultiplier;
                //}

                if (lootCurrent >= toLoot)
                {
                    lootCurrent -= toLoot;
                }
                else
                {
                    toLoot = lootCurrent;
                    lootCurrent = 0;
                    growingElapsed = 0;



                }

                Loot(toLoot);

            }

            if (lootCurrent == 0)
            {
                OnExhausted?.Invoke(this);

                StopExecuting();
            }


            return true;
        }

        protected virtual void Loot(int count)
        {
            StartCoroutine(DoLoot(count));
        }

        private IEnumerator DoLoot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
                GameObject obj = GameObject.Instantiate(lootObject, transform.position + new Vector3(0, lootVerticalOffset, 0), Quaternion.identity);

                Collider[] colls = transform.parent.GetComponentsInChildren<Collider>();
                foreach(Collider coll in colls)
                {
                    Physics.IgnoreCollision(coll, obj.GetComponent<Collider>(), true);
                }
                
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.AddForce(Random.Range(-5f*forceMul.x, 5f * forceMul.x), Random.Range(6f * forceMul.y, 10f * forceMul.y), Random.Range(-5f * forceMul.z, 5f * forceMul.z), ForceMode.VelocityChange);
                rb.AddTorque(Random.Range(-30f*torqueMul.x, 30*torqueMul.x), Random.Range(-30f*torqueMul.y, 30f*torqueMul.y), Random.Range(-30f*torqueMul.z, 30f*torqueMul.z), ForceMode.VelocityChange);
                //rb.AddTorque(0, 0, 0, ForceMode.VelocityChange);

                yield return new WaitForSeconds(1f);
                foreach (Collider coll in colls)
                {
                    Physics.IgnoreCollision(coll, obj.GetComponent<Collider>(), false);
                }

            }

            //if (lootCurrent == 0)
            //    OnExhausted?.Invoke(this);

        }



    }

}
