using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
#if !UNITY_EDITOR
        Destroy(gameObject);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            CacheManager.Instance.Save();
    }
}
