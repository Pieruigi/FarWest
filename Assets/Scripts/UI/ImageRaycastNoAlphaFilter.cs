using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRaycastNoAlphaFilter : MonoBehaviour, ICanvasRaycastFilter
{

    private Collider2D coll;
    private RectTransform trans;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        coll = GetComponent<Collider2D>();
        trans = GetComponent<RectTransform>();
    }

    public bool IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
    {

        Vector2 localPoint = Vector2.zero;

        // Get the position inside the sprite rectangle
        bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            trans,
            screenPos,
            eventCamera,
            out localPoint
        );

        if (isInside)
        {
            //
            // Transform coordinates
            //
            float rectWidth = (transform as RectTransform).rect.width;
            float rectHeight = (transform as RectTransform).rect.height;

            float x = localPoint.x + rectWidth / 2f;
            float y = localPoint.y + rectHeight / 2f;

            // 
            // Adjust ibn texture coordinates
            //
            x *= image.sprite.texture.width / rectWidth;
            y *= image.sprite.texture.height / rectHeight;

            Color c = image.sprite.texture.GetPixel((int)x, (int)y);

            return c.a > 0;

        }



        return isInside;
    }
}
