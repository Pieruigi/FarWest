using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RecipeBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("Assets/Create/Builders/Recipe")]
    public static void CreateHammerRecipe()
    {
        Recipe asset = ScriptableObject.CreateInstance<Recipe>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/Recipes/Empty.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
