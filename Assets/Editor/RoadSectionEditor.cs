using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadSectionBehaviour))]
public class RoadSectionEditor : Editor
{
    private void OnSceneGUI()
    {
        var roadSection = target as RoadSectionBehaviour;
        var transform = roadSection.transform;

        Handles.color = Color.green;
        Handles.DrawLine(transform.position, transform.position + roadSection.roadSectionData.direction.normalized);        
    }
}



