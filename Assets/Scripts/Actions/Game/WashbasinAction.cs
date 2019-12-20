using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS
{
    public class WashbasinAction : Action
    {
        [SerializeField]
        GameObject foamBrushPrefab;

        [SerializeField]
        GameObject bladePrefab;

        [SerializeField]
        GameObject foamPrefab;

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

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            fluff = PlayerController.GetComponent<Fluff>();
            hat = new List<Transform>(PlayerController.GetComponentsInChildren<Transform>()).Find(g => g.gameObject.tag.ToLower().Equals(Constants.TagHat.ToLower()));
            handL = new List<Transform>(PlayerController.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.l"));
            handR = new List<Transform>(PlayerController.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("hand.r"));
            head = new List<Transform>(PlayerController.GetComponentsInChildren<Transform>()).Find(g => g.name.ToLower().Equals("head.x"));
            hatPosDefault = hat.localPosition;
            hatRotDefault = hat.localEulerAngles;
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
            Debug.Log("ActionMessage:" + message);
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
        }

        IEnumerator TakeBlade()
        {
            Utility.ObjectPopOut(tool);

            yield return new WaitForSeconds(1f);
            tool = Utility.ObjectPopIn(bladePrefab, handR);
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

    }

}

public static class WashbasinUtility
{
    public static void TakeHat(Transform hat, Transform hand)
    {

        Debug.Log("hat:" + hat);
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