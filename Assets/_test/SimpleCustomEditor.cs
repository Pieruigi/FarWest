//#define AAA
#if AAA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

//[CustomEditor(typeof(SceneManager))]
[CanEditMultipleObjects]
public class SimpleCustomEditor : Editor
{
    SerializedProperty nextFieldVisible;

    SerializedProperty nextField;

    SerializedProperty testObjects;

    SerializedProperty customProp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //Debug.Log("Enableddddddddddddddddd");
        
        nextFieldVisible = serializedObject.FindProperty("nextFieldVisible");

        nextField = serializedObject.FindProperty("nextField");

        testObjects = serializedObject.FindProperty("objList");
    }

    // public void SetProperties(string parent, List<string> children)
    //{
    //    Debug.Log("target:" + target.GetType());

    //    nextFieldVisible = serializedObject.FindProperty("nextFieldVisible");

    //    nextField = serializedObject.FindProperty("nextField");

    //    testObjects = serializedObject.FindProperty("objList");
    //}

    public override void OnInspectorGUI()
    {
        //MonoBehaviour myTarget = GameObject.FindObjectOfType<SceneManager>();
        ////target = myTarget;
        //EditorUtility.SetDirty(myTarget);

        serializedObject.Update();
        EditorGUILayout.PropertyField(nextFieldVisible);


        if (nextFieldVisible.boolValue)
        {
            EditorGUILayout.PropertyField(nextField, true);
            EditorGUILayout.PropertyField(testObjects, true);

            
            
        }
            


        

        serializedObject.ApplyModifiedProperties();

        Debug.Log(nextFieldVisible.boolValue);

        //nextField.serializedObject.vi 
    }
}
#endif