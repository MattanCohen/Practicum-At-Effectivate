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
   
    private void Awake() {
        rightButtonText = rightButton.GetComponentInChildren<TMP_Text>();
        leftButtonText = leftButton.GetComponentInChildren<TMP_Text>();
        gameHandler = FindObjectOfType<GameHandler>();
        leftButtonText.text = same;
        rightButtonText.text = different;
    }

    public void Spawn(){
        if (shouldSwitchButtons)
        {
            leftButtonText.text = lastLeft == same ? different : same;
            rightButtonText.text = lastRight == same ? different : same;
            gameHandler.stepsSinceChange = 0;
        }
        else
        {
            gameHandler.stepsSinceChange ++;
        }
        
        // bool madeAtleastOneMistake      = gameHandler.madeAtleastOneMistake;
        // bool lastMoveWasRight           = gameHandler.lastMoveWasRight;
        // bool threeLastMovesWereRight    = gameHandler.threeLastMovesWereRight;
    }

    public void UpdateControls(){
        lastLeft = leftButtonText.text;
        lastRight = rightButtonText.text;

        bool canChange =   gameHandler.stepsSinceChange >= gameHandler.levelData.minStepsToSwitch;
        float switchChange = (float)gameHandler.levelData.switchButtonsChance / 100f;
        shouldSwitchButtons = Random.Range(0f, 1f) < switchChange && canChange ? true : false;   
 
        Debug.Log("handler chance: " + gameHandler.levelData.switchButtonsChance.ToString());
        Debug.Log("chance: " + switchChange.ToString() + " shouldSwitch? " + shouldSwitchButtons);
        
        
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
