using UnityEngine;
using System.Collections.Generic;

public class PostGameStats : MonoBehaviour {
    List<float> reactions;

    private void Awake() {
        reactions = new List<float>();    
    }

    public void AddReactionTime(float time){
        reactions.Add(time);
    }

    public float GetAverageReactionTime(){
        float sum = 0;
        foreach (float reaction in reactions)
        {
            sum += reaction;
        }
        return sum / (float)reactions.Count;
    }

    public float GetSuccessRate(){
        GameHandler handler = FindObjectOfType<GameHandler>();
        Debug.Log("getting suc rate");
        if (handler.stageNum == 0)
            return 0f;
        Debug.Log("suc rate = " + (float)handler.score / (float)handler.stageNum);
        return (float)handler.score / (float)handler.stageNum;
    }
}