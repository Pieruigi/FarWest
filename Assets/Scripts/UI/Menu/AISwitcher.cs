using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AISwitcher : MonoBehaviour
{
    [SerializeField]
    Color colorDisabled;

    [SerializeField]
    Sprite spriteDisabled;

    Color colorEnabled;
    Sprite spriteEnabled;


    Image image;

    bool isEnabled = true;

    private void Awake()
    {
        image = GetComponent<Image>();
        colorEnabled = image.color;
        spriteEnabled = image.sprite;

        string key = ProfileCacheManager.Instance.GetValue(FreeTimeBuildManager.PlayerCacheAI);
        if (key != null && !"".Equals(key.Trim()) && "0".Equals(key.Trim()))
        {
            isEnabled = false;
            image.color = colorDisabled;
            image.sprite = spriteDisabled;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchAI()
    {
        if (isEnabled)
        {
            
            image.color = colorDisabled;
            image.sprite = spriteDisabled;
            ProfileCacheManager.Instance.AddOrUpdate(FreeTimeBuildManager.PlayerCacheAI, "0");
        }
        else
        {
            image.color = colorEnabled;
            image.sprite = spriteEnabled;
            ProfileCacheManager.Instance.AddOrUpdate(FreeTimeBuildManager.PlayerCacheAI, "1");
        }
        isEnabled = !isEnabled;
        ProfileCacheManager.Instance.Save();
    }
}
