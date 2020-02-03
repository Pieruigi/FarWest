using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostEventController : EventController
{
    [SerializeField]
    Mesh groundMesh;
    
    [SerializeField]
    GameObject ghostPrefab;

    [SerializeField]
    AudioClip teeths;

    GameObject ghost;

    Animator playerAnim;
    ChicoFXController playerFx;
    protected override void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag(Constants.TagPlayer);

        playerAnim = p.GetComponent<Animator>();
        playerFx = p.GetComponent<ChicoFXController>();
    }

    protected override void Execute()
    {
       
        StartCoroutine(DoExecute());
    }

    protected override void Stop()
    {
        StartCoroutine(DoStop());
    }

    IEnumerator DoExecute()
    {
        ghost = GameObject.Instantiate(ghostPrefab);
        ghost.GetComponentInChildren<FadeInOutAlpha>().FadeIn();

        yield return new WaitForSeconds(1.5f);
        playerAnim.SetFloat("SSLoopId", 22);
        playerAnim.SetTrigger("SSLoop");

        playerFx.Play(teeths, true);
    }

    IEnumerator DoStop()
    {
        ghost.GetComponentInChildren<FadeInOutAlpha>().FadeOut();
        AudioSource[] sources = ghost.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource s in sources)
            s.GetComponent<FadeInOutAudio>().FadeOut();
            

        yield return new WaitForSeconds(3f);

        Destroy(ghost);

        playerFx.StopPlaying();
        playerAnim.SetTrigger("SSLoopExit");

        yield return new WaitForSeconds(2f);

        Debug.Log("Child Stop");
        base.Stop();
    }
}
