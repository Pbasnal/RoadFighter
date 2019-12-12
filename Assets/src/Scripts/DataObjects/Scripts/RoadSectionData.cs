using UnityEngine;

[CreateAssetMenu(fileName = "Road Section Data", menuName = "Level/Road/SectionData", order = 51)]
public class RoadSectionData : ScriptableObject
{
    public float velocity;
    public Vector3 direction;
    public Vector3 newSpawnPoint;
    public int roadSectionType;
}
