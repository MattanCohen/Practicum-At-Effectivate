using UnityEngine;
using UnityEngine.UI;

public class ImageShifter : Shifter {

    public void ChooseSprite(Sprite chosenSprite){
        this.transform.GetComponent<Image>().sprite = chosenSprite;   
    }

    public override void Shift(){}

    
    
}