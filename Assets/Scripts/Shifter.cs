using UnityEngine;


public abstract class Shifter : MonoBehaviour {

    RandomBorders randomContentBorders;

    private void Awake() {
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
    
    public void Dissapear(){
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetBool("shouldDissapear", true);
    }
    public void Kill(){
        Destroy(transform.parent.gameObject);
    }
}