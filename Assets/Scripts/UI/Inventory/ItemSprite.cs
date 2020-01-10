using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSprite : MonoBehaviour
{

    [SerializeField]
    private Image icoImage;

    private Image maskImage;



    // Start is called before the first frame update
    void Awake()
    {
        maskImage = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIco(Sprite ico, Vector2 size)
    {
        icoImage.sprite = ico;

        //Debug.Log("Size:" + size);
        //Debug.Log("Parent:" + transform.parent.gameObject);
        (icoImage.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        (icoImage.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }

    public void SetIco(Sprite ico)
    {
        icoImage.sprite = ico;

    }

    public void Darken(bool value)
    {
        Color c;
        float mul = 0.5f;
        if (value)
        {
            c = Color.white * new Vector4 (mul, mul, mul, 1);

        }
        else
        {
            c = Color.white;
        }

        icoImage.color = c;
        maskImage.color = c;
    }

    public void SetSize(Vector2 size)
    {
       
        GetComponent<RectTransform>().sizeDelta = size;
        icoImage.rectTransform.sizeDelta = size;
    }
}
