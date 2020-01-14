using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChicoFXController : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> steps;

    [SerializeField]
    List<AudioClip> axeHits;

    [SerializeField]
    List<AudioClip> pickaxeHits;

    [SerializeField]
    List<AudioClip> shovelHits;

    [SerializeField]
    List<AudioClip> hammerHits;

    [SerializeField]
    AudioClip knifeHit;

    [SerializeField]
    AudioClip pickUp;

    [SerializeField]
    List<AudioClip> soilHands;

    [SerializeField]
    List<AudioClip> rockKicks;

    [SerializeField]
    AudioClip rockAhi;

    [SerializeField]
    AudioClip shrubAhi;

    [SerializeField]
    AudioClip treeHit;

    [SerializeField]
    AudioClip treeFall;

    [SerializeField]
    AudioClip handCraft;

    [SerializeField]
    AudioSource source;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController)
        {
            playerController.OnLootStopped += HandleOnLootStopped;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void HandleOnLootStopped()
    {
        StopPlaying();
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }
    void PlaySteps()
    {
        PlayRandom(steps, false, 0.3f);
    }

    void PlayAxeHits()
    {
        PlayRandom(axeHits);
    }

    void PlayPickaxeHits()
    {
        PlayRandom(pickaxeHits);
    }

    void PlayShovelHits()
    {
        PlayRandom(shovelHits);
    }

    void PlayHammerHits()
    {
        PlayRandom(hammerHits);
    }

    void PlayKnifeHit()
    {
        Play(knifeHit, true);
    }

    void PlaySoilHand()
    {
        PlayRandom(soilHands, false);
    }

    void PlayRockKick()
    {
        PlayRandom(rockKicks, false);
    }

    void PlayRockAhi()
    {
        Play(rockAhi, false);
    }

    void PlayShrubAhi()
    {
        Play(shrubAhi, false);
    }

    void OnPickUpEnter()
    {
        Play(pickUp, false);
    }

    void PlayTreeHit()
    {
        Debug.Log("PlayHit");
        Play(treeHit, false);
    }

    void PlayTreeAhi()
    {
        Play(treeFall, false);
    }

    void PlayHandCraft()
    {
        Play(handCraft, false);
    }

    void StopPlaying()
    {
        source.Stop();
    }

    public void PlayRandom(List<AudioClip> clips, bool loop = false, float volume = 1)
    {
        int r = Random.Range(0, clips.Count);

        Play(clips[r], loop, volume);

        //source.clip = clips[r];
        //source.loop = loop;
        //source.Play();
    }

    public void Play(AudioClip clip, bool loop = false, float volume = 1)
    {
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.Play();
    }



}
