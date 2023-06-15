using UnityEngine;
using UnityEngine.UI;

public class ImageShifter : Shifter {

    float grayscaleValue;
    Material mat;
    public void ChooseSprite(float grayscale, Sprite chosenSprite, bool shouldShift){
        isShifted = shouldShift;
        this.transform.GetComponent<Image>().sprite = chosenSprite;   
        mat = GetComponent<Image>().material;   
        grayscaleValue = 1f - grayscale / 100f;
        mat.SetFloat("_GrayscaleAmount", grayscaleValue);
    }

    public override void Shift(){}


    
}