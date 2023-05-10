using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JSONLevelReader : MonoBehaviour
{

    [SerializeField] TextAsset jsonFile;

    void Awake()
    {
        string jsonString = jsonFile.text;
        JSONNode data = JSON.Parse(jsonString);
        foreach (JSONNode jsonLevel in data["levels"]){
        // find the level scriptable object from resources 
            int levelNum = int.Parse(jsonLevel["levelNumber"].Value);
            LevelScriptableObject newLevel = Resources.Load<LevelScriptableObject>(string.Format("Levels/Level {0}", levelNum));
            
        // set variables through json:
            // set level number
            newLevel.levelNumber = int.Parse(jsonLevel["levelNumber"].Value);  
            // set images flags
            newLevel.imagesFlags.geometricImages = jsonLevel["geometricImages"].Value == "True";  
            newLevel.imagesFlags.semanticImages = jsonLevel["semanticImages"].Value == "True";  
            // spawn arrows chance
            newLevel.spawnArrowsChance = (int)(float.Parse(jsonLevel["spawnArrowsChance"].Value) * 100);  
            // set spread flags
            newLevel.spreadFlags.spawnLine = jsonLevel["spawnLine"].Value == "True";
            newLevel.spreadFlags.spawnColumn = jsonLevel["spawnColumn"].Value == "True";
            newLevel.spreadFlags.spawnRandom = jsonLevel["spawnRandom"].Value == "True";
            // set items to spawn
            newLevel.itemsToSpawn.min = int.Parse(jsonLevel["minItemsToSpawn"].Value);
            newLevel.itemsToSpawn.max = int.Parse(jsonLevel["maxItemsToSpawn"].Value);
            // set reaction time
            newLevel.reactionTime.min = int.Parse(jsonLevel["minReactionTime"].Value);
            newLevel.reactionTime.max = int.Parse(jsonLevel["maxReactionTime"].Value);
            // switch buttons chance
            newLevel.switchButtonsChance = (int)(float.Parse(jsonLevel["switchButtonsChance"].Value) * 100);  
            // switch min steps to switch
            newLevel.minStepsToSwitch = int.Parse(jsonLevel["minStepsToSwitch"].Value);  
        }

    }
    
    
    /** json:
 levels [
    {
        "levelNumber": 20,
        "geometricImages": false,
        "semanticImages": True,
        "spawnArrowsChance": 0.1,
        "spawnLine": True,
        "spawnColumn": True,
        "spawnRandom": True,
        "minItemsToSpawn": 11,
        "maxItemsToSpawn": 21,
        "minReactionTime": 5,
        "maxReactionTime": 8,
        "switchButtonsChance": 0.5,
        "minStepsToSwitch": 1
    }
]
    */
}
