using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSpawner : MonoBehaviour
{
    public enum ImagesType
    {
        Geometric,
        Semantic,
        Geometric_Semantic
    }
    GameHandler gameHandler;
    int itemsToSpawn;
    int columnMaxItemsToSpawn = 11;
    int lineMaxItemsToSpawn = 13;
    int spawnningIndex;
    
    float spawnStart;
    float levelDuration;
    // float scaleFactor;
    



    float GetScaleFactor(int numberToSpawn){
        bool isImage = gameHandler.levelData.shapesType == ShapeType.Images;
        // return isImage ? image_scale : arrow_scale;
        // end of test


        float scaleFactor = 1f;
        float spacing = 50;

        if (gameHandler.chosenContent == gameHandler.randomContent){
            // do nothing
        }
        else if (gameHandler.chosenContent == gameHandler.columnContent){
            VerticalLayoutGroup verticalLayoutGroup = gameHandler.chosenContent.GetComponent<VerticalLayoutGroup>();
            if (!isImage){
                // TODO
                switch (numberToSpawn)
                {
                case (<= 11):
                    scaleFactor = 1f;
                    spacing = 50f;
                    break;
                case (<= 17):
                    scaleFactor = 0.7f;
                    spacing = 0f;
                    break;
                default:
                    scaleFactor = 0.6f;
                    spacing = -18f;
                    break;
                }
                verticalLayoutGroup.spacing = spacing;
            }
            else{
                switch (numberToSpawn)
                {
                    case (< 9):
                        scaleFactor = 1.8f;
                        spacing = 80f;
                        break;
                    case (< 13):
                        scaleFactor = 1.7f;
                        spacing = 60f;
                        break;
                    case (<= 17):
                        scaleFactor = 1.2f;
                        spacing = 20f;
                        break;
                    default:
                        scaleFactor = 0.8f;
                        spacing = -30f;
                        break;
                }
                verticalLayoutGroup.spacing = spacing;
            }
        }
        else if (gameHandler.chosenContent == gameHandler.rowContent){
            HorizontalLayoutGroup horizontalLayoutGroup = gameHandler.chosenContent.GetComponent<HorizontalLayoutGroup>();
    
            if (!isImage) {
                switch (numberToSpawn)
                {
                    case (<=11):
                        scaleFactor = 1.5f;
                        spacing = 100f;
                        break;
                    case (<=15):
                        scaleFactor = 1f;
                        spacing = 50f;
                        break;
                    default:
                        scaleFactor = 0.7f;
                        spacing = 0f;
                        break;
                }
            }
            else{
                switch (numberToSpawn)
                {
                    case (<13):
                        scaleFactor = 2f;
                        spacing = 100f;
                        break;
                    case (<=17):
                        scaleFactor = 1.2f;
                        spacing = 20f;
                        break;
                    default:
                        scaleFactor = 1f;
                        spacing = 0f;
                        break;
                }
            }

            horizontalLayoutGroup.spacing = spacing;
        }
        return scaleFactor;
    }
    // Start is called before the first frame update
    
    void Start()
    {
        gameHandler = GetComponent<GameHandler>();

        levelDuration = 1.5f * 60f; // levelDuration = one minute and 30 seconds
    }
    bool TouchingOtherShape(GameObject newShape){

        var size = newShape.GetComponent<RectTransform>().sizeDelta;
        var newShapePos = newShape.transform.localPosition;

        foreach (Transform shape in gameHandler.chosenContent.transform)
        {
            if (shape == newShape.transform) continue;

            var childPos = shape.localPosition;

            bool touchingInfront = Mathf.Abs(childPos.x - newShapePos.x) < size.x + 15; 
            bool touchingOnTop = Mathf.Abs(childPos.y - newShapePos.y) < size.y + 15; 

            if (touchingInfront && touchingOnTop)
            {
                return true;
            }   
        }

        return false;
    }

    void SpawnArrows(){
        GameObject prefabToSpawn = gameHandler.levelData.spawningPrefab;
        
        var scaleFactor = GetScaleFactor(itemsToSpawn);
        var scaleX = Random.Range(0f, 1f) > 0.5 ? 1 : -1;
        var scale = new Vector3(scaleX * scaleFactor, scaleFactor , scaleFactor);

        var rotationZ = Random.Range(0f, 1f) > 0.5 ? 90 : -90;
        var rotation = new Quaternion(0,0,0,rotationZ);

        int indexToShift = Random.Range(1, itemsToSpawn);
        
        for (int i = 0; i < itemsToSpawn; i++){
            
            // must have in every spawn loop
            GameObject newShape = Instantiate(prefabToSpawn);
            newShape.transform.SetParent(gameHandler.chosenContent.transform);

            // arrows changes
            newShape.transform.localScale = scale;
            if (gameHandler.chosenContent == gameHandler.columnContent)
                newShape.transform.localRotation = rotation;

            // must have in every spawn loop
            if (gameHandler.chosenContent == gameHandler.randomContent){
                do {newShape.transform.GetChild(0).GetComponent<Shifter>().RandomPosition();}
                while (TouchingOtherShape(newShape));
            }

            // must have in every spawn loop
            if (i == indexToShift){
                if (gameHandler.shiftAShape)
                    newShape.transform.GetChild(0).GetComponent<Shifter>().Shift();
            }
        }
    }

    // public void SpawnLetters(){
    //     GameObject prefabToSpawn = gameHandler.levelData.spawningPrefab;
    //     int randomVariation = Random.Range(0, gameHandler.levelData.spawningPrefab.transform.GetChild(0) .GetComponent<SemanticShifter>().variations.Length);
    //     int indexToShift = Random.Range(1, itemsToSpawn);

    //     for (int i = 0; i < itemsToSpawn; i++){
    //         // must have in every spawn loop
    //         GameObject newShape = Instantiate(prefabToSpawn);
    //         newShape.transform.SetParent(gameHandler.chosenContent.transform);


                
    //         // must have in every spawn loop
    //         if (gameHandler.chosenContent == gameHandler.randomContent){
    //             while (TouchingOtherShape(newShape));
    //         }

    //         float scaleFactor = GetScaleFactor();
    //         newShape.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);

    //         newShape.transform.GetChild(0).GetComponent<SemanticShifter>().SetVariation(randomVariation);

    //         // must have in every spawn loop
    //         if (i == indexToShift){
    //             if (gameHandler.shiftAShape)
    //                 newShape.transform.GetChild(0).GetComponent<Shifter>().Shift();
    //         }
    //     }
    // }

    // Sprite [] GetSprites(ImagesType imagesType){
    //     // return FindObjectOfType<Imager>().GetTwoRandomSprites(imagesType);


    //     List<Sprite> sprites = new List<Sprite>();

    //     ImageSelector imageSelector = FindObjectOfType<ImageSelector>();

    //     switch (imagesType)
    //     {
            
    //         case ImagesType.Geometric:
                
    //             string semantic1 = imageSelector.GetRandomSemantic();
    //             string semantic2;
    //             do
    //             {
    //                 semantic2 = imageSelector.GetRandomSemantic();
    //             } while (semantic1 == semantic2);

    //             sprites.Add(imageSelector.GetSpriteFromImageName(semantic1));
    //             sprites.Add(imageSelector.GetSpriteFromImageName(semantic2));
                
    //             break;


    //         case ImagesType.Semantic:
    //             string geometricName = imageSelector.GetRandomGeometric();
                
    //             string image1 = imageSelector.GetRandomSemanticFromGeometric(geometricName);
    //             string image2;
    //             do
    //             {
    //                 image2 = imageSelector.GetRandomSemanticFromGeometric(geometricName);
    //             } while (image1 == image2);

    //             sprites.Add(imageSelector.GetSpriteFromImageName(image1));
    //             sprites.Add(imageSelector.GetSpriteFromImageName(image2));

    //             break;
    //     }

    //     return sprites.ToArray();
    // }

    void SpawnPureSemantic(){
        Sprite [] sprites = FindObjectOfType<Imager>().GetNumberSemanticsOneGeometric(itemsToSpawn);
        Sprite shiftedSprite = sprites[sprites.Length - 1];
        
        GameObject prefabToSpawn = gameHandler.levelData.spawningPrefab;
        int indexToShift = Random.Range(1, sprites.Length - 1);
        
        for (int i = 0; i < sprites.Length - 1; i++){
            // must have in every spawn loop
            GameObject newShape = Instantiate(prefabToSpawn);
            newShape.transform.SetParent(gameHandler.chosenContent.transform);
            // ImageShifter imageShifter = newShape.transform.GetChild(0).GetComponent<ImageShifter>();
            ImageShifter imageShifter = newShape.GetComponentInChildren<ImageShifter>();

            // must have in every spawn loop
            if (gameHandler.chosenContent == gameHandler.randomContent){
                // do {newShape.transform.GetChild(0).GetComponent<Shifter>().RandomPosition();}
                do {imageShifter.RandomPosition();}
                while (TouchingOtherShape(newShape));
            }
            
            
            float scaleFactor = GetScaleFactor(sprites.Length - 1);
            newShape.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);

            if (i == indexToShift && gameHandler.shiftAShape)
                imageShifter.ChooseSprite(shiftedSprite, true);
            else
                imageShifter.ChooseSprite(sprites[i], false);

        }
    }
   
     public void SpawnImages(ImagesType imagesType){
        if (imagesType == ImagesType.Semantic){
            SpawnPureSemantic();
            return;
        }
        
        GameObject prefabToSpawn = gameHandler.levelData.spawningPrefab;
        int indexToShift = Random.Range(1, itemsToSpawn);

        Sprite [] sprites = FindObjectOfType<Imager>().GetTwoRandomSprites(imagesType);

        Sprite normalSprite = sprites[0];
        Sprite shiftedSprite = sprites[1];

        for (int i = 0; i < itemsToSpawn; i++){
            // must have in every spawn loop
            GameObject newShape = Instantiate(prefabToSpawn);
            newShape.transform.SetParent(gameHandler.chosenContent.transform);
            // ImageShifter imageShifter = newShape.transform.GetChild(0).GetComponent<ImageShifter>();
            ImageShifter imageShifter = newShape.GetComponentInChildren<ImageShifter>();

            // must have in every spawn loop
            if (gameHandler.chosenContent == gameHandler.randomContent){
                // do {newShape.transform.GetChild(0).GetComponent<Shifter>().RandomPosition();}
                do {imageShifter.RandomPosition();}
                while (TouchingOtherShape(newShape));
            }
            
            
            float scaleFactor = GetScaleFactor(itemsToSpawn);
            newShape.transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);

            if (i == indexToShift && gameHandler.shiftAShape)
                imageShifter.ChooseSprite(shiftedSprite, true);
            else
                imageShifter.ChooseSprite(normalSprite, false);

        }
    }

    public bool FinishedGame(){
        return Time.realtimeSinceStartup - spawnStart >= levelDuration;
    }

    void GetItemsToSpawn(){

        bool lessThanTwoMoves = gameHandler.stageNum < 2;

        var maxSpawn = gameHandler.levelData.maxItemsToSpawn;
        var minSpawn = gameHandler.levelData.minItemsToSpawn;

        if (lessThanTwoMoves){
            itemsToSpawn = minSpawn;
            return;
        }

        bool lastMoveWasWrong = gameHandler.rightMoves == 0;
        bool twoLastMovesRight = gameHandler.rightMoves >= 2;
        
        itemsToSpawn += lastMoveWasWrong 
                            ? -2 
                            : twoLastMovesRight 
                                ? +2 
                                : 0;

        itemsToSpawn = Mathf.Clamp(itemsToSpawn, minSpawn, maxSpawn); 
        
        Debug.Log("spawning " + itemsToSpawn + " items.");
        
    }

    void UpdateReactionTime(){
        bool madeAtleastOneMistake      = gameHandler.madeAtleastOneMistake;
        bool lastMoveWasWrong           = gameHandler.rightMoves == 0;
        bool threeLastMovesWereRight    = gameHandler.rightMoves >= 3;

        float multiplier = 1f;

        if (madeAtleastOneMistake)
        {
            if (lastMoveWasWrong)
                // made atleast one mistake and last move was wrong         : up by 5%
                multiplier = 1.05f; 
            else if (threeLastMovesWereRight)
                // made atleast one mistake and last three moves were right : down by 10%
                multiplier = 0.9f;
        }
        else
        {
            if (!lastMoveWasWrong)
                // didn't make any mistakes and last move was right         : down by 5% 
                multiplier = 0.95f;
        }

        var newReactionTime = gameHandler.reactionTime * multiplier;

        if (newReactionTime < gameHandler.levelData.maxReactionTime && newReactionTime > gameHandler.levelData.minReactionTime)
            gameHandler.reactionTime = newReactionTime;

        Debug.Log("new reaction time: " + gameHandler.reactionTime);

    }

    void ChooseContent(){
        
        List<string> whatToSpawn = new List<string>();

        
        if (itemsToSpawn <= columnMaxItemsToSpawn && gameHandler.levelData.spawnColumn)
            whatToSpawn.Add("column");
        if (itemsToSpawn <= lineMaxItemsToSpawn && gameHandler.levelData.spawnLine)
            whatToSpawn.Add("line");
        if (gameHandler.levelData.spawnRandom)
            whatToSpawn.Add("random");

        if (whatToSpawn.Count == 0) whatToSpawn.Add("line");

        string randomSummon = whatToSpawn.ToArray()[Random.Range(0, whatToSpawn.ToArray().Length)];

        switch (randomSummon)
        {
            case "column":
                gameHandler.chosenContent = gameHandler.columnContent;
                break;
            case "line":
                gameHandler.chosenContent = gameHandler.rowContent;
                break;
            case "random":
                gameHandler.chosenContent = gameHandler.randomContent;
                break;
        }
    }

    void CheckAndSetArrowChanges(){
        float arrowChange = (float)gameHandler.levelData.spawnArrowsChance / 100f;
        bool shouldSpawnArrows = Random.Range(0f, 1f) < arrowChange ? true : false;  
        gameHandler.levelData.shapesType = shouldSpawnArrows ? ShapeType.Arrows : ShapeType.Images;
        gameHandler.levelData.SetSpawningPrefab();
    }

    void CheckForInfinite(){
        if (!gameHandler.isPlayingForever) return;
    
        bool [] picks = {false, false, false};
        picks[Random.Range(0, picks.Length)] = true;
        
        gameHandler.levelData.geometric_semanticImages  = picks[0];
        gameHandler.levelData.geometricImages           = picks[1];
        gameHandler.levelData.semanticImages            = picks[2];

    }

    public void Spawn(){

        GetItemsToSpawn();
        UpdateReactionTime();
        ChooseContent();
        CheckAndSetArrowChanges();
        CheckForInfinite();


        switch (gameHandler.levelData.shapesType)
        {
            case ShapeType.Arrows:
                SpawnArrows();
                break;

            // case ShapeType.Letters:
            //     SpawnLetters();
            //     break;

            case ShapeType.Images:
                if (gameHandler.levelData.geometric_semanticImages)
                {
                    SpawnImages(ImagesType.Geometric_Semantic);
                }
                else if (gameHandler.levelData.semanticImages)
                {
                    SpawnImages(ImagesType.Semantic); 
                }
                else
                {
                    SpawnImages(ImagesType.Geometric);
                }
                break;
        }

    }


}



