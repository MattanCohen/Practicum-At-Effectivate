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
        bool lastMoveWasWrong = !FindObjectOfType<GameHandler>().lastMoveWasRight;
        bool madeAtleastOneMistake = FindObjectOfType<GameHandler>().madeAtleastOneMistake;
        
        if (isShifted && lastMoveWasWrong && madeAtleastOneMistake){
            var containerBackground = GameHandler.GetChildWithName(transform.parent, "background");
            Debug.Log("YOURE WRONG LOL ");
            if (containerBackground == null){
                transform.parent.GetChild(1).SetSiblingIndex(0);
                transform.parent.GetChild(0).gameObject.SetActive(true);
            }
            else
                containerBackground.GetComponent<Image>().color = wrongColor;

        }

        var container = GameHandler.GetChildWithName(transform.parent, "container");
        if (container == null){
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetBool("shouldDissapear", true);
            return;
        }
        
        container.GetComponent<Animator>().enabled = true;
        container.GetComponent<Animator>().SetBool("shouldDissapear", true);
    }
    public void Kill(){
        Destroy(transform.parent.gameObject);
    }
}