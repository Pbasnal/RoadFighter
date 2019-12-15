using UnityEngine;

public interface IRoadSection : IPoolObject, IGameObject
{
    Vector2 Position { get; }
    RoadSectionData RoadSectionData { get; }
    Vector2 NewSpawnPosition { get; }
    FloatValue LevelSpeed { get; }
    FloatValue RoadSpeed { get; }

    void SetPosition(Vector2 position);
    void SetActive(bool isActive);
    void SetParent(Transform parent);
}