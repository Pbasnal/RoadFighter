using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RoadSectionFactory", menuName = "Factory/RoadSection", order = 52)]
public class RoadSectionFactory : ScriptableObject, IGameObjectFactory<IRoadSection>
{
    public GameObject prefab;
    public readonly string gameObjectType;

    public string GameObjectType => gameObjectType;

    public IRoadSection[] GetMultipleRoadSections(int count)
    {
        var instances = new IRoadSection[count];
        for (int i = 0; i < count; i++)
        {
            instances[i] = GetRoadSection();
        }
        return instances;
    }

    public IRoadSection GetRoadSection()
    {
        var component = prefab.GetComponent<RoadSectionBehaviour>();
        if (component == null)
        {
            return null;
        }
        var section = Instantiate(prefab).GetComponent<RoadSectionBehaviour>();
        section.name += Guid.NewGuid().ToString();

        return section;
    }
}
