using UnityEngine;
using System;
using System.Text.RegularExpressions;

public enum ShapeType
{
    Images,
    Arrows
}


[Serializable]
[CreateAssetMenu(fileName = "New Level", menuName = "New Level", order = 2)]
public class LevelScriptableObject : ScriptableObject {


    public ImagesFlags imagesFlags; // (semantic images or geometric images)
    public SpreadFlags spreadFlags; // (spread in line, column or randomly )
    public MinMax itemsToSpawn;     // (minimum and maximum items to spawn)
    public MinMax reactionTime;     // (minimum and maximum reaction time )
    public int switchButtonsChance;
    public int spawnArrowsChance = 10;


    // levelNumber    
    [HideInInspector] public int levelNumber;
    // general inital value
    [HideInInspector] public float probabilityToDifferentShape = 0.5f;
    
    // shapeType
    [HideInInspector] public ShapeType shapesType;
    // shape prefab
    [HideInInspector] public GameObject spawningPrefab;
    // reactionTime
    [HideInInspector] public int minReactionTime;
    [HideInInspector] public int maxReactionTime;
    // itemsToSpawn
    [HideInInspector] public int minItemsToSpawn;
    [HideInInspector] public int maxItemsToSpawn;
    // min steps to switch
    [HideInInspector] public int minStepsToSwitch;

    // inspectorFlags
        // spreadFlags
    [HideInInspector] public bool spawnLine;
    [HideInInspector] public bool spawnColumn;
    [HideInInspector] public bool spawnRandom;
        // imageFlags
    [HideInInspector] public bool geometricImages;
    [HideInInspector] public bool semanticImages;
    [HideInInspector] public bool geometric_semanticImages;


    public void SetSpawningPrefab(){
        string path = "Prefabs/Shapes/";
        switch (shapesType)
        {
            case ShapeType.Arrows:
                path += "Arrow";
                break; 
            // case ShapeType.Letters:
            //     path += "Letter";
            //     break;
            case ShapeType.Images:
                path += "Image";
                break;
        }
        spawningPrefab = Resources.Load<GameObject>(path + "-Prefab");
    }

    public void InitData(){
        // levelNumber
        string pattern = @"(\d+)$"; // regex for "level + num"
        Match match = Regex.Match(name, pattern); // get the regex match
        if (match.Success)
            int.TryParse(match.Groups[1].Value, out levelNumber); // parse out to levelNumber the value of the num
        
        // imageFlags
        if (imagesFlags != null)
        {
            geometricImages = imagesFlags.geometricImages;
            semanticImages = imagesFlags.semanticImages;
        }
        geometric_semanticImages = semanticImages && geometricImages;
    
        // shapesType;  -   find prefab from enum
        SetSpawningPrefab();

        // spreadFlags
        spawnColumn = spreadFlags.spawnColumn;
        spawnLine = spreadFlags.spawnLine;
        spawnRandom = spreadFlags.spawnRandom;
        
        // itemsToSpawn
        minItemsToSpawn = itemsToSpawn.min;
        maxItemsToSpawn = itemsToSpawn.max;
        
        // reactionTime
        minReactionTime = reactionTime.min;
        maxReactionTime = reactionTime.max;
    }
}