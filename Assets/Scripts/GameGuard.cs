using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameMode{
    Pre,
    During,
    Post
}

public class GameGuard : MonoBehaviour
{
    
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject preGame;
    [SerializeField] GameObject duringGame;
    [SerializeField] GameObject postGame;
    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameHandler gameHandler;

    [SerializeField] GameObject levelsParent;
    GameMode pausedMode;
    public bool paused{get; private set;}

    void Awake()
    {
        MainMenu();

    }
    

    public void MainMenu(){
        BackFromPauseMenu();

        levelSelect.SetActive(true);
        
        gameUI.SetActive(false);
        /*child of game ui: */ preGame.SetActive(true);
        /*child of game ui: */ duringGame.SetActive(false);
        /*child of game ui: */ postGame.SetActive(false);

        foreach (var level in Resources.LoadAll<LevelScriptableObject>(""))
        {
            level.InitData();
        }

        foreach (LevelSelector levelButton in FindObjectsOfType<LevelSelector>())
        {
            levelButton.selected = false;
        }
 

        foreach (Transform levelContainer in levelsParent.transform)
        {
            levelContainer.gameObject.SetActive(true);
            levelContainer.gameObject.SetActive(false);
        }
        levelsParent.transform.GetChild(0).gameObject.SetActive(true);

        gameHandler.ResetStats();
        
        var chosenContent = gameHandler.chosenContent;
        if (!chosenContent) return;

        foreach (Transform child in chosenContent.transform)
        {
            child.transform.GetChild(0).GetComponent<Shifter>().Kill();
        }
        
    }

    public void PreGame(){
        BackFromPauseMenu();
        levelSelect.SetActive(false);
        
        /*child of game ui: */ preGame.SetActive(true);
        /*child of game ui: */ duringGame.SetActive(false);
        /*child of game ui: */ postGame.SetActive(false);
        /*child of game ui: */ pauseMenu.SetActive(false);
        gameUI.SetActive(true);
    }
    public void DuringGame(){
        BackFromPauseMenu();
        levelSelect.SetActive(false);
        
        /*child of game ui: */ preGame.SetActive(false);
        /*child of game ui: */ duringGame.SetActive(true);
        /*child of game ui: */ postGame.SetActive(false);
        /*child of game ui: */ pauseMenu.SetActive(false);
        gameUI.SetActive(true);
    }
    public void PostGame(){
        BackFromPauseMenu();
        levelSelect.SetActive(false);
        
        /*child of game ui: */ preGame.SetActive(false);
        /*child of game ui: */ duringGame.SetActive(false);
        /*child of game ui: */ postGame.SetActive(true);
        /*child of game ui: */ pauseMenu.SetActive(false);
        gameUI.SetActive(true);
    }
    
    void BackFromPauseMenu(){
        /*child of game ui: */ pauseMenu.SetActive(false);
        paused = false;
    }

    public void PauseGame(){
        if (paused){
            UnPauseGame();
            return;
        }


        levelSelect.SetActive(false);

        if (preGame.activeSelf) pausedMode = GameMode.Pre;
        /*child of game ui: */ preGame.SetActive(false);
                 
        if (duringGame.activeSelf) pausedMode = GameMode.During;
        /*child of game ui: */ duringGame.SetActive(false);
        
        if (postGame.activeSelf) pausedMode = GameMode.Post;
        /*child of game ui: */ postGame.SetActive(false);
        
        /*child of game ui: */ pauseMenu.SetActive(true);
        
        gameUI.SetActive(true);

        paused = true;
    } 
    void UnPauseGame(){

        switch (pausedMode)
        {
            case GameMode.Pre:
                PreGame();
                break;
            case GameMode.During:
                DuringGame();
                break;
            case GameMode.Post:
                PostGame();
                break;
        }
        BackFromPauseMenu();
    }
    
}
