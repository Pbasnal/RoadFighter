namespace Assets.Scripts.UnityLogic.CommonInterface
{
    public interface IObjectPool<T>
    {
        void AddActiveObject(T poolObject);
        void AddInactiveObject(T poolObject);
        void DeactivateObject(T poolObject);
        T RecycleObject();
    }
}