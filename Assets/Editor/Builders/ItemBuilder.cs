using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [MenuItem("Assets/Create/Builders/Item")]
    public static void CreateItem()
    {
        Item asset = ScriptableObject.CreateInstance<Item>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Items/Empty.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    
}
