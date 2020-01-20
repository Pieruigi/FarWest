using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectQualityLevel { None, Low, Medium, High }


public class Constants
{
    //
    // Layers
    //
    public const string LayerNameGround = "Ground";
    public const string LayerNameInteraction = "Interaction";

    //
    // Tags
    //
    public const string TagPlayer = "Player";
    public const string TagCameraClose = "CameraClose";
    public const string TagDoor = "Door";
    public const string TagHat = "Hat";

    //
    // Methods
    //
    public const string MethodExecute = "StartExecuting";
    public const string MethodStopExecuting = "StopExecuting";

    //
    // Paths
    //
    public const string PathAssetItem = "Items";
    public const string PathAssetRecipes = "Recipes";


    //
    // Cache manager keys
    public const string CacheKeyInventory = "INV"; // For inventory
    public const string CacheKeyTimeElapsed = "TEL"; // The time you are playing in seconds
    public const string CacheKeySpawnable = "SPW"; // Key for spawnable objects
    public const string CacheKeyFluffElapsed = "FEL";
    public const string CacheKeyStorage = "STG"; // For storage


    // Animation layers
    public const string AnimationLayerScreenSaver = "SS Layer";
    public const string AnimationLayerGame = "Game Layer";

    // Animations
    public const string AnimationNameUseHammer = "UseHammer";
    public const string AnimationNameUseHand = "UseHand";
}


