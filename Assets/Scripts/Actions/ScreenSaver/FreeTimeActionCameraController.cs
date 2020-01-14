using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeTimeActionCameraController : MonoBehaviour
{
    [SerializeField]
    List<Camera> cameraList;

    //[SerializeField]
    int defaultCameraId = 6;

    ScreenSaverCameraManager cameraManager;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = GameObject.FindObjectOfType<ScreenSaverCameraManager>();

        foreach (Camera c in cameraList)
        {
            c.gameObject.SetActive(false);
        }

        player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);
    }


    public void UpdateCameraList()
    {
        //List<GameObject> clipVolumes = new List<GameObject>(transform.parent.GetComponentsInChildren<GameObject>()).FindAll(g=>g.layer == LayerMask.NameToLayer("ClippingEver") || g.layer == LayerMask.NameToLayer("ClippingInside")) ;
        List<Transform> clipVolumes = new List<Transform>(transform.parent.GetComponentsInChildren<Transform>()).FindAll(g => g.gameObject.layer == LayerMask.NameToLayer("ClippingEver") || g.gameObject.layer == LayerMask.NameToLayer("ClippingInside"));

        foreach (Transform g in clipVolumes)
            g.gameObject.SetActive(false);

        List<Camera> tmp = new List<Camera>();
        Vector3 cPos;
        foreach(Camera c in cameraList)
        {

            if (!Utility.CameraIsClipped(player.transform.position, c.transform.position, out cPos))
                tmp.Add(c);
        }

        if (tmp.Count == 0)
            tmp.Add(cameraManager.Cameras[defaultCameraId]);

        cameraManager.ReplaceCameraList(tmp);
        cameraManager.SwitchingDisabled = false;

        foreach (Transform g in clipVolumes)
            g.gameObject.SetActive(true);
    }

    public void ResetCameraList()
    {
        StartCoroutine(DoResetCameraList());
    }

    IEnumerator DoResetCameraList()
    {
        cameraManager.SwitchingDisabled = true;
        cameraManager.ResetCameraList();

        yield return new WaitForSeconds(1);

        foreach (Camera c in cameraList)
            c.gameObject.SetActive(false);

        cameraManager.SwitchingDisabled = false;
    }
}
