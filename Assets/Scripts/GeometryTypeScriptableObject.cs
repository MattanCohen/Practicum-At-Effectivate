using UnityEngine;

[CreateAssetMenu(fileName = "new Geometry Type", menuName = "Geometry Types", order = 0)]
public class GeometryTypeScriptableObject : ScriptableObject {

    public string typeName;
    public string [] semantics;

}