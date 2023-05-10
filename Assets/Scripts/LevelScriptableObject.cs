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


    public ImagesFlags imagesFlags;
    public SpreadFlags spreadFlags;
    public MinMax itemsToSpawn;
    public MinMax reactionTime;
    public int switchButtonsChance;
    public ShapeType shapesType;


    // levelNumber    
    [HideInInspector] public int levelNumber;
    // general inital value
    [HideInInspector] public float probabilityToDifferentShape = 0.5f;
    
    // shapeType
    [HideInInspector] public GameObject spawningPrefab;
    // reactionTime
    [HideInInspector] public int minReactionTime;
    [HideInInspector] public int maxReactionTime;
    // itemsToSpawn
    [HideInInspector] public int minItemsToSpawn;
    [HideInInspector] public int maxItemsToSpawn;
    // inspectorFlags
        // spreadFlags
    [HideInInspector] public bool spawnLine;
    [HideInInspector] public bool spawnColumn;
    [HideInInspector] public bool spawnRandom;
        // imageFlags
    [HideInInspector] public bool geometricImages;
    [HideInInspector] public bool semanticImages;


    void FindPrefabByShape(){
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
        geometricImages = imagesFlags.isGeometric;
        semanticImages = imagesFlags.isSemantic;
    
        // shapesType;  -   find prefab from enum
        FindPrefabByShape();

        // spreadFlags
        spawnColumn = spreadFlags.isColumn;
        spawnLine = spreadFlags.isLine;
        spawnRandom = spreadFlags.isRandom;
        
        // itemsToSpawn
        minItemsToSpawn = itemsToSpawn.min;
        maxItemsToSpawn = itemsToSpawn.max;
        
        // reactionTime
        minReactionTime = reactionTime.min;
        maxReactionTime = reactionTime.max;
    }
}