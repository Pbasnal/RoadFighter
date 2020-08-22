using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public interface IInputManager
    {
        float TimeWhenCommandCame { get; }
        InputCommand GetCommand();
    }

    public abstract class ScriptableInputManager : ScriptableObject, IInputManager
    {
        public abstract float TimeWhenCommandCame { get; set; }

        public abstract InputCommand GetCommand();
    }

    public abstract class BehaviourInputManager : MonoBehaviour, IInputManager
    {
        public abstract float TimeWhenCommandCame { get; set; }

        public abstract InputCommand GetCommand();
    }
}
