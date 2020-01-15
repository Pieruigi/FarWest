using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class Tree : MonoBehaviour
    {

        [SerializeField]
        LootAction lootAction;

        [SerializeField]
        AudioSource source;

        [SerializeField]
        AudioClip fallingClip;

        [SerializeField]
        AudioClip growingClip;

        [SerializeField]
        ParticleSystem fallingLeafs;

        Transform parent;

        bool initialized = false;

        private void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log("Initialized:" + initialized);
            //lootAction.OnExhausted += HandleOnExhausted;
            //lootAction.OnRestored += HandleOnRestored;
            ////parent = transform.parent;

            //Debug.Log("Tree - actionLootCacher:" + lootAction.LootCurrent);

            //if (lootAction.LootCurrent == 0)
            //{
            //    GetComponent<Rigidbody>().isKinematic = true;
            //    //transform.parent = parent;
            //    transform.localPosition = Vector3.zero;
            //    transform.localRotation = Quaternion.identity;
            //    transform.localScale = Vector3.zero;
            //    gameObject.SetActive(false);
            //}

            StartCoroutine(DoInit());
        }


        // Update is called once per frame
        void Update()
        {
    
        }

        IEnumerator DoInit()
        {
            //yield return new WaitForSeconds(5);
            yield return null;
                      
            lootAction.OnExhausted += HandleOnExhausted;
            lootAction.OnRestored += HandleOnRestored;
            //parent = transform.parent;

            if (lootAction.LootCurrent == 0)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                //transform.parent = parent;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.zero;
                gameObject.SetActive(false);
            }
        }

        private void HandleOnRestored(LootAction arg0)
        {
            source.clip = growingClip;
            source.Play();

            // Enable and reset parent and position
            GetComponent<Rigidbody>().isKinematic = true;
            //transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            // Pop in
            LeanTween.scale(gameObject, Vector3.one, 1f).setEaseOutElastic();

            if(fallingLeafs)
                fallingLeafs.Play();

        }

        void HandleOnExhausted(LootAction arg0)
        {
            StartCoroutine(CutDown());
        }


        private IEnumerator CutDown()
        {
            // Play audio
            source.clip = fallingClip;
            source.PlayDelayed(0.5f);

            yield return new WaitForSeconds(0.5f);

            // Add force
            //parent = transform.parent;
            //transform.parent = null;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
         
            rb.AddForceAtPosition(Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * transform.forward * .8f, transform.position + new Vector3(0, 4, 0), ForceMode.VelocityChange);

            // Fadeout
            yield return new WaitForSeconds(6);
            LeanTween.scale(gameObject, Vector3.zero, 1f).setEaseOutElastic();

            // Disable
            yield return new WaitForSeconds(3f);
            
            gameObject.SetActive(false);
           
        }


    }

}
