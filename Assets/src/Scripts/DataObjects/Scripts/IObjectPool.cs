public interface IObjectPool<T> where T : IPoolObject
{
    void AddInactiveObject(T poolObject);
    void AddInactiveObjects(T[] poolObject);

    bool AddActiveObject(T poolObject);

    bool DeactivateObject(T poolObject);

    bool RecycleObject(out T recycledObject);
}