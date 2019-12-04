using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class Liana : MonoBehaviour
    {
        [SerializeField]
        AudioClip growingClip;

        [SerializeField]
        AudioSource source;

        DirectLootAction lootAction;

        Vector3 localScaleDefault;

        ChicoFXController chicoFXController;

        private void Awake()
        {
            localScaleDefault = transform.localScale;
        }

        // Start is called before the first frame update
        void Start()
        {
            chicoFXController = GameObject.FindObjectOfType<ChicoFXController>();

            lootAction = transform.parent.GetComponentInChildren<DirectLootAction>();

            lootAction.OnResourceTaken += HandleOnResourceTaken;
            lootAction.OnRestored += HandleOnRestored;
                     
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnResourceTaken(DirectLootAction lootAction, GameObject resource)
        {
            Debug.Log("Resourcetaken");
            if(resource == gameObject)
            {
                StartCoroutine(Take());
            }
                
        }

        void HandleOnRestored(LootAction lootAction)
        {
            Debug.Log("Resource Restored");

            if (growingClip)
            {
                source.clip = growingClip;
                source.Play();
            }
            

            gameObject.SetActive(true);

            // Pop up
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, localScaleDefault, 1f).setEaseOutElastic();
        }

        IEnumerator Take()
        {
            
            float time = 1;
            LeanTween.scale(gameObject, Vector3.zero, time).setEaseInElastic();
            yield return new WaitForSeconds(time);

            if (chicoFXController.IsPlaying())
                chicoFXController.SendMessage("StopPlaying");

            gameObject.SetActive(false);
        }
    }

}
