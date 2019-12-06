using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/**
    * Simply create a message box calling GameObject.Instantiate(prefab); then call Init().
    * */
public class MessageBox : MonoBehaviour
{
    public enum Types { Ok, YesNo }

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button okButton;

    [SerializeField]
    private Button noButton;

    [SerializeField]
    private Text msgText;

    [SerializeField]
    private Image panelImage;

    //private UnityAction _okYesAction;

    //private UnityAction _noAction;

    private static MessageBox instance;

    private Color panelColorDefault;

    

    private RectTransform box;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            transform.SetAsLastSibling();
            panelColorDefault = panelImage.color;
            box = transform.GetChild(0) as RectTransform;
            gameObject.SetActive(false);
                
        }
        else
        {
            Object.Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
            
    }

    private void OnDisable()
    {
        panelImage.sprite = null;
        panelImage.color = panelColorDefault;
        instance.box.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }



    public static void Show(Types type, string msgText = null, UnityAction okYesAction = null, UnityAction noAction = null)
    {
        switch (type)
        {
            case Types.Ok:
            instance.okButton.gameObject.SetActive(true);
            instance.yesButton.gameObject.SetActive(false);
            instance.noButton.gameObject.SetActive(false);

                if (okYesAction != null)
                {
                instance.okButton.onClick.AddListener(okYesAction);
                }

                instance.okButton.onClick.AddListener(delegate { instance.gameObject.SetActive(false); });
                break;

            case Types.YesNo:
            instance.okButton.gameObject.SetActive(false);
            instance.yesButton.gameObject.SetActive(true);
            instance.noButton.gameObject.SetActive(true);
                if (okYesAction != null)
                {
                instance.yesButton.onClick.AddListener(okYesAction);
                }
                if (noAction != null)
                {
                instance.noButton.onClick.AddListener(noAction);

                }
            instance.yesButton.onClick.AddListener(delegate { instance.gameObject.SetActive(false); });
            instance.noButton.onClick.AddListener(delegate { instance.gameObject.SetActive(false); });
                break;
        }

        if (msgText != null)
        instance.msgText.text = msgText;

    instance.gameObject.SetActive(true);
    }

    public static void SetPanel(Sprite sprite)
    {
    instance.panelImage.sprite = sprite;
    instance.panelImage.color = Color.white;
    }

    public static void SetBottomPosition()
    {
    instance.box.localPosition = new Vector3(0, -380, 0);
    }

}

