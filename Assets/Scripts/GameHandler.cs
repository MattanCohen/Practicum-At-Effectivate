using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {

    public GameUiHandler gameUiHandler;
    public LevelScriptableObject levelData{get; private set;}
    public GameObject rowContent;
    public GameObject columnContent;
    public GameObject randomContent;
    

    [HideInInspector] public GameObject chosenContent;

    public int stageNum{get; private set;}   
    public int score{get; private set;}   
    

    [HideInInspector] public bool isPlaying;
    [HideInInspector] public bool isPlayingForever = false;
    [HideInInspector] public bool gameFinished{get; private set;}

    [HideInInspector] public bool shiftAShape{get; private set;}
    [HideInInspector] public int  rightMoves{get; private set;}
    [HideInInspector] public bool madeAtleastOneMistake{get; private set;}
    [HideInInspector] public int stepsSinceChange;
    [HideInInspector] public float reactionTime;   
    public float minutesForNormalLevel;
    public float minutesForInfiniteLevel;

    ShapeSpawner shapeSpawner;

    public static GameObject GetChildWithName(Transform trans, string name) {
    Transform childTrans = trans. Find(name);
    if (childTrans != null) {
        return childTrans.gameObject;
    } else {
        foreach (Transform t in trans)
        {
            var ans = GetChildWithName(t, name);
            if (ans != null)
                return ans;
        }
        return null;
    }
    }

    public void ResetStats(){
        stageNum = 0;
        score = 0;
        isPlaying = false;
        shiftAShape = false;
        stepsSinceChange = 0;
    }


    private void Start() {
        shapeSpawner = GetComponent<ShapeSpawner>();
    }


     

    // public bool FinishedGame(){return shapeSpawner.FinishedGame();}
    // each round is a minute and a half
    


    // public bool FinishedGame(){return FindObjectOfType<TimerScript>().GetTotalTimeInMinutes() > minutesForNormalLevel;} 
    public bool FinishedGame(){
        var timePassedInMinutes = FindObjectOfType<TimerScript>().GetTotalTimeInMinutes();
        var minutesLimit = levelData.isForever ? minutesForInfiniteLevel : minutesForNormalLevel;

        return timePassedInMinutes >  minutesLimit; 
    } 

    public void StartStage(){
        if (FinishedGame()){
            FinishGame();
            return;
        }

        isPlayingForever = levelData.isForever;

        

        shiftAShape = Random.Range(0, 1f) < levelData.probabilityToDifferentShape;

        shapeSpawner.Spawn();

        FindObjectOfType<MoveHandler>().Spawn();


        isPlaying = true;
        gameUiHandler.FixUi();
    }
    
    public void ResetColorfulValue(){FindObjectOfType<ShapeSpawner>().colorfulValue = 100f;}

    public void StartGame(){

        reactionTime = levelData.maxReactionTime;

        gameUiHandler.StartGame();

        ResetMovesIndicators();

        ResetColorfulValue();
    }

    public void ContinueGame(){

        // reactionTime = levelData.maxReactionTime;

        gameUiHandler.StartGame();
        StartStage();
        // ResetMovesIndicators();
    }
    
    public void ResetMovesIndicators(){
        rightMoves = 0;
        madeAtleastOneMistake = false;
    }
    
    void UpdateMovesIndicators(bool isLastMoveRight){
        rightMoves = isLastMoveRight ? rightMoves + 1 : 0;
        if (!isLastMoveRight) madeAtleastOneMistake = true;
    }


    public IEnumerator WinStage(){
        Debug.Log("you won the stage!");
        score++;

        gameUiHandler.WinStage();
        

        isPlaying = false;
        UpdateMovesIndicators(true);
        DestroyShapes();

        yield return new WaitForSeconds(2);

        stageNum++;

        GameGuard gameGuard = FindObjectOfType<GameGuard>();
       
        while (gameGuard.paused){
            yield return new WaitForSeconds(0.1f);
        }

        StartStage();
    }

    public IEnumerator LoseStage(){
        Debug.Log("you lost the stage..");

        gameUiHandler.LoseStage();

        isPlaying = false;
        UpdateMovesIndicators(false);
        DestroyShapes();

        yield return new WaitForSeconds(2);

        stageNum++;

        GameGuard gameGuard = FindObjectOfType<GameGuard>();
        
        while (gameGuard.paused){
            yield return new WaitForSeconds(0.1f);
        }

        StartStage();
    }


    public void DestroyShapes(){
        if (!chosenContent) return;

        foreach (Transform child in chosenContent.transform)
        {
            GetChildWithName(child, "shape").GetComponent<Shifter>().Disappear();
            // child.transform.GetChild(0).GetComponent<Shifter>().Disappear();
        }
    }    

    void FinishGame(){
        Debug.Log(string.Format("you finished the game! score: {0}",score));
        DestroyShapes();
        
        gameUiHandler.FinishGame();
    }

    // void InitLevel(){
    //     string path = "Prefabs/Shapes/";
    //     switch (levelData.shapesType)
    //     {
    //         case ShapeType.Arrows:
    //             path += "Arrow";
    //             break; 
    //         case ShapeType.Letters:
    //             path += "Letter";
    //             break;
    //         case ShapeType.Images:
    //             path += "Image";
    //             break;
    //     }
    //     levelData.spawningPrefab = Resources.Load<GameObject>(path + "-Prefab");
    //     Debug.Log(levelData.spawningPrefab ? levelData.spawningPrefab.name : "no collected");

    //     levelData.InitData();

    // }

    public void SetLevelData(LevelScriptableObject newLevelData){
        levelData = newLevelData;
        levelData.InitData();
        ResetStats();
        Debug.Log(string.Format("Game Handler: level {0} loaded ", levelData.levelNumber));
    }
}