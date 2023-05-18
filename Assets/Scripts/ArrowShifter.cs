using UnityEngine;
using UnityEngine.UI;

public class ArrowShifter : Shifter {

    

    public override void Shift()
    {
        var scale = transform.parent.localScale;
        scale.x *= -1;
        transform.parent.localScale = scale;
        isShifted = true;
    }
    
    
}