using UnityEngine;

public interface IGameObjectFactory<T> where T: IGameObject
{
    string GameObjectType { get; }
    T GetRoadSection();
    T[] GetMultipleRoadSections(int count);
}