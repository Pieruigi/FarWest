using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraFilters : MonoBehaviour
{
    public UnityAction<Component> OnFilterActivated;

    List<GameObject> filters;

    Component currentFilter;
    public Component CurrentFilter
    {
        get { return currentFilter; }
    }

    bool testingFilter = false;
    public bool TestingFilter
    {
        get { return testingFilter; }
    }

    private void Awake()
    {
        filters = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
            filters.Add(transform.GetChild(i).gameObject);

        Debug.Log("Filters.Count:" + filters.Count);
    }


    public List<string> GetFilterNameList()
    {
        List<string> ret = new List<string>();
        foreach (GameObject g in filters)
            ret.Add(g.name);

        return ret;
    }

    public void ActivateFilter(string name)
    {
        GameObject filter = filters.Find(g => g.name == name);

        if (!filter)
            currentFilter = null;
        else
            currentFilter = new List<Component>(filter.GetComponents<Component>()).Find(c => c.GetType() != typeof(Transform));

        OnFilterActivated?.Invoke(currentFilter);
    }

    public void TestFilter(bool value)
    {
        testingFilter = value;

        OnFilterActivated?.Invoke(value ? currentFilter : null);
       
    }


}
