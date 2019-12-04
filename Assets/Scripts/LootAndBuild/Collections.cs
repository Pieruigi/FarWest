using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/**
 * Code convension:
 * Item: ITMXXXX
 * Recipe: RCPXXXX
 * Building: BLDXXXX
 * */

public class AssetCollection<T> where T : Asset
{

    protected List<T> objects;

    protected AssetCollection(){}

    private static AssetCollection<T> instance;

    public static void Create(string path)
    {

        if (instance == null)
        {
            instance = new AssetCollection<T>();

            object[] tmp = Resources.LoadAll(path);

            instance.objects = new List<T>();
            foreach (object o in tmp)
            {
                instance.objects.Add((T)o);
            }

        }
    }

    public static T GetAssetByCode(string code)
    {
        return instance.objects.Find(i => i.Code == code);
    }

    public static IList<T> GetAssetAll()
    {
        return instance.objects.AsReadOnly();
    }
}

public class ItemCollection : AssetCollection<Item>
{

}

public class RecipeCollection : AssetCollection<Recipe>
{

}

