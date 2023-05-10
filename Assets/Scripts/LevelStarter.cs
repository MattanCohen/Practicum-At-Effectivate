using UnityEngine;

public class LevelStarter : MonoBehaviour {
    


    public void StartGame(){
        foreach (LevelSelector levelButton in GameObject.FindObjectsOfType<LevelSelector>())
        {
            if (!levelButton.selected)
                continue;
            
            GameObject.FindObjectOfType<GameGuard>().PreGame();
            GameObject.FindObjectOfType<GameHandler>().StartGame();
            Debug.Log(string.Format("level {0} seleceted", levelButton.gameObject.name));

            return;
        }

        Debug.Log("no level selected.");

    }
}