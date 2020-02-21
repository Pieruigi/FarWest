using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class WashbasinAction : Action
    {
        [Header("Objects")]
        [SerializeField]
        GameObject foamBrushPrefab;

        [SerializeField]
        GameObject bladePrefab;

        [SerializeField]
        GameObject foamPrefab;

        [Header("Audio")]
        [SerializeField]
        List<AudioClip> brushClips;

        [SerializeField]
        List<AudioClip> shaveClips;

        [SerializeField]
        List<AudioClip> washClips;

        [SerializeField]
        AudioClip happyClip;


        Fluff fluff;

        bool isDoing = false;

        Transform hat;
        Transform handL;
        Transform handR;

        Transform head;

        Vector3 hatPosDefault;
        Vector3 hatRotDefault;

        GameObject tool;
        GameObject foam;

        AudioSource toolSource;
        ChicoFXController chicofx;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            GameObject player = GameObject.FindGameObjectWithTag(Constants.TagPlayer);

            fluff = player.GetComponent<Fluff>();
            hat = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.gameObject.tag.ToLower().Equals(Constants.TagHat.ToLower()));
            handL = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.l"));
            handR = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.r"));
            head = new List<Transform>(player.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("head.x"));
            hatPosDefault = hat.localPosition;
            hatRotDefault = hat.localEulerAngles;
            chicofx = player.GetComponent<ChicoFXController>();
        }

        public override bool DoSomething()
        {
            if (isDoing)
                return false;

            isDoing = true;

            PlayerController.SetInputEnabled(false);
            MessageBox.Show(MessageBox.Types.YesNo, "Are you sure you want to shave?", HandleOnYes, HandleOnNo);
            
            return true;
        }

        public override bool CanBeDone()
        {
            if (fluff.HasMoustache || fluff.HasBeard)
                return true;
           
            return false;
        }

        public override void ActionMessage(string message)
        {
            if (message == "Exit")
                ShaveCompleted();

            if (message == "TakeHat")
                WashbasinUtility.TakeHat(hat, handL);

            if (message == "ReleaseHat")
                WashbasinUtility.ReleaseHat(hat);

            if (message == "PutOnHead")
                StartCoroutine(WashbasinUtility.DoPutOnHead(hat, head, hatPosDefault, hatRotDefault));

            if (message == "TakeBrush")
                TakeBrush();

            if (message == "TakeBlade")
                StartCoroutine(TakeBlade());

            if (message == "ShowFoam")
                ShowFoam();

            if (message == "Shaved")
                Shaved();

            if (message == "ReleaseBlade")
                ReleaseBlade();

            if (message == "PlayWash")
                PlayWash();

            if (message == "PlayBrush")
                PlayBrush();

            if (message == "PlayBlade")
                PlayBlade();

            if (message == "PlayHumming")
                PlayHumming();

        }

        private void Shave()
        {
            Animator anim = PlayerController.GetComponent<Animator>();
            anim.SetFloat("GenericActionId", 0);
            anim.SetTrigger("GenericAction");
            
            //yield return new WaitForSeconds(10);

           
        }

        void ShaveCompleted()
        {
            PlayerController.SetInputEnabled(true);

            isDoing = false;
            StopExecuting();
        }

        void TakeBrush()
        {
            tool = Utility.ObjectPopIn(foamBrushPrefab, handR);
            toolSource = tool.GetComponent<AudioSource>();
        }

        IEnumerator TakeBlade()
        {
            Utility.ObjectPopOut(tool);

            yield return new WaitForSeconds(1f);
            tool = Utility.ObjectPopIn(bladePrefab, handR);
            toolSource = tool.GetComponent<AudioSource>();
        }

        void ShowFoam()
        {
            foam = Utility.ObjectPopIn(foamPrefab, head);
        }

        private void HandleOnYes()
        {
            Shave();
        }

        private void ReleaseBlade() 
        {
            Utility.ObjectPopOut(tool);
        }

        private void HandleOnNo()
        {
            PlayerController.SetInputEnabled(true);
            isDoing = false;
            StopExecuting();
        }

        private void Shaved()
        {
            Utility.ObjectPopOut(foam);
            fluff.Shave();
        }

        void PlayWash()
        {
            toolSource.clip = washClips[Random.Range(0, washClips.Count)];
            toolSource.Play();
        }

        void PlayBrush()
        {
            toolSource.clip = brushClips[Random.Range(0, brushClips.Count)];
            toolSource.Play();
        }

        void PlayBlade()
        {
            toolSource.clip = shaveClips[Random.Range(0, shaveClips.Count)];
            toolSource.Play();
        }

        void PlayHumming()
        {
            chicofx.Play(happyClip);
        }
    }

}

public static class WashbasinUtility
{
    public static void TakeHat(Transform hat, Transform hand)
    {
        hat.parent = hand;
    }

    public static void ReleaseHat(Transform hat, Transform newParent = null)
    {
        hat.parent = newParent;
    }

    
    public static IEnumerator DoPutOnHead(Transform hat, Transform head, Vector3 hatPosition, Vector3 hatEulerAngles)
    {
        hat.parent = head;
        yield return null;
        LeanTween.moveLocal(hat.gameObject, hatPosition, 0.2f);
        LeanTween.rotateLocal(hat.gameObject, hatEulerAngles, 0.2f);


    }
}