using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AntialiasingController : MonoBehaviour
{

    AntialiasingSettings antialiasingSettings;

    PostProcessLayer ppl;

    private void Awake()
    {
        ppl = GetComponent<PostProcessLayer>();
        antialiasingSettings = GameObject.FindObjectOfType<AntialiasingSettings>();

        antialiasingSettings.OnAntialiasingChanged += HandleOnAntialiasingChanged;

        SetAntialiasing(antialiasingSettings.Quality); // The antialiasingSettings might have already been set up
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleOnAntialiasingChanged(int quality)
    {
        SetAntialiasing(quality);
    }

    void SetAntialiasing(int quality)
    {
        switch (quality)
        {
            case 0:
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.None;
                break;

            case 1:
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                break;

            case 2:
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                break;

            case 3:
                ppl.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                break;
        }
    }
}
