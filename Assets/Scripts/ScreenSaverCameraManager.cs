using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaverCameraManager : MonoBehaviour
{
    [SerializeField]
    List<Camera> cameras;
    public IList<Camera> Cameras
    {
        get { return cameras.AsReadOnly(); }
    }

    public int NumberOfCameras
    {
        get { return (cameras != null ?  cameras.Count : 0 ); }
    }

    FadeInOut fadeInOut;

#if FORCE_SS
    float minTime = 30;//30;
    float maxTime = 120;//120;
#else
    float minTime = 30;
    float maxTime = 120;
#endif


    float time = 0;

    Camera currentCamera;


#if FORCE_SS
    int testCam = 9;
#endif

    public Camera CurrentCamera
    {
        get { return currentCamera; }
    }

    bool switchingCamera = false;

    bool switchingDisabled = false;
    public bool SwitchingDisabled
    {
        get { return switchingDisabled; }
        set { switchingDisabled = value; }
    }
   
    bool cameraCloseDisabled = false;

    int cameraType = -1;

    Camera[] cameraListDefault;

    MainManager mainManager;
    public bool CameraCloseDisabled
    {
        get { return cameraCloseDisabled; }
        set // Setting true will not force the camera to swtich
        {
            cameraCloseDisabled = value;
            //if (cameraCloseExcluded && currentCamera.tag == "CameraClose")
            //    SwitchCamera();
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        

        mainManager = GameObject.FindObjectOfType<MainManager>();
        //cameras = new List<Camera>(GetComponentsInChildren<Camera>());

        int.TryParse(ProfileCacheManager.Instance.GetValue("CameraType"), out cameraType);
        cameraType--; // We saved the option id, but we need the option value, that il optionId-1 in this case

#if FORCE_SS
        cameraType = -1;
#endif

        if (cameraType < 0)
        {
            switchingDisabled = false;
        }
        else
        {
            switchingDisabled = true;
        }
            

        if (!mainManager.IsScreenSaver)
        {
            //Destroy(gameObject);
            foreach (Camera c in cameras)
                c.gameObject.SetActive(false);
        }
        else
        {
            //bool.TryParse(CacheManager.Instance.GetValue("CameraType"), out switchingDisabled);
            
            fadeInOut = GameObject.FindObjectOfType<FadeInOut>();

            // Load and disable all screen saver cameras
            
            foreach(Camera c in cameras)
            {
                c.gameObject.SetActive(false);
            }

            // Setting random camera
            time = Random.Range(minTime, maxTime);
            if(cameraType < 0)
            {
                currentCamera = cameras[Random.Range(0, cameras.Count)];
            }
            else
            {
                currentCamera = cameras[cameraType];
                cameras.RemoveAll(c => c != currentCamera);
            }
            //if(cameraType < 0)

            //else
            //{
            //    currentCamera = cameras[cameraType];
            //}
            cameraListDefault = new Camera[cameras.Count];
            cameras.CopyTo(cameraListDefault);

            Debug.Log("CurrentCamera:" + currentCamera);

#if FORCE_SS
            currentCamera = cameras[testCam];
#endif
            currentCamera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainManager.IsScreenSaver || switchingDisabled || switchingCamera)
            return;

        
        time -= Time.deltaTime;
        if(time < 0)
        {
            StartCoroutine(SwitchCamera());
        }
    }

    public void ForceSwitchCamera()
    {
        StartCoroutine(SwitchCamera());
    }

    /**
     * Used by some sketches to set specific cameras.
     * */
    public void ReplaceCameraList(List<Camera> newList)
    {
        cameras = newList;
        //switchingDisabled = false;
        ForceSwitchCamera();
    }

    /**
     * Call it if you want to reset the default camera list: for example when a sketch has completed.
     * */
    public void ResetCameraList()
    {
        cameras = new List<Camera>(cameraListDefault);
        ForceSwitchCamera();
    }

    IEnumerator SwitchCamera()
    {
        if (fadeInOut == null)
            yield break;

        List<Camera> tmp = cameras.FindAll(c => !cameraCloseDisabled || !Constants.TagCameraClose.Equals(c.tag));

        Camera newCam = tmp[Random.Range(0, tmp.Count)];

#if FORCE_SS
        //newCam = tmp[testCam];
#endif

        if (newCam == currentCamera)
            yield break;

        switchingCamera = true;
        yield return fadeInOut.FadeOut();

        currentCamera.gameObject.SetActive(false);
        currentCamera = newCam;
        currentCamera.gameObject.SetActive(true);

        time = Random.Range(minTime, maxTime);

        yield return fadeInOut.FadeIn();
        switchingCamera = false;
    }

}
