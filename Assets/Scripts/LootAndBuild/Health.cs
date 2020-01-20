using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Health : MonoBehaviour
{
    [SerializeField]
    Image imageHealth;

    [SerializeField]
    Image imageBackground;

    //[SerializeField]
    Transform owner;

    PlayerController playerCtrl;

    bool loop = false;

    SS.LootAction current;

    void Awake()
    {
        MainManager mainManager = GameObject.FindObjectOfType<MainManager>();
        if (mainManager.IsScreenSaver || mainManager.SandboxMode)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            imageHealth.enabled = false;
            imageBackground.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        owner = transform.root;
        
        //healthImg = GetComponentInChildren<Image>();
        playerCtrl = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerController>();

        playerCtrl.OnLootStarted += HandleOnLootStarted;
        playerCtrl.OnLootStopped += HandleOnLootStopped;

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 pos = Camera.main.WorldToScreenPoint(owner.position);

        //healthImg.rectTransform.position = pos;

        if (!loop)
            return;

        //Debug.Log("Health:" + current.GetHealthNormalized());
        imageHealth.fillAmount = current.GetHealthNormalized();
    }

    private void HandleOnLootStarted(SS.LootAction action)
    {
        imageHealth.enabled = true;
        imageBackground.enabled = true;

        current = action;
        //current.OnExhausted += HandleOnExhausted;

        Vector2 pos = Camera.main.WorldToScreenPoint(action.transform.root.position);

        imageHealth.rectTransform.position = pos + 100f*Vector2.up;
        imageBackground.rectTransform.position = pos + 100f * Vector2.up;

        loop = true;
    }

    private void HandleOnLootStopped()
    {
        //current.OnExhausted -= HandleOnExhausted;
        imageHealth.fillAmount = 1;
        imageHealth.enabled = false;
        imageBackground.enabled = false;
    }

    //private void HandleOnExhausted(SS.LootAction action)
    //{
    //    HandleOnLootStopped();
    //}
}
