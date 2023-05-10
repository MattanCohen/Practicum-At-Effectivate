using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ImageSelector : MonoBehaviour {


    GeometryTypeScriptableObject [] GetAllGeometricScriptableObjects(){
        return Resources.LoadAll<GeometryTypeScriptableObject>("");
    }

    string [] GetAllGeometricTypesNames(){
        List<string> stringTypes = new List<string>();

        foreach(GeometryTypeScriptableObject geometricType in GetAllGeometricScriptableObjects()){
            stringTypes.Add(geometricType.typeName);
        }

        return stringTypes.ToArray();
    }
    
    string [] GetAllSemanticTypes(int indexInGeometricTypes){
        return GetAllGeometricScriptableObjects()[indexInGeometricTypes].semantics;
    }





    public string GetRandomGeometric(){
        string [] stringTypes = GetAllGeometricTypesNames();
        string ans =  stringTypes[Random.Range(0, stringTypes.Length)];
        Debug.Log("GetRandomGeometric chosen geometric :" + ans);
        return ans;
    }
    

    public string GetRandomSemanticFromGeometric(string geometricName){

        foreach (var geometricScriptable in GetAllGeometricScriptableObjects())
        {
            if (geometricScriptable.typeName == geometricName)
                return geometricScriptable.semantics[Random.Range(0, geometricScriptable.semantics.Length)];
        }

        Debug.LogError("bad geometricType name in GetRandomSemanticFromGeometric");
        return "";
    }

    public string GetRandomSemantic(){
        string geometricName = GetRandomGeometric();
        // return geometricName + " " + GetRandomSemanticFromGeometric(geometricName);
        return GetRandomSemanticFromGeometric(geometricName);
    }


    public Sprite GetSpriteFromImageName(string imageName){
        
        foreach (var sprite in Resources.LoadAll<Sprite>(""))
        {
            if (sprite.name.Contains(imageName))
                return sprite;
        }

        Debug.LogError("bad image name in GetSpriteFromImageName");
        return null;

    }

    
/*  
   ------------------------------------------------------------------------------------------------------------------------

    EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES EXAMPLES

    ------------------------------------------------------------------------------------------------------------------------
*/

    // example on how to poll two totally random item names (semantically and geometrically)
    void RandomGeometryImage(){
        string image1 = GetRandomSemantic();

        string image2;
        do{
            image2 = GetRandomSemantic();
        }while (image2 == image1);
    
        Debug.Log("Two Random Geometric: \"" + image1 + "\" & \"" + image2 + "\"");
    }

    // example on how to poll two totally random item names only semantically (from the same geometry)
    void RandomSemanticImage(){
        string geometricType = GetRandomGeometric();
    
        string image1 = GetRandomSemanticFromGeometric(geometricType);

        string image2;
        do{
            image2 = GetRandomSemanticFromGeometric(geometricType);
        }while (image2 == image1);

        image1 = geometricType + " " + image1;
        image2 = geometricType + " " + image2;
        
        Debug.Log("Two Random Semantics: \"" + image1 + "\" & \"" + image2 + "\"");
        
    }
    

}