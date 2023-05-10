using UnityEngine;
using TMPro;

public class SemanticShifter : Shifter {
    [SerializeField] TMP_Text shapeText;
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