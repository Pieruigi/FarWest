using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFilterController : MonoBehaviour
{
    CameraFilters cameraFilters;

    Component filter;

    private void Awake()
    {
        cameraFilters = GameObject.FindObjectOfType<CameraFilters>();

        cameraFilters.OnFilterActivated += HandleOnFilterActivated;

        SetFilter(cameraFilters.CurrentFilter);
    }


    void HandleOnFilterActivated(Component filter)
    {
        SetFilter(filter);
    }

    void SetFilter(Component filter)
    {
        if(this.filter)
            Destroy(this.filter);

        if(filter)
        {
            this.filter = gameObject.AddComponent(filter.GetType());
            System.Reflection.FieldInfo[] fields = filter.GetType().GetFields();
           
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(this.filter, field.GetValue(filter));
            }

        }
    }


}
