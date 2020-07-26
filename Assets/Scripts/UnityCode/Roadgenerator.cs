using System.Collections.Generic;
using Assets.Scripts.UnityCode;
using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityCode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Roadgenerator : PausableBehaviour
{
    public GameState gameState;
    public FloatValue levelSpeed;
    public int numberOfSections;
    public RoadsectionPool roadsectionPool;

    private void Awake()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private List<Roadsection> sections;
    private Queue<Roadsection> recycleQueue;
    private Roadsection topmostSection;

    // Use this for initialization
    private void Start()
    {
        var spawnPoint = transform.position;
        sections = new List<Roadsection>();
        recycleQueue = new Queue<Roadsection>();
        for (int i = 0; i < numberOfSections; i++)
        {
            var roadSection = GetRoadsection();
            roadSection.transform.position = spawnPoint;

            sections.Add(roadSection);

            spawnPoint = new Vector2(spawnPoint.x, roadSection.SpawnHeight);
            topmostSection = roadSection;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameState.State != States.Running)
        {
            return;
        }

        RecycleSections();
        sections.ForEach(s => s.transform.position = new Vector2(s.transform.position.x, s.transform.position.y - levelSpeed.value * Time.deltaTime));
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameState.State != States.Running)
        {
            return;
        }

        //Roadsection
        if (collision.gameObject.tag == "Roadsection")
        {
            recycleQueue.Enqueue(collision.gameObject.GetComponent<Roadsection>());
        }
    }

    private void RecycleSections()
    {
        while (recycleQueue.Count > 0)
        {
            var r = recycleQueue.Dequeue();
            r.transform.position = new Vector2(topmostSection.transform.position.x, topmostSection.SpawnHeight);
            topmostSection = r;
        }
    }

    private Roadsection GetRoadsection()
    {
        var roadsection = roadsectionPool.RecycleObject();
        if (roadsection != null)
        {
            return roadsection;
        }

        roadsection = Instantiate(roadsectionPool.prefab).GetComponent<Roadsection>();
        roadsection.transform.position = transform.position;
        roadsection.transform.parent = transform;
        roadsectionPool.AddActiveObject("", roadsection);

        return roadsection;
    }
}
