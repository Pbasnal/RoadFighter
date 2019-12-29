namespace Assets.Scripts.UnityLogic.CommonInterface
{
    public interface IObjectPool<T>
    {
        void AddActiveObject(string objectype, T gameObject);
        void AddInactiveObject(string objectType, T gameObject);
        void DeactivateObject(T gameObject);
        T RecycleObject();
    }
}