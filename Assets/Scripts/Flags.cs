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
    public bool spawnLine;
    public bool spawnColumn;
    public bool spawnRandom;
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
    public bool geometricImages;
    public bool semanticImages;
}

[Serializable]
 public class ImagesFlags : Images, ISerializationCallbackReceiver
 {
     public void OnBeforeSerialize(){}
     
     public void OnAfterDeserialize(){}
 }
