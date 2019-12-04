using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    Image image;

    float time = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeIn()
    {
        Color c = Color.black;
        c.a = 0;
        LeanTween.color((RectTransform)image.transform, c, time);

        yield return new WaitForSeconds(time);
    }

    public IEnumerator FadeOut()
    {
        Color c = Color.black;
        c.a = 1;
        LeanTween.color((RectTransform)image.transform, c, time);

        yield return new WaitForSeconds(time);
    }
}
