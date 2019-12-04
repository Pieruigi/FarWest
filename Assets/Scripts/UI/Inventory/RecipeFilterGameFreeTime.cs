using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeFilterGameFreeTime : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    Button button;

    int filter = 0; // 0:all; 1:game; 2:free time

    Image image;
    
    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleOnClick);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        filter = 0;
        image.sprite = sprites[filter];
    }

    void HandleOnClick()
    {
        filter++;
        if (filter == 3)
            filter = 0;

        SendMessageUpwards("FilterGameFreeTime", filter);
        
        

        image.sprite = sprites[filter];
        
    }
}
