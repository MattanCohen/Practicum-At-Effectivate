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
    [HideInInspector] public bool lastMoveWasRight{get; private set;}
    [HideInInspector] public bool previousMoveWasRight{get; private set;}
    [HideInInspector] public bool madeAtleastOneMistake{get; private set;}
    [HideInInspector] public bool threeLastMovesWereRight{get; private set;}
    [HideInInspector] public int stepsSinceChange;
    [HideInInspector] public float reactionTime;   
    public float minutesForEachLevel;
    public float minutesForInfiniteLevel;

    ShapeSpawner shapeSpawner;

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
    


    // public bool FinishedGame(){return !isPlayingForever && FindObjectOfType<TimerScript>().GetTotalTimeInMinutes() > minutesForEachLevel;} 
    public bool FinishedGame(){return FindObjectOfType<TimerScript>().GetTotalTimeInMinutes() > minutesForEachLevel;} 


    public void StartStage(){
        if (FinishedGame()){
            FinishGame();
            return;
        }

        if (isPlayingForever)   minutesForEachLevel = 0.7f;
        else                    minutesForEachLevel = 1.5f;

        

        shiftAShape = Random.Range(0, 1f) < levelData.probabilityToDifferentShape;

        shapeSpawner.Spawn();

        FindObjectOfType<MoveHandler>().Spawn();


        isPlaying = true;
        gameUiHandler.FixUi();
    }
    
    public void StartGame(){

        reactionTime = levelData.maxReactionTime;

        gameUiHandler.StartGame();

        ResetMovesIndicators();
    }

    public void ContinueGame(){

        // reactionTime = levelData.maxReactionTime;

        gameUiHandler.StartGame();
        StartStage();
        // ResetMovesIndicators();
    }
    
    public void ResetMovesIndicators(){
        previousMoveWasRight = false;
        lastMoveWasRight = false;
        madeAtleastOneMistake = false;
        threeLastMovesWereRight = false;
    }
    
    void UpdateMovesIndicators(bool isLastMoveRight){
        /*
            before assigning new values this is the state of the variables:
                isLastMoveRight      := last move was right
                lastMoveWasRight     := previous move was right
                previousMoveWasRight := 2 moves ago was right
        */
        if (isLastMoveRight && lastMoveWasRight && previousMoveWasRight)
            threeLastMovesWereRight = true;
        else
            threeLastMovesWereRight = false;

        // update variables to contain real data
        previousMoveWasRight = lastMoveWasRight;
        lastMoveWasRight = isLastMoveRight;

        // in case that is the first mistake
        if (!isLastMoveRight)
            madeAtleastOneMistake = true;
        
    }


    public IEnumerator WinStage(){
        Debug.Log("you won the stage!");
        score++;

        gameUiHandler.WinStage();
        

        isPlaying = false;
        DestroyShapes();

        yield return new WaitForSeconds(2);

        stageNum++;

        GameGuard gameGuard = FindObjectOfType<GameGuard>();
       
        while (gameGuard.paused){
            yield return new WaitForSeconds(0.1f);
        }

        UpdateMovesIndicators(true);
        StartStage();
    }

    public IEnumerator LoseStage(){
        Debug.Log("you lost the stage..");

        gameUiHandler.LoseStage();

        isPlaying = false;
        DestroyShapes();

        yield return new WaitForSeconds(2);

        stageNum++;

        GameGuard gameGuard = FindObjectOfType<GameGuard>();
        
        while (gameGuard.paused){
            yield return new WaitForSeconds(0.1f);
        }

        UpdateMovesIndicators(false);
        StartStage();
    }


    public void DestroyShapes(){
        if (!chosenContent) return;

        foreach (Transform child in chosenContent.transform)
        {
            child.transform.GetChild(0).GetComponent<Shifter>().Disappear();
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