using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    //[ExecuteInEditMode]
    public class PickUpAction : Action
    {
        [Header("Custom Action")]
        [SerializeField]
        Item item;

        //[SerializeField]
        float lifeTime = 120;

        


        protected override void Update()
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime < 0)
                Destroy(transform.parent.gameObject);

            base.Update();
        }

        public override bool DoSomething()
        {
            StartCoroutine(PickUp());
            return true;
        }

        private IEnumerator PickUp()
        {
            //PlayerController.SetInputEnabled(false);

            Animator anim = PlayerController.GetComponent<Animator>();
            anim.SetTrigger("PickUp");

            yield return new WaitForSeconds(0.68f);

            if (GameObject.FindObjectOfType<Inventory>().AddItem(item))
            {
                Destroy(transform.parent.gameObject);
            }
                
        }
    }

}
