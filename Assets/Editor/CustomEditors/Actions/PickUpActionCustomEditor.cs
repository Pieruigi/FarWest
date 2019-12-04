using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SS
{
    //[CustomEditor(typeof(PickUpAction))]
    [CanEditMultipleObjects]
    public abstract class ActionCustomEditor : Editor
    {
        SerializedProperty reachingMode;

        SerializedProperty reachingTarget;

        SerializedProperty neededResources;

        SerializedProperty neededTool;


        protected virtual void OnEnable()
        {
            reachingMode = serializedObject.FindProperty("reachingMode");
            reachingTarget = serializedObject.FindProperty("target");
            neededResources = serializedObject.FindProperty("neededResources");
            neededTool = serializedObject.FindProperty("neededTool");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(neededResources);
            EditorGUILayout.PropertyField(neededTool);

            EditorGUILayout.PropertyField(reachingMode);



            switch (reachingMode.enumValueIndex)
            {
                case (int)ReachingMode.Point:
                    EditorGUILayout.PropertyField(reachingTarget);
                    break;

                default:
                    break;

            }



            serializedObject.ApplyModifiedProperties();


            //nextField.serializedObject.vi 
        }
    }

}
