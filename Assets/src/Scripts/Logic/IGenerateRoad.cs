using UnityEngine;

public interface IGenerateRoad
{
    GameObjectPool RoadSectionPool { get; set; }
    int StartingNumberOfSections { get; set; }
    FloatValue LevelSpeed { get; set; }
    Vector2 Position { get; }
    Transform Transform { get; }
}
