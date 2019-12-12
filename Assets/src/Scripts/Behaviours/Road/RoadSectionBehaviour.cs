using UnityEngine;

public class RoadSectionBehaviour : MonoBehaviour
{
    public RoadSectionData roadSectionData;
    public Transform[] newSpawnPosition;
    public FloatValue levelSpeed;

    private float roadSpeed;
    private float roadLength;

    private void Start()
    {
        roadSpeed = levelSpeed.value * 0.4f;
    }

    private void Update()
    {
        transform.position += roadSectionData.direction.normalized * roadSpeed * Time.deltaTime;
    }
}