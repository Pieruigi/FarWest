using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using System;

// Name convention:
// The game executable is fileName.exe
// The screen saver is fileName.scr
//

public class MainManager : MonoBehaviour
{
    /**
     * We need external SystemParametersInfo in order to update the winlogonUI after the 'screensavetimeout' has changed.
     * */
    const int SPI_SETSCREENSAVETIMEOUT = 15; // The windows param we want to update
    const int SPIF_UPDATEINIFILE = 0x01; 
    const int SPIF_SENDCHANGE = 0x02;
    [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
    public static extern bool SystemParametersInfo(int uiAction, int uiParam, IntPtr pvParam, int fWinIni);


    public const string regKeyScreenSaver = "SCRNSAVE.EXE";
    public const string regPathScreenSaver = "\"HKEY_CURRENT_USER\\Control Panel\\Desktop\"";
    public const string regKeyScreenSaverTimeOut = "ScreenSaveTimeOut";

    public const string scrFilePattern = "_scr.exe";

    bool isScreenSaver = false; // True if is running as screensaver, otherwise is false;
    public bool IsScreenSaver
    {
        get { return isScreenSaver; }
    }

    private bool isPlayingScreenSaverInGame = false; // True wheter the screensaver is running from the game or is not running at all

    bool isMiniGame = false;

    private bool sandboxMode = false;
    public bool SandboxMode
    {
        get { return sandboxMode; }
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

    static MainManager instance = null;
    public static MainManager Instance
    {
        get { return instance; }
    }

    bool exitDisabled = false;
    bool isLoading = false;
    public bool IsLoading
    {
        get { return isLoading; }
    }

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

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

            appFileName = appFileName.Substring(appFileName.LastIndexOf("\\") + 1);
            appFileName = appFileName.Substring(0, appFileName.LastIndexOf("."));

            if (isScreenSaver)
                appFileName = appFileName.Replace(scrFilePattern, "");

#if FORCE_SS
        isScreenSaver = true; 
#endif
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
            
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

            

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                GameObject.Destroy(gameObject);
            }

            // Load item from resources
            ItemCollection.Create(Constants.PathAssetItem);
            RecipeCollection.Create(Constants.PathAssetRecipes);

#if UNITY_EDITOR
            CacheManager.Create(Application.persistentDataPath + "/sav_editor.txt");
#else
        CacheManager.Create(Application.persistentDataPath + "/sav.txt");
#endif

            ProfileCacheManager.Create(Application.persistentDataPath + "/prf.txt");
           
            CacheManager.Instance.Load();
            ProfileCacheManager.Instance.Load();

        }

        //CacheUtility.DebugCache(CacheManager.Instance);
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
        if (isMiniGame)
            return;

