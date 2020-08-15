namespace Assets.Scripts.UnityLogic.BehaviourInterface
{
    public interface IEnemy
    {
        string Name { get; }
        float StartTime { get; }
        float MinYTime { get; set; }
        float MaxYTime { get; set; }
        float Speed { get; }
    }
}
