using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class Rock : MonoBehaviour
    {
        [SerializeField]
        LootAction lootAction;

        [SerializeField]
        AudioSource source;

        [SerializeField]
        AudioClip fallingClip;

        [SerializeField]
        AudioClip growingClip;

        private void Awake()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            lootAction.OnExhausted += HandleOnExhausted;
            lootAction.OnRestored += HandleOnRestored;

        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnExhausted(LootAction lootAction)
        {
            StartCoroutine(CutDown());
        }

        void HandleOnRestored(LootAction lootAction)
        {
            if (source)
            {
                source.clip = growingClip;
                source.Play();
            }

            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            gameObject.SetActive(true);

            // Reset color
            Color c = GetComponent<MeshRenderer>().material.color;
            c.a = 1;
            LeanTween.color(gameObject, c, 0.5f);

            // Pop up
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, 1f).setEaseOutElastic();
        }

        private IEnumerator CutDown()
        {
            // Play sound
            if (source)
            {
                source.clip = fallingClip;
                source.Play();
            }
                

            yield return new WaitForSeconds(0.5f);

            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), Random.Range(-4f, 4f)), ForceMode.VelocityChange);

            // Fadeout
            yield return new WaitForSeconds(1);
            Color c = GetComponent<MeshRenderer>().material.color;
            c.a = 0;
            LeanTween.color(gameObject, c, 3f);

            // Disable
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);

        }
    }

}
