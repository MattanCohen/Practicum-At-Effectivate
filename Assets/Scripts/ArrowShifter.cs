using UnityEngine;
using UnityEngine.UI;

public class ArrowShifter : Shifter {

    

    public override void Shift()
    {
        isShifted = true;
        
        var scale = transform.parent.localScale;
        scale.x *= -1;
        transform.parent.localScale = scale;
    }
    
    
}