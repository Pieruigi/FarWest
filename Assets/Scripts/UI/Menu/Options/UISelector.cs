using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class UISelector<T> : MonoBehaviour
{
    public UnityAction<T> OnOptionChanged;

    [Header("Base Section")]

    [SerializeField]
    string keyName; // If null the setting is not in cache, but rather from the engine ( as for the resolution )

    [SerializeField]
    GameObject optionText;

    [SerializeField]
    Button optionPrev;

    [SerializeField]
    Button optionNext;

    //[SerializeField]
    //GameObject dependency;

    List<T> options;
    protected List<T> Options
    {
        get { return options; }
    }

    int currentOptionId = 0;


    int defaultOptionId = 0;
    protected int DefaultOptionId
    {
        get { return defaultOptionId; }
        set { defaultOptionId = value; }
    }

    //bool isInteractable = false;
    //public bool IsInteractable
    //{
    //    get { return isInteractable; }
    //}

    bool buttonsForcedOff = false;

    bool hasInputField = false;

    bool onValueChangedDisabled = false;

    protected abstract void InitOptionList();
    
    public abstract void Commit();

    protected virtual void Awake()
    {
        if(!"".Equals(keyName))
        {
            ProfileCacheManager.Instance.OnSave += HandleOnSave;
        }

        InputField iField = optionText.GetComponent<InputField>();
        if (iField)
        {
            hasInputField = true;
            iField.onValueChanged.AddListener(HandleOnInputFieldValueChanged);
        }
            


            // Init available options
        options = new List<T>();
        InitOptionList();

        // Init
        Init();
        
    }

    protected virtual void Start() { }

    #region VIRTUAL
    protected virtual void Init()
    {
        int id = defaultOptionId;

        bool found = false;

        // Try to load setting from profile cache
        if (!ProfileCacheManager.Instance.IsEmpty())
        {
            if (int.TryParse(ProfileCacheManager.Instance.GetValue(keyName), out id))
                found = true;
            else
                id = defaultOptionId; // Reset the id
        }


        // Set the option: change text, reset buttons...
        SetOption(id);

        // Commit changes ( for example enable bloom )
        Commit();

        // If not found then set in cache
        if (!found)
            ProfileCacheManager.Instance.Save();
            
    }


    protected virtual string FormatOptionString(T option)
    {
        return option.ToString();
    }

    #endregion

    // Start is called before the first frame update


    #region PUBLIC
    public void SelectNext()
    {
       
        if (currentOptionId + 1 >= Options.Count)
            return;

        SetOption(currentOptionId + 1);

        if (!"".Equals(keyName))
            ProfileCacheManager.Instance.Save();
            //ProfileCacheManager.Instance.AddOrUpdate(keyName, currentOptionId.ToString());

        Commit();
    }


    public void SelectPrevious()
    {
        if (currentOptionId <= 0)
            return;

        SetOption(currentOptionId-1);

        if (!"".Equals(keyName))
            ProfileCacheManager.Instance.Save();
            //ProfileCacheManager.Instance.AddOrUpdate(keyName, currentOptionId.ToString());

        Commit();
    }

    public void ResetButtons()
    {

        optionPrev.interactable = false;
        optionNext.interactable = false;

        if (options == null || buttonsForcedOff)
            return;

        if (currentOptionId > 0)
            optionPrev.interactable = true;

        if (currentOptionId < options.Count - 1)
            optionNext.interactable = true;
    }

    public void ForceInteractablesOff(bool value)
    {
        buttonsForcedOff = value;
        ResetButtons();

        if (hasInputField)
            optionText.GetComponent<InputField>().interactable = !value;
    }
    #endregion


    #region PROTECTED
    protected void SetOption(int id)
    {
        if (id < 0)
            return;

        currentOptionId = id;

        if (!hasInputField)
            optionText.GetComponent<Text>().text = FormatOptionString(options[currentOptionId]);
        else
        {
            onValueChangedDisabled = true;
            optionText.GetComponent<InputField>().text = FormatOptionString(options[currentOptionId]);
            onValueChangedDisabled = false;
        }
            

        OnOptionChanged?.Invoke(options[currentOptionId]);

        ResetButtons();
    }

    protected T GetCurrentOption()
    {
        return options[currentOptionId];
    }

    #endregion

    #region PRIVATE



    void HandleOnSave()
    {
        ProfileCacheManager.Instance.AddOrUpdate(keyName, currentOptionId.ToString());
    }




    void HandleOnInputFieldValueChanged(string value)
    {
        if (onValueChangedDisabled)
            return;

        int id = options.FindIndex(o => o.ToString() == value);

        if (id < 0)
            return;

        SetOption(id);

        if (!"".Equals(keyName))
            ProfileCacheManager.Instance.Save();
        

        Commit();

        ResetButtons();
    }

    #endregion


}
