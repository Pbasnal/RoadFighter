using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Road Lane", menuName = "Level/Road Lane", order = 51)]
public class RoadLaneData : ScriptableObject
{
    public int LaneWidthInUnits = 1;
    //public int MaxNumberOfLanes = 5;
}
