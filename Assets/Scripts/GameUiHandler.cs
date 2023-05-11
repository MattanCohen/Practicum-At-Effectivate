using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PostGameStats))]
[RequireComponent(typeof(MoveHandler))]
public class GameUiHandler : MonoBehaviour {
    public GameObject duringGameUI;
    public GameObject postGameUI;
    public GameObject nextLevelButton;
    public GameObject pauseMenuButton;
    public TMP_Text muteButtonText;
    public TMP_Text preGameLevelText;
    public Slider duringGameReactionSlider;
    public TMP_Text duringGameStageCounterText;
    public TMP_Text duringGameScoreCounterText;
    public TMP_Text postGameScoreCounterText;
    public TMP_Text postGameLevelText;
    public TMP_Text averageReactionTimeText;
    public TMP_Text successRateText;
    float lastReactionTimeSet;
    GameHandler gameHandler;
    GameGuard gameGuard;
    int levelNum;
    
    public void ResetReactionTime(){
        lastReactionTimeSet = Time.realtimeSinceStartup;
    }
    
    void UpdateStageCounterText(){
        // duringGameStageCounterText.text = string.Format("{0} / {1}", gameHandler.stageNum + 1, gameHandler.levelData.itemsToSpawn.Length);
        duringGameStageCounterText.text = string.Format("{0}", gameHandler.stageNum);
}
    public void UpdateScoreCounterText(){
        duringGameScoreCounterText.text = string.Format("{0}", gameHandler.score);
        postGameScoreCounterText.text = string.Format("{0}", gameHandler.score);
    }
    public void UpdateLevelText(){
        preGameLevelText.text = string.Format("{0}", gameHandler.levelData.levelNumber);
        postGameLevelText.text = string.Format("{0}", gameHandler.levelData.levelNumber);
    }
    public void UpdateAverageReactionTimeText(){
        float ans = GetComponent<PostGameStats>().GetAverageReactionTime();
        averageReactionTimeText.text = string.Format("{0}", ans.ToString("F1"));
    }
    public void UpdateSuccessRate(){
        int ans = (int)(GetComponent<PostGameStats>().GetSuccessRate() * 100f);
        successRateText.text = string.Format("{0}%", ans.ToString());
    }


    private void Awake() {
        gameHandler = FindObjectOfType<GameHandler>();
        gameGuard = FindObjectOfType<GameGuard>();
        duringGameReactionSlider.value = 1;
    }

    void UpdatePauseMenu(){
        
        if (FinishedGame()) pauseMenuButton.SetActive(true);


        string newTextForMuteButton;
        if (FindObjectOfType<SoundManager>().muted)
            newTextForMuteButton = "צלילים כבויים";
        else
            newTextForMuteButton = "צלילים פועלים";

        muteButtonText.text = newTextForMuteButton;
    }

    bool FinishedGame(){
        // the game is finished if and only if:
        //      game handler finished the game and its the last level
        //  or  game handler finished the game and the level appended
        return gameHandler.FinishedGame() || gameHandler.levelData.levelNumber == levelNum + 1;
    }

    private void FixedUpdate() {
        UpdatePauseMenu();
        
        if (!gameHandler.isPlaying){
            // GetComponent<TimerScript>().PauseTimer();
            return;
        }            
        // GetComponent<TimerScript>().PlayTimer();
    

        pauseMenuButton.SetActive(false);

        gameGuard.DuringGame();

        if (Time.realtimeSinceStartup - lastReactionTimeSet > (float)gameHandler.reactionTime){
            StartCoroutine(gameHandler.LoseStage());
        }

        duringGameReactionSlider.value = 1 - (Time.realtimeSinceStartup - lastReactionTimeSet) / (float)gameHandler.reactionTime;
        UpdateStageCounterText();
        UpdateScoreCounterText();
    }

    bool LoadLevelNum(int n)
    {
        LevelScriptableObject level = Resources.Load<LevelScriptableObject>(string.Format("Levels/Level {0}", n));
        if (level == null){
            Debug.Log("game gui handler failed to load level, stay on the same level");
            return false;
        }
        Debug.Log(string.Format("Game gui handler succesfuly loaded level number {0}", level.levelNumber));

        gameHandler.SetLevelData(level);

        return true;

        // // Get all ScriptableObjects in the Levels folder
        // string[] guids =   AssetDatabase.FindAssets("t:LevelScriptableObject", new[] { "Assets/Levels" });

        // // Loop through each ScriptableObject to find the one with the correct levelNum
        // foreach (string guid in guids)
        // {
        //     string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        //     level = AssetDatabase.LoadAssetAtPath<LevelScriptableObject>(assetPath);

        //     if (level.levelNumber == n)
        //     {
        //         gameHandler.SetLevelData(level);
        //         return true;
        //     }
        // }
        // return false;
    }

    public void FixUi(){
        ResetReactionTime();
        UpdateStageCounterText();
        UpdateScoreCounterText();
        UpdateLevelText();
        UpdateAverageReactionTimeText();
    }

    float GetReactionTime(){
        return Time.realtimeSinceStartup - lastReactionTimeSet;
    }
    
    public void WinStage(){
        UpdateScoreCounterText();
        GetComponent<PostGameStats>().AddReactionTime(GetReactionTime());
    }
    
    public void LoseStage(){
        FindObjectOfType<SoundManager>().PlayWrongAnswerEffect();
        GetComponent<PostGameStats>().AddReactionTime(GetReactionTime());
    }

    public void NextLevel(){
        nextLevelButton.SetActive(false);
        FixUi();
        gameHandler.StartGame();
        gameGuard.PreGame();
    }

    public void StartGame(){
        levelNum = gameHandler.levelData.levelNumber;
        FixUi();
        GetComponent<MoveHandler>().UpdateControls();
        GetComponent<TimerScript>().StartTimer();
    }

    public void FinishGame(){
        UpdateSuccessRate();
        
        FixUi();
        
        if (LoadLevelNum(gameHandler.levelData.levelNumber + 1)){
            nextLevelButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "לשלב הבא";
            // nextLevelButton.SetActive(true);
        }
        else{
            nextLevelButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "המשך שלב";
            // nextLevelButton.SetActive(false);
        }


        gameGuard.PostGame();
    }
    public void ResetGame(){
        LoadLevelNum(levelNum);
        NextLevel();
    }

    public void MainMenu(){
        nextLevelButton.SetActive(false);
        gameGuard.MainMenu();
    }

    public void PressMuteButton(){
        FindObjectOfType<SoundManager>().MuteUnMute();
    }
}
