using UnityEngine;
using System.Collections;

public class MultilaneRoad : MonoBehaviour
{
    public Transform[] lanes;

    [HideInInspector]public Vector2[] lanePositions;

    // Use this for initialization
    void Start()
    {
        if (lanes == null || lanes.Length == 0)
        {
            return;
        }

        lanePositions = new Vector2[lanes.Length];

        for (int i = 0; i < lanePositions.Length; i++)
        {
            lanePositions[i] = lanes[i].position;
        }
    }
}
