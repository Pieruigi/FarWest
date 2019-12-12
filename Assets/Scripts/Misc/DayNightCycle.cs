using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
   
    public static int NumberOfSecondsInOneDay = 24 * 60 * 60; // How many seconds in one day

    [SerializeField]
    float speedMul = 1;

    [Header("Ambient Light")]
    [SerializeField]
    Color[] skyColors;

    [SerializeField]
    Color[] equatorColors;

    [SerializeField]
    Color[] groundColors;

    [Header("SkyBoxShader")]
    [SerializeField]
    float[] sunSizeArray;

    [SerializeField]
    float[] sunSizeConvergenceArray;

    [SerializeField]
    float[] atmosphereThicknessArray;

    [SerializeField]
    Color[] skyTintArray;

    [SerializeField]
    Color[] groundArray;

    [SerializeField]
    float[] exposureArray;

    [Header("Sun")]
    [SerializeField]
    float[] sunLightIntensityArray;

    [SerializeField]
    Color[] sunLightColorArray;

    [Header("Moon")]
    [SerializeField]
    GameObject moon;

    [SerializeField]
    Color[] moonColors; // Color depends on the sun position


    public float SpeedMultiplier
    {
        get { return speedMul; }
        set { speedMul = value; }
    }

    float dayTimeInSec;
    public int DayTimeInSeconds
    {
        get { return (int)Mathf.Round(dayTimeInSec); }
    }

    float rotSpeed = 1f / 240f; // 90 degrees in 6 hours

    //float elapsed = 0;

    //float colorSky, colorEquator, colorGround;

    LTDescr ltDescColorSky, ltDescColorEquator, ltDescColorGround;
    LTDescr ltDescSunSize, ltDescAtmThick, ltDescSkyTint, ltDescExposure;

    Light sunLight;
    float sunLightPowerTime, sunLightColorTime;
    LTDescr ltDescSunLightPower, ltDescSunLightColor;

    LTDescr ltDescMoonRevolution;
    LTDescr ltDescMoonColor;

    float time;

    private float moonRevolutionInSeconds = 29f * NumberOfSecondsInOneDay;
    float monthTimeInSec;
    float moonTime;
    float moonRevolutionSpeed;

   
    float moonColorTime;

    string sunSizePropertyName = "_SunSize";
    string sunSizeConvergencePropertyName = "_SunSizeConvergence";
    string atmosphereThicknessPropertyName = "_AtmosphereThickness";
    string skyTintPropertyName = "_SkyTint";
    string groundColorPropertyName = "_GroundColor";
    string exposurePropertyName = "_Exposure";

    private void Awake()
    {
        if (skyColors.Length != equatorColors.Length || skyColors.Length != groundColors.Length)
            throw new System.Exception("DayNightCycle error - skyColors, equatorColors and groundColors can not have different length.");

        // Calculate day time in seconds ( depending on the speed daytime may be different from the current daytime )
        DateTime dt = DateTime.Now;
        dayTimeInSec = (float)(dt.TimeOfDay.TotalSeconds);// * speedMul);
        dayTimeInSec = dayTimeInSec % NumberOfSecondsInOneDay;

        //dayTimeInSec = 81000; // 21.600: 06, 43.200: 12; 18: 64.800 ////////////////////////// TO REMOVE ////////////////////////

        // Sets and starts rotating the sun
        float angle = (dayTimeInSec * 360f / NumberOfSecondsInOneDay) + 270f;
        transform.Rotate(Vector3.right, angle);

        // Set ambient light
        int timeSegmentLength = NumberOfSecondsInOneDay / skyColors.Length;
        float passed = (dayTimeInSec % timeSegmentLength) / speedMul;

        time = NumberOfSecondsInOneDay / skyColors.Length / speedMul; // LeanTween interpolation duration in seconds
        int fromId, toId;
        GetColorIds(dayTimeInSec, skyColors.Length, out fromId, out toId);
        ltDescColorSky = LeanTween.value(gameObject, OnColorSkyUpdate, skyColors[fromId], skyColors[toId], time);
        ltDescColorSky.passed = passed;
        ltDescColorEquator = LeanTween.value(gameObject, OnColorEquatorUpdate, equatorColors[fromId], equatorColors[toId], time);
        ltDescColorEquator.passed = passed;
        ltDescColorGround = LeanTween.value(gameObject, OnColorGroundUpdate, groundColors[fromId], groundColors[toId], time);
        ltDescColorGround.passed = passed;

        // Set skybox shader parameters
        if (sunSizeArray.Length > 0)
            ltDescSunSize = LeanTween.value(gameObject, OnSunSizeUpdate, sunSizeArray[fromId], sunSizeArray[toId], time);

        if (atmosphereThicknessArray.Length > 0)
        {
            ltDescAtmThick = LeanTween.value(gameObject, OnAtmosphereThicknessUpdate, atmosphereThicknessArray[fromId], atmosphereThicknessArray[toId], time);
            ltDescAtmThick.passed = passed;
        }
        if (skyTintArray.Length > 0)
        {
            ltDescSkyTint = LeanTween.value(gameObject, OnSkyTintUpdate, skyTintArray[fromId], skyTintArray[toId], time);
            ltDescSkyTint.passed = passed;
        }
        if (exposureArray.Length > 0)
        {
            ltDescExposure = LeanTween.value(gameObject, OnExposureUpdate, exposureArray[fromId], exposureArray[toId], time);
            ltDescExposure.passed = passed;
        }

        // Set sun light
        sunLight = GetComponent<Light>();
        if(sunLightIntensityArray != null)
        {
            sunLightPowerTime = NumberOfSecondsInOneDay / sunLightIntensityArray.Length / speedMul;
            GetFloatIds(dayTimeInSec, sunLightIntensityArray.Length, out fromId, out toId);
            Debug.Log(string.Format("SuLightInit:{0},{1}", fromId, toId));
            ltDescSunLightPower = LeanTween.value(gameObject, OnSunLightPowerUpdate, sunLightIntensityArray[fromId], sunLightIntensityArray[toId], sunLightPowerTime);
            ltDescSunLightPower.passed = passed;
        }
        if (sunLightColorArray != null)
        {
            sunLightColorTime = NumberOfSecondsInOneDay / sunLightColorArray.Length / speedMul;
            GetColorIds(dayTimeInSec, sunLightColorArray.Length, out fromId, out toId);
            ltDescSunLightColor = LeanTween.value(gameObject, OnSunLightColorUpdate, sunLightColorArray[fromId], sunLightColorArray[toId], sunLightColorTime);
            ltDescSunLightColor.passed = passed;
        }


        // Set up moon
        if (moon)
        {
            
            moonTime = moonRevolutionInSeconds / speedMul;
            moonRevolutionSpeed = 360f / moonTime;
            //ltDescMoonRevolution = LeanTween.value(gameObject, OnMoonRevolutionUpdate, 0, 360, moonTime);

            float t = NumberOfSecondsInOneDay / moonColors.Length / speedMul;

            GetColorIds(dayTimeInSec, moonColors.Length, out fromId, out toId);
            moonColorTime = NumberOfSecondsInOneDay / moonColors.Length / speedMul;
            ltDescMoonColor = LeanTween.value(moon.gameObject, OnMoonColorUpdate, moonColors[fromId], moonColors[toId], t);
            ltDescMoonColor.passed = passed;
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        //CacheManager.OnSave += HandleOnSave;

        //float.TryParse(CacheManager.GetValue(Constants.CacheKeyTimeElapsed), out timeElapsed);

    }

    // Update is called once per frame
    void Update()
    {
        int timeSegmentLength = NumberOfSecondsInOneDay / skyColors.Length;
        float passed = (dayTimeInSec % timeSegmentLength) / speedMul;

        //timeElapsed += Time.deltaTime * speedMul;

        dayTimeInSec += Time.deltaTime * speedMul;
        dayTimeInSec = dayTimeInSec % NumberOfSecondsInOneDay;

        // Rotate the sun and starts
        transform.Rotate(Vector3.right, rotSpeed * Time.deltaTime * speedMul);

        if (!LeanTween.isTweening() || !LeanTween.isTweening(ltDescColorSky.id))
        {
            //LeanTween.cancelAll();
            LeanTween.cancel(ltDescColorSky.id);
            LeanTween.cancel(ltDescColorEquator.id);
            LeanTween.cancel(ltDescColorGround.id);
            LeanTween.cancel(ltDescAtmThick.id);
            LeanTween.cancel(ltDescSkyTint.id);
            LeanTween.cancel(ltDescExposure.id);

            int fromId, toId;
            GetColorIds(dayTimeInSec, skyColors.Length, out fromId, out toId);
            ltDescColorSky = LeanTween.value(gameObject, OnColorSkyUpdate, skyColors[fromId], skyColors[toId], time);
            ltDescColorEquator = LeanTween.value(gameObject, OnColorEquatorUpdate, equatorColors[fromId], equatorColors[toId], time);
            ltDescColorGround = LeanTween.value(gameObject, OnColorGroundUpdate, groundColors[fromId], groundColors[toId], time);

            ltDescColorSky.passed = passed;
            ltDescColorEquator.passed = passed;
            ltDescColorGround.passed = passed;

            if (atmosphereThicknessArray.Length > 0)
            {
                ltDescAtmThick = LeanTween.value(gameObject, OnAtmosphereThicknessUpdate, atmosphereThicknessArray[fromId], atmosphereThicknessArray[toId], time);
                ltDescAtmThick.passed = passed;
            }
                
            if (skyTintArray.Length > 0)
            {
                ltDescSkyTint = LeanTween.value(gameObject, OnSkyTintUpdate, skyTintArray[fromId], skyTintArray[toId], time);
                ltDescSkyTint.passed = passed;
            }
                
            if (exposureArray.Length > 0)
            {
                ltDescExposure = LeanTween.value(gameObject, OnExposureUpdate, exposureArray[fromId], exposureArray[toId], time);
                ltDescExposure.passed = passed;
            }
                


           
        }

        if (!LeanTween.isTweening() || !LeanTween.isTweening(ltDescSunLightPower.id))
        {
            LeanTween.cancel(ltDescSunLightPower.id);
            int fromId, toId;
            GetFloatIds(dayTimeInSec, sunLightIntensityArray.Length, out fromId, out toId);
            ltDescSunLightPower = LeanTween.value(gameObject, OnSunLightPowerUpdate, sunLightIntensityArray[fromId], sunLightIntensityArray[toId], sunLightPowerTime);
            ltDescSunLightPower.passed = passed;
        }

        if (!LeanTween.isTweening() || !LeanTween.isTweening(ltDescSunLightColor.id))
        {
            LeanTween.cancel(ltDescSunLightColor.id);
            int fromId, toId;
            GetColorIds(dayTimeInSec, sunLightColorArray.Length, out fromId, out toId);
            ltDescSunLightColor = LeanTween.value(gameObject, OnSunLightColorUpdate, sunLightColorArray[fromId], sunLightColorArray[toId], sunLightColorTime);
            ltDescSunLightColor.passed = passed;
        }

        //if(!LeanTween.isTweening() || !LeanTween.isTweening(ltDescMoonRevolution.id))
        //{
        //    LeanTween.cancel(ltDescMoonRevolution.id);
        //    ltDescMoonRevolution = LeanTween.value(gameObject, OnMoonRevolutionUpdate, 0, 360, moonTime);
        //}

        if (moon)
        {
            moon.transform.Rotate(Vector3.right, moonRevolutionSpeed * Time.deltaTime);
        }

        if (!LeanTween.isTweening() || !LeanTween.isTweening(ltDescMoonColor.id))
        {
            LeanTween.cancel(ltDescMoonColor.id);
            int fromId, toId;
            GetColorIds(dayTimeInSec, moonColors.Length, out fromId, out toId);
            float t = NumberOfSecondsInOneDay / moonColors.Length / speedMul;
            ltDescMoonColor = LeanTween.value(moon.gameObject, OnMoonColorUpdate, moonColors[fromId], moonColors[toId], t);
            ltDescMoonColor.passed = passed;
        }
       
    }


    void OnColorSkyUpdate(Color value)
    {
        RenderSettings.ambientSkyColor = value;
    }

    void OnColorEquatorUpdate(Color value)
    {

        RenderSettings.ambientEquatorColor = value;
    }

    void OnColorGroundUpdate(Color value)
    {

        RenderSettings.ambientGroundColor = value;
    }

    void OnSunSizeUpdate(float value)
    {
        RenderSettings.skybox.SetFloat("_SunSize", value);
    }

    void OnAtmosphereThicknessUpdate(float value)
    {
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", value);
    }

    void OnExposureUpdate(float value)
    {
        RenderSettings.skybox.SetFloat("_Exposure", value);
    }

    void OnSkyTintUpdate(Color value)
    {
        RenderSettings.skybox.SetColor("_SkyTint", value);
    }

    void OnSunLightPowerUpdate(float value)
    {
        sunLight.intensity = value;
    }

    void OnSunLightColorUpdate(Color value)
    {
        sunLight.color = value;
    }

    void OnMoonRevolutionUpdate(float value)
    {
        moon.transform.eulerAngles = Vector3.right * value;
    }

    void OnMoonColorUpdate(Color value)
    {
        moon.GetComponentInChildren<MeshRenderer>().material.color = value;
    }

    void GetColorIds(float dayTimeInSec, int arrayLenght, out int fromId, out int toId)
    {

        float t = NumberOfSecondsInOneDay / arrayLenght;
        int from = (int)(dayTimeInSec / t);
        int to = from + 1;
        if (to >= arrayLenght)
            to = 0;

        fromId = from;
        toId = to;

    }

    void GetFloatIds(float dayTimeInSec, int arrayLength, out int fromId, out int toId)
    {

        float t = NumberOfSecondsInOneDay / arrayLength;
        int from = (int)(dayTimeInSec / t);
        int to = from + 1;
        if (to >= arrayLength)
            to = 0;

        fromId = from;
        toId = to;

    }


}
