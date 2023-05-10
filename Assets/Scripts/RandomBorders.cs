using UnityEngine;

public class RandomBorders : MonoBehaviour {
    public float Xborder{get; private set;}
    public float Yborder{get; private set;}

    private void Awake() {
        var sizeDelta = GetComponent<RectTransform>().sizeDelta;
        Xborder = sizeDelta.x / 2f;
        Yborder = sizeDelta.y / 2f;
    }

}