        if (isScreenSaver)
        {
            if (exitDisabled)
                return;

            if (Input.anyKeyDown)
            {
                if (isPlayingScreenSaverInGame)
                    InGameStopSS();
                else
                    Application.Quit();
            }
                

            if (Input.GetAxis("Mouse X")!=0 || Input.GetAxis("Mouse Y") != 0)
            {
                if (isPlayingScreenSaverInGame)
                    InGameStopSS();
                else
                    Application.Quit();
            }
                

        }
        else
        {
#if UNITY_EDITOR
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    //InGamePlaySS();
            //    if (!sandboxMode)
            //        EnterSandboxMode();
            //    else
            //        ExitSandboxMode();
            //}
#endif
            // Autosave
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

    public void EnterSandboxMode()
    {
        CacheManager.Instance.Save();
        sandboxMode = true;
        isLoading = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void ExitSandboxMode()
    {
        CacheManager.Instance.Save();
        sandboxMode = false;
        isLoading = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void EnterMiniGame(int sceneIndex)
    {
        isMiniGame = true;
        isLoading = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void ExitMiniGame()
    {
        isMiniGame = false;
        isLoading = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
        string filePath = appFolderName + "\\" + appFileName + ".scr";

        System.Text.StringBuilder strBuild = new System.Text.StringBuilder(65000);
        int ret = Utility.GetShortPathName(filePath, strBuild, strBuild.Capacity);

        if (ret <= 0)
            return false;

        string cmd = string.Format("/c reg add {2} /v {1} /t REG_SZ /d {0} /f", strBuild.Replace("\\", "/").ToString(), regKeyScreenSaver, regPathScreenSaver);

        var proc = ExecuteCommand(cmd);

        proc.WaitForExit();

        if (proc.ExitCode == 0)
        {
            //UpdateUserParmeters();
            return true;
        }
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
        {
            //UpdateUserParmeters();
            return true;
        }
        else
            return false;
    }

   
    public bool SetScreenSaverTimeOut(int timeOut)
    {
        //reg add "HKEY_CURRENT_USER\Control Panel\Desktop" /v ScreenSaveTimeOut /t REG_SZ /d 600 /f
        string cmd = string.Format("/c reg add {2} /v {1} /t REG_SZ /d {0} /f", timeOut, regKeyScreenSaverTimeOut, regPathScreenSaver);

        var proc = ExecuteCommand(cmd);

        proc.WaitForExit();
       
        if (proc.ExitCode == 0)
        {
            IntPtr val = (IntPtr)timeOut;

            bool spiRet = SystemParametersInfo(SPI_SETSCREENSAVETIMEOUT, timeOut, val, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
           
       
            return true;
        }
        else
            return false;

    }
    public int GetScreenSaverTimeOut()
    {
        string cmd = string.Format("/c reg query {1} /v {0}", regKeyScreenSaverTimeOut, regPathScreenSaver);

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

    //private void UpdateUserParmeters()
    //{
    //    string cmd = string.Format("/c %SystemRoot%\\System32\\RUNDLL32.EXE user32.dll, UpdatePerUserSystemParameters");
    //    var proc = ExecuteCommand(cmd);
    //    proc.WaitForExit();
    //}

    public void ApplicationQuit()
    {
        if (IsScreenSaverEnabled())
        {
            ConfirmApplicationQuit();
        }
        else
        {
            MessageBox.Show(MessageBox.Types.YesNo, "Chico screensaver is disabled. Enabled it in the option menu.\nQuit anyway?", ConfirmApplicationQuit, null);
        }
    }

    private void ConfirmApplicationQuit()
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
        
        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow, res.refreshRate);
    }

    private System.Diagnostics.Process ExecuteCommand(string cmd)
    {
        var procInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", cmd);
        procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        procInfo.RedirectStandardOutput = true;
        procInfo.UseShellExecute = false;
        procInfo.Verb = "runas"; // Run as admin

        var proc = System.Diagnostics.Process.Start(procInfo);

        return proc;
    }

    void DoResetGame()
    {
        CacheManager.Instance.Clear();

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

       
    }

    public void InGamePlaySS()
    {
        if (IsScreenSaver)
            return;

        if (isPlayingScreenSaverInGame)
            return;

        CacheManager.Instance.Save();
        isPlayingScreenSaverInGame = true;
        isScreenSaver = true;
        isLoading = true;
        //exitDisabled = true;
        //StartCoroutine(StartExitDisableTimer());
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void InGameStopSS()
    {
        if (!IsScreenSaver)
            return;

        if (!isPlayingScreenSaverInGame)
            return;


        
        isPlayingScreenSaverInGame = false;
        isScreenSaver = false;
        isLoading = true;


        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        //UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/FakeLoadingScene");

    }

    IEnumerator StartExitDisableTimer()
    {
        exitDisabled = true;
        yield return new WaitForSeconds(1f);

        exitDisabled = false;
    }

    void HandleSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        isLoading = false;

        if(isPlayingScreenSaverInGame && isScreenSaver)
        {
            StartCoroutine(StartExitDisableTimer());
        }
        //CacheUtility.DebugCache(CacheManager.Instance);
    }

  
}
