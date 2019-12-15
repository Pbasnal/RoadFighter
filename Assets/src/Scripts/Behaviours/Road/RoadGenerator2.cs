using UnityEngine;

public class RoadGenerator2 : MonoBehaviour
{
    public RoadSectionFactory roadSectionFactory;
    public RoadSectionPool roadSectionPool;
    public int startingNumberOfSections = 3;
    public FloatValue levelSpeed;

    private RoadController roadController;

    private void Start()
    {
        if (roadSectionPool == null)
        {
            return;
        }
        roadController = new RoadController(roadSectionFactory, roadSectionPool, levelSpeed);
        var sections = roadController.InitializeSections(transform.position, startingNumberOfSections);
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].SetParent(transform);
        }
    }

    private void FixedUpdate()
    {
        if (roadController == null)
        {
            Start();
            return;
        }

        var sections = roadController.SpawnNewSectionIfAny();
        for (int i = 0; i < sections?.Length; i++)
        {
            sections[i].SetParent(transform);
        }

        foreach (Transform child in transform)
        {
            var roadSection = child.GetComponent<RoadSectionBehaviour>();
            if (roadSection == null || !child.gameObject.activeInHierarchy)
            {
                continue;
            }

            roadController.MoveActiveSection(roadSection);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var roadSection = collision.gameObject.GetComponent<RoadSectionBehaviour>();

        if (roadSection == null)
        {
            return;
        }

        roadController.EnqueueForRecycle(roadSection); 
    }
}
