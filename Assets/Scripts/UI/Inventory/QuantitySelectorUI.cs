using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuantitySelectorUI : MonoBehaviour
{
    [SerializeField]
    private Image imageIcon;

    [SerializeField]
    private Text textMax;

    [SerializeField]
    private Text textCount;

    //private int min = 1;
    private int count;

    public Slider slider;

    private UnityAction<int> callback; // Returns zero if you press "cancel", otherwise returns the number of selected elements
    
    public bool IsActive
    {
        get { return transform.GetChild(0).gameObject.activeSelf; }
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            count = (int)slider.value;
            textCount.text = count.ToString();
        }
    }

    private void OnDisable()
    {
        
    }

    private void Init(Sprite icon, int max, UnityAction<int> callback)
    {
        imageIcon.sprite = icon;
        slider.value = slider.minValue;
        count = (int)slider.value;
        slider.maxValue = max;
        textMax.text = max.ToString();
        textCount.text = count.ToString();
        this.callback = callback;
       
    }

    public void Show(Sprite icon, int max, UnityAction<int> callback)
    {
        //transform.GetChild(0).gameObject.SetActive(true);
        Init(icon, max, callback);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Cancel()
    {
        count = 0;
        Hide();
    }

    public void Select()
    {
        count = (int)slider.value;
        Hide();
    }

    public void All()
    {
        count = (int)slider.maxValue;
        Hide();
    }

    

    private void Hide()
    {
        callback?.Invoke(count);
        callback = null;

        // Reset
        //max = 0;
        //count = min;
        //imageIcon.sprite = null;
        //textCount.text = count.ToString();
        transform.GetChild(0).gameObject.SetActive(false);
    }


}
