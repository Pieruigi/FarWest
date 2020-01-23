using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{

    public class MiniGameAction : Action
    {
        [Header("MiniGame Conf")]
        [SerializeField]
        int sceneIndex; 

        public override bool DoSomething()
        {
            //UnityEngine.SceneManagement.SceneManager.loa
            //Debug.Log("Launching minigame...");
            GameObject.FindObjectOfType<MainManager>().EnterMiniGame(sceneIndex);
            return true;
        }
    }

}
