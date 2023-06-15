using UnityEngine;
using UnityEngine.UI;

public class SemanticShifter : Shifter {
    [SerializeField] Text shapeText;
    public string [] variations;
    string [] usingLetters;

    public void SetVariation(int i){
        usingLetters = variations[i].Split();
        shapeText.text = usingLetters[0];
    }

    public override void Shift(){
        shapeText.text = usingLetters[1];
    }
}