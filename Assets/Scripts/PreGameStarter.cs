using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameStarter : MonoBehaviour
{
    public void StartGame()
    {
        FindObjectOfType<GameUiHandler>().StartGame();
        FindObjectOfType<GameHandler>().StartStage();
    }
}
