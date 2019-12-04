using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Assets/Create/Assets/Item")]
    public static void CreateItem()
    {
        Item asset = ScriptableObject.CreateInstance<Item>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Items/Empty.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Assets/Recipe")]
    public static void CreateHammerRecipe()
    {
        Recipe asset = ScriptableObject.CreateInstance<Recipe>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Recipes/Empty.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Assets/Building")]
    public static void CreateBuilding()
    {
        Building asset = ScriptableObject.CreateInstance<Building>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Buildings/Empty.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

}
