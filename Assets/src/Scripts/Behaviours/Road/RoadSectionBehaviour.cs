using UnityEngine;

public class RoadSectionBehaviour : MonoBehaviour, IRoadSection
{
    public RoadSectionData roadSectionData;
    public Transform newSpawnPosition;
    public FloatValue levelSpeed;
    public FloatValue roadSpeed;

    public RoadSectionData RoadSectionData => roadSectionData;
    public Vector2 NewSpawnPosition => newSpawnPosition.position;
    public FloatValue LevelSpeed => levelSpeed;
    public FloatValue RoadSpeed => roadSpeed;

    public Vector2 Position => transform.position;

    public void SetActive(bool isActive) => gameObject.SetActive(isActive);

    public void SetParent(Transform parent) => transform.parent = parent;

    public void SetPosition(Vector2 position) => transform.position = position;
}