using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Asset : ScriptableObject, IEquatable<Asset>
{
    
    //[SerializeField]
    //private string code;
    public string Code
    {
        get { return string.Format("{0}{1}",GetType().ToString(), name); }
    }

    [Header("Common")]
    [SerializeField]
    private string assetName;
    public String AssetName
    {
        get { return assetName; }
    }

    [SerializeField]
    private string description;
    public string Description
    {
        get { return description; }
    }

    [SerializeField]
    private bool freeTimeOnly;
    public bool FreeTimeOnly
    {
        get { return freeTimeOnly; }
    }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
    }

    public static bool operator ==(Asset asset1, Asset asset2)
    {
        return asset1?.Code == asset2?.Code;
    }

    public static bool operator !=(Asset asset1, Asset asset2)
    {
        return !(asset1?.Code == asset2?.Code);
    }

    public bool Equals(Asset other)
    {

        return Code == other?.Code;
    }

    public override bool Equals(object other)
    {
        if (typeof(Asset) != other.GetType())
            return false;

        return Equals(other as Asset);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Code.GetHashCode();
            return hashCode;
        }
        
    }
}
