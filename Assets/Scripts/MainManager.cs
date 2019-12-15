//#define FORCE_SCREENSAVER  // For test on editor
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Name convention:
// The game executable is fileName.exe
// The screen saver is fileName.scr
//

public class MainManager : MonoBehaviour
{
    public const string regKeyScreenSaver = "SCRNSAVE.EXE";
    public const string regPathScreenSaver = "\"HKEY_CURRENT_USER\\Control Panel\\Desktop\"";
    public const string regKeyScreenSaverTimeOut = "ScreenSaveTimeOut";

    public const string scrFilePattern = "_scr.exe";

    bool isScreenSaver = false;
    public bool IsScreenSaver
    {
        get { return isScreenSaver; }
    }

    private string appPath;

    string appFileName;

    string appFolderName;

    bool isPlaying = false;
    public bool IsPlaying
    {
        get { return isPlaying; }
        set { isPlaying = value; autoSaveElapsed = 0; }
    }
    float autoSaveElapsed = 0;
    float autoSaveTime = 180;

    private void Awake()
    {
        Debug.Log("cmd-line:" + System.Environment.CommandLine);
        appFolderName = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")); // The folder where the game has been installed
        appFileName = System.Environment.CommandLine; // The name of the executable without extension

        if (appFileName.Contains(".exe ") && !Application.isEditor)
        {
            isScreenSaver = true;
            appFileName = appFileName.Substring(0, appFileName.IndexOf(".exe "));
            appFileName += ".exe";
        }
        else
        {
            isScreenSaver = false;
        }

        //if (appFileName.ToLower().EndsWith(scrFilePattern))
        //    isScreenSaver = true;
        //else
        //    isScreenSaver = false;

        appFileName = appFileName.Substring(appFileName.LastIndexOf("\\")+1);
        appFileName = appFileName.Substring(0, appFileName.LastIndexOf("."));

        if (isScreenSaver)
            appFileName = appFileName.Replace(scrFilePattern,"");

#if FORCE_SCREENSAVER
        isScreenSaver = true; 
#endif

        // Load item from resources
        ItemCollection.Create(Constants.PathAssetItem);
        RecipeCollection.Create(Constants.PathAssetRecipes);


        // Load data from save file or cloud
#if UNITY_EDITOR
        CacheManager.Create(Application.persistentDataPath + "/sav_editor.txt");
#else
        CacheManager.Create(Application.persistentDataPath + "/sav.txt");
#endif

        ProfileCacheManager.Create(Application.persistentDataPath + "/prf.txt");
        CacheManager.Instance.Load();
        ProfileCacheManager.Instance.Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isScreenSaver)
        {
            // Disable game menu
            GameObject.FindObjectOfType<MainMenu>().gameObject.SetActive(false);

            // Disable cursor
            Cursor.visible = false;

            // Set native resolution
            SetResolutionForScreenSaver();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isScreenSaver)
        {
            if (Input.anyKeyDown)
                Application.Quit();

                if (Input.GetAxis("Mouse X")!=0 || Input.GetAxis("Mouse Y") != 0)
                Application.Quit();

        }
        else
        {
            if (autoSaveElapsed < autoSaveTime)
            {
                autoSaveElapsed += Time.deltaTime;
            }
            else
            {
                autoSaveElapsed = 0;
                CacheManager.Instance.Save();
            }

        }
    }

    public bool IsScreenSaverEnabled()
    {
        string cmd = string.Format("/c reg query {1} /v {0}", regKeyScreenSaver, regPathScreenSaver);
        //var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        //procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //procInfo.RedirectStandardOutput = true;
        //procInfo.UseShellExecute = false;

        //var proc = System.Diagnostics.Process.Start(procInfo);
        var proc = ExecuteCommand(cmd);

        string ret = proc.StandardOutput.ReadToEnd();

        proc.WaitForExit();

        if (proc.ExitCode != 0)
            return false;

        if (!ret.Contains(appFileName + ".scr"))
            return false;

        return true;

        
    }

    public bool EnableScreenSaver()
    {
        //string filePath = "\"" + appFolderName.Replace("\\", "/") + "/" + appFileName + ".scr\"";
        string filePath = appFolderName + "\\" + appFileName + ".scr";

        System.Text.StringBuilder strBuild = new System.Text.StringBuilder(65000);
        int ret = Utility.GetShortPathName(filePath, strBuild, strBuild.Capacity);

        if (ret <= 0)
            return false;

        //string cmd = string.Format("/c reg add {3} /v {2} /t REG_SZ /d {0}/{1}.scr /f", appFolderName, appFileName, regKeyScreenSaver, regPathScreenSaver);
        string cmd = string.Format("/c reg add {2} /v {1} /t REG_SZ /d {0} /f", strBuild.Replace("\\", "/").ToString(), regKeyScreenSaver, regPathScreenSaver);

        Debug.Log("CMD:" + cmd);
        //return false;
        //var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        //procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        //var proc = System.Diagnostics.Process.Start(procInfo);
        var proc = ExecuteCommand(cmd);

        proc.WaitForExit();

        if (proc.ExitCode == 0)
            return true;
        else
            return false;
    }

    public bool DisableScreenSaver()
    {
        string cmd = string.Format("/c reg delete {1} /v {0} /f", regKeyScreenSaver, regPathScreenSaver);

        //var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        //procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        //var proc = System.Diagnostics.Process.Start(procInfo);
        var proc = ExecuteCommand(cmd);

        proc.WaitForExit();

        if (proc.ExitCode == 0)
            return true;
        else
            return false;
    }

    public bool SetScreenSaverTimeOut(int timeOut)
    {
        //reg add "HKEY_CURRENT_USER\Control Panel\Desktop" /v ScreenSaveTimeOut /t REG_SZ /d 600 /f
        string cmd = string.Format("/c reg add {2} /v {1} /t REG_SZ /d {0} /f", timeOut, regKeyScreenSaverTimeOut, regPathScreenSaver);

        Debug.Log("CMD:" + cmd);
        //var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        //procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        //var proc = System.Diagnostics.Process.Start(procInfo);
        var proc = ExecuteCommand(cmd);

        proc.WaitForExit();

        if (proc.ExitCode == 0)
            return true;
        else
            return false;

    }
    public int GetScreenSaverTimeOut()
    {
        string cmd = string.Format("/c reg query {1} /v {0}", regKeyScreenSaverTimeOut, regPathScreenSaver);

        //var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        //procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //procInfo.RedirectStandardOutput = true;
        //procInfo.UseShellExecute = false;

        //var proc = System.Diagnostics.Process.Start(procInfo);
        var proc = ExecuteCommand(cmd);

        string ret = proc.StandardOutput.ReadToEnd();

        proc.WaitForExit();

        if (proc.ExitCode != 0)
            return -1;

        // Ret is something like this: HKEY_CURRENT_USER\Control Panel\Desktop ScreenSaveTimeOut REG_SZ    34
        ret = ret.Substring(ret.LastIndexOf(" "));
        ret = ret.Trim();

        int retInt = 0;
        int.TryParse(ret, out retInt);
        
        return retInt;


    }

    public void ApplicationQuit()
    {
        CacheManager.Instance.Save();
        Application.Quit();
    }

    public void ResetGame()
    {
        

        MessageBox.Show(MessageBox.Types.YesNo, "All your progress will be lost! Are you sure you want to procede?", () => DoResetGame());
    }

    private void SetResolutionForScreenSaver()
    {
        Resolution res = Screen.resolutions[Screen.resolutions.Length-1];
        //Resolution curr = Screen.currentResolution;
        Debug.Log("Res:" + Screen.currentResolution);
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow, res.refreshRate);
    }

    private System.Diagnostics.Process ExecuteCommand(string cmd)
    {
        var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        procInfo.RedirectStandardOutput = true;
        procInfo.UseShellExecute = false;

        var proc = System.Diagnostics.Process.Start(procInfo);

        return proc;
    }

    void DoResetGame()
    {
        CacheManager.Instance.Clear();

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

       
    }
}
