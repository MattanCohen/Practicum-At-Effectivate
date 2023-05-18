using UnityEngine;
using System.Collections;
using TMPro;

public class MoveHandler : MonoBehaviour
{
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    TMP_Text rightButtonText;
    TMP_Text leftButtonText;
    string same = "דומה";
    string different = "שונה";

    bool shouldSwitchButtons;

    string lastLeft;
    string lastRight;
    GameHandler gameHandler;

    int lastMinStepsToSwitch;
    int lastSwitchButtonsChance;
            
   
    private void Awake() {
        rightButtonText = rightButton.GetComponentInChildren<TMP_Text>();
        leftButtonText = leftButton.GetComponentInChildren<TMP_Text>();
        gameHandler = FindObjectOfType<GameHandler>();
        leftButtonText.text = same;
        rightButtonText.text = different;
        lastMinStepsToSwitch = gameHandler.levelData.minStepsToSwitch;
        lastSwitchButtonsChance = gameHandler.levelData.switchButtonsChance;
    }

    public void Spawn(){
        if (shouldSwitchButtons)
        {
            leftButtonText.text = lastLeft == same ? different : same;
            rightButtonText.text = lastRight == same ? different : same;
            gameHandler.stepsSinceChange = 0;
            FindObjectOfType<SoundManager>().PlaySwitchEffect();
        }
        else
        {
            gameHandler.stepsSinceChange ++;
        }
        
        // bool madeAtleastOneMistake      = gameHandler.madeAtleastOneMistake;
        // bool lastMoveWasRight           = gameHandler.lastMoveWasRight;
        // bool threeLastMovesWereRight    = gameHandler.threeLastMovesWereRight;
    }

    void CheckForInfinitePlaying(){
        if (!gameHandler.isPlayingForever) return;

        int precentDelta = 5;

        bool rand = Random.Range(0f, 1f) <= 0.5f; 

        MinMax switchChance = gameHandler.levelData.infiniteSwitchChance;
        MinMax stepsToSwitch = gameHandler.levelData.infiniteStepsToSwitch;

        bool lastMoveWasRight           = gameHandler.lastMoveWasRight;
        bool threeLastMovesWereRight    = gameHandler.threeLastMovesWereRight;

        if (rand){
            gameHandler.levelData.switchButtonsChance = Random.Range(switchChance.min, switchChance.max);
            
            gameHandler.levelData.minStepsToSwitch = Random.Range(stepsToSwitch.min, stepsToSwitch.max);

        }
        else{
            // get back to normal before rand if rand happened
            gameHandler.levelData.switchButtonsChance = lastSwitchButtonsChance;
            gameHandler.levelData.minStepsToSwitch = lastMinStepsToSwitch;


            // switch button chance
            bool canIncreaseSwitchChance = gameHandler.levelData.switchButtonsChance + precentDelta <= switchChance.max;
            bool canDecreaseSwitchChance = gameHandler.levelData.switchButtonsChance - precentDelta >= switchChance.min;
            // steps to switch
            bool canIncreaseSteps = gameHandler.levelData.minStepsToSwitch + 1 <= stepsToSwitch.max;
            bool canDecreaseSteps = gameHandler.levelData.minStepsToSwitch - 1 >= stepsToSwitch.min;

            for (int i = 0; i < precentDelta; i++)
            {
                // switch button chance
                gameHandler.levelData.switchButtonsChance +=    lastMoveWasRight && canIncreaseSwitchChance   ? 1 
                                                            :   !lastMoveWasRight && canDecreaseSwitchChance  ? -1
                                                            :   0;
                // steps to switch
            
            }
            

            gameHandler.levelData.minStepsToSwitch +=    lastMoveWasRight && canDecreaseSteps       ? -1 
                                                        :   !lastMoveWasRight && canIncreaseSteps   ? -1
                                                        :   0;
        }

    }

    public void UpdateControls(){
        lastLeft = leftButtonText.text;
        lastRight = rightButtonText.text;
        
        CheckForInfinitePlaying();
        bool canChange =   gameHandler.stepsSinceChange >= gameHandler.levelData.minStepsToSwitch;
        float switchChange = (float)gameHandler.levelData.switchButtonsChance / 100f;
        shouldSwitchButtons = Random.Range(0f, 1f) < switchChange && canChange ? true : false;   
 
        
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left"))
            ClickLeftButton();
        else if (Input.GetKeyDown("right"))
            ClickRightButton();
    }

    void PlayMove(bool isVotingSame){
        if (!gameHandler.isPlaying)
            return;

        Debug.Log(string.Format("played the move: {0}", isVotingSame ? "same" : "different"));
        bool correctAnswer =    (!isVotingSame && gameHandler.shiftAShape) ||   
                                (isVotingSame && !gameHandler.shiftAShape);
        if (correctAnswer){
            StartCoroutine(gameHandler.WinStage());
            FindObjectOfType<SoundManager>().PlayCorrectAnswerEffect();
        }
        else{
            StartCoroutine(gameHandler.LoseStage());
            FindObjectOfType<SoundManager>().PlayWrongAnswerEffect();
        }
        UpdateControls();
    }

    public void ClickRightButton(){
        PlayMove(rightButtonText.text == same);
    }
    public void ClickLeftButton(){
        PlayMove(leftButtonText.text == same);
    }
}
