using UnityEngine;
using UnityEngine.UI;

public abstract class Shifter : MonoBehaviour {

    [SerializeField] GameObject markerGameObject;
    RandomBorders randomContentBorders;
    public bool isShifted;
    public Color wrongColor;

    private void Awake() {
        var markerGO = Instantiate(markerGameObject, transform.localPosition, new Quaternion (0,0,0,0));
        markerGO.transform.SetParent(transform.parent);
        markerGO.SetActive(false);

        GameHandler gameHandler = FindObjectOfType<GameHandler>();
        if (gameHandler.chosenContent != gameHandler.randomContent) return;
        
        randomContentBorders = gameHandler.randomContent.GetComponent<RandomBorders>();
    }

    public void RandomPosition(){
        transform.parent.localPosition = new Vector3(   
                                Random.Range(-randomContentBorders.Xborder, randomContentBorders.Xborder), 
                                Random.Range(-randomContentBorders.Yborder, randomContentBorders.Yborder), 
                                0f);
    }


    public abstract void Shift();
    
    public void Disappear(){
        bool lastMoveWasWrong = FindObjectOfType<GameHandler>().rightMoves == 0;
        if (isShifted && lastMoveWasWrong){
            var marker = GameHandler.GetChildWithName(transform.parent, "Marker(Clone)");
            if (marker != null){
                Debug.Log("showing marker");

                marker.transform.SetSiblingIndex(0);
                marker.transform.gameObject.SetActive(true);
            }
            else Debug.Log("have no marker");
        }

        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetBool("shouldDissapear", true);
        return;
    }
    public void Kill(){
        Destroy(transform.parent.gameObject);
    }
}