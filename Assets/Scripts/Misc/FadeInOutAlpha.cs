using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutAlpha : MonoBehaviour
{
    [SerializeField]
    bool startVisible = true;

    [SerializeField]
    float fadeTime = 3;

    Renderer rend;
    Material opaqueMat;
    Material transparentMat;

    bool isVisible;

    private void Awake()
    {
        isVisible = startVisible;

        rend = GetComponent<MeshRenderer>();
        if (!rend)
            rend = GetComponent<SkinnedMeshRenderer>();

        if (!rend)
        {
            Debug.LogWarning("No renderer has been found");
            return;
        }

        opaqueMat = rend.material;
        transparentMat = CreateTransparentMaterial();

        if (!startVisible)
        {
            SetTransparentMode();
            SetAlpha(0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       



        
    }

    // Update is called once per frame
    void Update()
    {
         
        
        
    }

    public void FadeIn()
    {
        LeanTween.value(gameObject, OnUpdateFadeIn, 0, 1, fadeTime);
    }

    public void FadeOut()
    {
        SetTransparentMode();
        LeanTween.value(gameObject, OnUpdateFadeOut, 1, 0, fadeTime);
    }

    private void OnUpdateFadeIn(float value)
    {
        SetAlpha(value); // Set transparent alpha just to reset starting color for the next fade out, eventually we need it

        if (value == 1)
        {
            SetOpaqueMode();
            isVisible = true;
        }
            
    }

    private void OnUpdateFadeOut(float value)
    {
        SetAlpha(value); // Set transparent alpha

        if (value == 0)
            isVisible = false;
    }

    private void SetTransparentMode()
    {
        rend.enabled = false;
        
        rend.material = transparentMat;
        
        rend.enabled = true;
    }

    private void SetOpaqueMode()
    {

        rend.enabled = false;
        rend.material = opaqueMat;
        rend.enabled = true;
    }

    private Material CreateTransparentMaterial()
    {
        Material m = new Material(rend.material);
        m.SetFloat("_Mode", 2);
        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        m.SetInt("_ZWrite", 1);
        m.DisableKeyword("_ALPHATEST_ON");
        m.EnableKeyword("_ALPHABLEND_ON");
        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        m.renderQueue = 3000;
        return m;
    }

    private void SetAlpha(float alpha)
    {
        Color c = rend.material.color;
        c.a = alpha;
        rend.material.color = c;
    }
}
