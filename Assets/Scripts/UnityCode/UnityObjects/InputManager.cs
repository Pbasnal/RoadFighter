using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public abstract class InputManager : ScriptableObject
    {
        public abstract InputCommand GetCommand();
    }
}
