using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    //public abstract class InputManager : ScriptableObject
    //{
    //    public abstract InputCommand GetCommand();
    //}

    public interface IInputManager
    {
        InputCommand GetCommand();
    }

    public abstract class ScriptableInputManager : ScriptableObject, IInputManager
    {
        public abstract InputCommand GetCommand();
    }

    public abstract class BehaviourInputManager : MonoBehaviour, IInputManager
    {
        public abstract InputCommand GetCommand();
    }
}
