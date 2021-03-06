﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationController : MonoBehaviour
{
    [SerializeField]
    string spritesFolder;

    [SerializeField]
    float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; time = 1f / Mathf.Abs(speed); }
    }

    [SerializeField]
    bool loop;

    [SerializeField]
    bool autoPlay;

    List<Sprite> sprites;

    bool isPlaying = false;

    Image image;

    int currentId = 0;

    float elapsed;
    float lastUpdate = 0;

    float time;

    private void Awake()
    {
        time = 1f / Mathf.Abs(speed);

        image = GetComponent<Image>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if (autoPlay)
            Play();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
            return;

        elapsed += Time.deltaTime;

        float diff = elapsed - lastUpdate;
        if (diff > time)
        {
            lastUpdate = elapsed;
            int disp = Mathf.RoundToInt( diff / time );

            if (!loop)
            {
                
                if(speed > 0)
                {
                    currentId += disp;
                    if (currentId > sprites.Count - 1)
                        currentId = sprites.Count - 1;
                }
                else
                {
                    currentId -= disp;
                    if (currentId < 0)
                        currentId = 0;
                }

                
            }
            else
            {
                if(speed > 0)
                    currentId = (currentId + disp) % sprites.Count;
                else
                {
                    currentId -= disp;
                    if (currentId < 0)
                        currentId = sprites.Count + currentId;
                    currentId = currentId % sprites.Count;
                }
                    
            }

            UpdateSprite();
        }
    }

    public void Play()
    {
        if (sprites == null)
            LoadSprites();

        isPlaying = true;
        if(speed>0)
            currentId = 0;
        else
            currentId = sprites.Count-1;

        image.sprite = sprites[currentId];
        elapsed = 0;
        lastUpdate = 0;
    }

    public void Stop()
    {
        isPlaying = false;
    }

    void UpdateSprite()
    {
       
        image.sprite = sprites[currentId];
    }

    void LoadSprites()
    {
        sprites = new List<Sprite>(Resources.LoadAll<Sprite>(spritesFolder));

        
    }
}
