using UnityEngine;

public class RoadSectionBehaviour : MonoBehaviour
{
    public RoadSectionData roadSectionData;
    public Transform[] newSpawnPosition;

    private float roadLength;

    private void Update()
    {
        transform.position += roadSectionData.direction.normalized * roadSectionData.velocity * Time.deltaTime;        
    }
}