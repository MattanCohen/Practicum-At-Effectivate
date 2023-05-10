using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;

public enum Difficulty
{
    Geometric,
    Semantic
}


public class Imager : MonoBehaviour {
    string FOLDERS = "folders";    
    string FOLDER_NAME = "folder-name";    
    string COLORS = "colors";

    [SerializeField] TextAsset jsonFile;
    Dictionary<string, Sprite[]> geometricSprites;
    
    private void Awake() {
        string imagesFolder = "Images/";

        geometricSprites = new Dictionary<string, Sprite[]>();
        
        string jsonString = jsonFile.text;
        JSONNode data = JSON.Parse(jsonString);
        foreach (JSONNode imageType in data[FOLDERS])
        {
            string folderName = imageType[FOLDER_NAME].Value;
            Sprite [] folderSprites = Resources.LoadAll<Sprite>(imagesFolder + folderName);
            geometricSprites.Add(folderName, folderSprites);
        }
    }

    Sprite [] GetTwoRandomGeometrics(){

        string geo1 = geometricSprites.Keys.ToArray()[Random.Range(0, geometricSprites.Keys.Count)];
        string geo2 = "";
        do
        {
            geo2 = geometricSprites.Keys.ToArray()[Random.Range(0, geometricSprites.Keys.Count)];
        } while (geo1 == geo2);
        
        Sprite [] sprites = {
            geometricSprites[geo1][Random.Range(0, geometricSprites[geo1].ToArray().Length)],
            geometricSprites[geo2][Random.Range(0, geometricSprites[geo2].ToArray().Length)]
        };
        return sprites;
    }
    Sprite [] GetTwoRandomSemantics(){
        string geo = geometricSprites.Keys.ToArray()[Random.Range(0, geometricSprites.Keys.Count)];
        
        Sprite s1 = geometricSprites[geo][Random.Range(0, geometricSprites[geo].ToArray().Length)];
        Sprite s2 = s1;
        do
        {
            s2 = geometricSprites[geo][Random.Range(0, geometricSprites[geo].ToArray().Length)];
        } while (s1 == s2);

        Sprite [] sprites = { s1, s2 };
        return sprites;
    }

    public Sprite [] GetTwoRandomSprites(ShapeSpawner.ImagesType d){
        switch (d)
        {
            case ShapeSpawner.ImagesType.Geometric:
                return GetTwoRandomGeometrics();
            case ShapeSpawner.ImagesType.Semantic:
                return GetTwoRandomSemantics();
        }

        return null;
    }

}