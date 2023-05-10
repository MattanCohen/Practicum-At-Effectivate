using UnityEngine;
using System.Collections;
using TMPro;

public class MoveHandler : MonoBehaviour
{
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    TMP_Text rightButtonText;
    TMP_Text leftButtonText;

    string same;
    string different;
    bool shouldSwitchButtons;
    GameHandler gameHandler;
   
    private void Awake() {
        rightButtonText = rightButton.GetComponentInChildren<TMP_Text>();
        leftButtonText = leftButton.GetComponentInChildren<TMP_Text>();
        gameHandler = FindObjectOfType<GameHandler>();
    }

    public void UpdateControls(){
        same = "דומה";
        different = "שונה";

        float switchChange = (float)gameHandler.levelData.switchButtonsChance / 100f;
        shouldSwitchButtons = Random.Range(0f, 1f) < switchChange ? true : false;        
        Debug.Log("handler chance: " + gameHandler.levelData.switchButtonsChance.ToString());
        Debug.Log("chance: " + switchChange.ToString() + " shouldSwitch? " + shouldSwitchButtons);
        
        if (shouldSwitchButtons)
        {
            leftButtonText.text = different;
            rightButtonText.text = same;
        }
        else
        {
            leftButtonText.text = same;
            rightButtonText.text = different;
        }
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
        
        // shouldSwitchButtons  =>  right button same
        if (shouldSwitchButtons)
            PlayMove(true);
        else
            PlayMove(false);
    }
    public void ClickLeftButton(){

        // shouldSwitchButtons  =>  left button different
        if (shouldSwitchButtons)
            PlayMove(false);
        else
            PlayMove(true);
    }
}
