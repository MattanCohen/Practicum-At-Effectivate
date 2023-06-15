using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public bool selected;
    LevelScriptableObject levelData;
    private void Awake() {
        selected = false;
        foreach (var data in Resources.LoadAll<LevelScriptableObject>(""))
        {
            if (gameObject.name != data.levelNumber.ToString()) continue;

            levelData = data;
            break;
        }
        
        Text button_name = transform.GetChild(0).GetComponent<Text>();
        if (!levelData){
            button_name.text = "Y";
            return;
        }


        button_name.text = levelData.levelNumber.ToString();
    }

    private void FixedUpdate() {

        if (selected)
            return;
        
        var color = GetComponent<Image>().color;
        color.a = 180f/255f;
        GetComponent<Image>().color = color;
    }

    public void SelectLevel(){
        if (!levelData){
            return;
        }
        
        foreach (LevelSelector button in FindObjectsOfType<LevelSelector>())
        {
            button.selected = false;
        }

        selected = true;
        
        GameObject.FindObjectOfType<GameHandler>().SetLevelData(levelData);
        
        var color = GetComponent<Image>().color;
        color.a = 255f/255f;
        GetComponent<Image>().color = color;
        FindObjectOfType<SoundManager>().PlayButtonSoundEffect();

    }


}
