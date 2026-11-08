using UnityEngine;

[CreateAssetMenu(fileName = "PointData", menuName = "Scriptable Objects/PointData")]
public class PointData : ScriptableObject
{
    public string namePoint;
    public int amount;
    public GameObject prefabPoint;
}
