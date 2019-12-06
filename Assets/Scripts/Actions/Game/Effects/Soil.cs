using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SS
{
    public class Soil : MonoBehaviour
    {

        [SerializeField]
        LootAction lootAction;

        float offset = -0.04f;

        private void Awake()
        {
            lootAction.OnExhausted += HandleOnExhausted;
            lootAction.OnRestored += HandleOnRestored;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (lootAction.LootCurrent == 0)
                transform.localPosition = new Vector3(transform.localPosition.x, offset, transform.localPosition.z);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnExhausted(LootAction lootAction)
        {
            StartCoroutine(DoHandleOnExhausted(lootAction));
        }

        IEnumerator DoHandleOnExhausted(LootAction lootAction)
        {
            yield return new WaitForSeconds(1);

            LeanTween.moveLocalY(gameObject, offset, 1).setEaseOutElastic();

        }

        void HandleOnRestored(LootAction lootAction)
        {
            LeanTween.moveLocalY(gameObject, 0, 1).setEaseOutElastic();
        }
    }

}
