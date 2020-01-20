using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxUI : MonoBehaviour
{
    bool buildMode = false;

    MainManager mainManager;
    BuildingMaker buildingMaker;

    private void Awake()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        if (!mainManager.SandboxMode)
            Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        buildingMaker = GameObject.FindObjectOfType<BuildingMaker>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchBuildMode()
    {
        if (buildMode)
            CloseBuildMode();
        else
            OpenBuildMode();
    }

    public void OpenBuildMode()
    {
        if (buildMode)
            return;

        buildMode = true;
        buildingMaker.SetEnable(true);
    }

    public void CloseBuildMode()
    {
        if (!buildMode)
            return;

        buildMode = false;
        buildingMaker.SetEnable(false);
    }
}
