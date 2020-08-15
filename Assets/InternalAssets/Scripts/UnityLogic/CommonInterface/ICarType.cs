using UnityEngine;

namespace UnityLogic.CommonInterface
{
    public interface ICanBePooled
    {
        string Name { get; }
        bool IsActive { get; }
        string Id { get; }
        GameObject SpawnedObject { get; }

        ICanBePooled SpawnANewInstance();
        void SetActive(bool isActive);
    }
}
