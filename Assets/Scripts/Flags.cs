using UnityEngine;
using System;
using System.Collections.Generic;
/**
    To use:
        i.e. "SerializableDictionary<string, int> {}"
*/

[Serializable]
public abstract class Spread
{
    public bool isLine;
    public bool isColumn;
    public bool isRandom;
}

[Serializable]
 public class SpreadFlags : Spread, ISerializationCallbackReceiver
 {
     public void OnBeforeSerialize(){}
     
     public void OnAfterDeserialize(){}

 }


[Serializable]
public abstract class Images
{
    public bool isGeometric;
    public bool isSemantic;
}

[Serializable]
 public class ImagesFlags : Images, ISerializationCallbackReceiver
 {
     public void OnBeforeSerialize(){}
     
     public void OnAfterDeserialize(){}
 }
