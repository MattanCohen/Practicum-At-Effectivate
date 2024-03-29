using UnityEngine;
using System;
using System.Collections.Generic;
/**
    To use:
        i.e. "SerializableDictionary<string, int> {}"
*/

public class Ints
{
    public int min;
    public int max;
}

[Serializable]
 public class MinMax : Ints, ISerializationCallbackReceiver
 {
     public void OnBeforeSerialize(){}
     
     public void OnAfterDeserialize(){}
    
    public MinMax(int _min, int _max){min = _min; max = _max;}
 }  
