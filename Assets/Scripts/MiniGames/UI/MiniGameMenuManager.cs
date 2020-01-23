using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameMenuManager : MenuManager
{
    MiniGameManager miniGameManager;


    protected override void Start()
    {
        
        MainManager = GameObject.FindObjectOfType<MainManager>();
        miniGameManager = GameObject.FindObjectOfType<MiniGameManager>();

#if UNITY_EDITOR
        if (MainManager == null)
        {
            MainManager = new MainManager();

        }

#endif

        MenuDefault = new List<GameObject>(MenuList)[0];
        Open();
    }




    public override void Close()
    {
        IsOpened = false;
        HideAll();
        miniGameManager.Paused = false;
    }

    public override void Open()
    {
        IsOpened = true;
        miniGameManager.Paused = true;
        HideAll();
        Open(MenuDefault);
    }

    /**
     * Quit the mini game
     * */
    public void Quit()
    {
        MessageBox.Show(MessageBox.Types.YesNo, "Quit the mini game?", OnQuitOk, null);
    }

    void OnQuitOk()
    {
        MainManager.ExitMiniGame();
    }
}
