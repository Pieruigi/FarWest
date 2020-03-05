using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestGhost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeInOut()
    {
        yield return new WaitForSeconds(5);

        GetComponent<FadeInOutAlpha>().FadeIn();

        yield return new WaitForSeconds(5);

        Debug.Log("FadeOUt");

        GetComponent<FadeInOutAlpha>().FadeOut();

        yield return new WaitForSeconds(3);

        Destroy(transform.root.gameObject);
    }
}
