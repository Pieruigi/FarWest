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

    float minTime = 30;
    float maxTime = 120;
    float time = 0;

    Camera currentCamera;


#if FORCE_SS
    int testCam = 12;
#endif

    public Camera CurrentCamera
    {
        get { return currentCamera; }
    }

    bool switchingCamera = false;

    bool switchingDisabled = false;

    bool cameraCloseDisabled = false;

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

        int v = 0;
        int.TryParse(ProfileCacheManager.Instance.GetValue("CameraType"), out v);
        if (v == 0)
            switchingDisabled = false;
        else
            switchingDisabled = true;

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
            currentCamera = cameras[Random.Range(0, cameras.Count)];

#if FORCE_SS
            //currentCamera = cameras[testCam];
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

    IEnumerator SwitchCamera()
    {
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
