using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Value/Float", fileName = "FloatValue", order = 51)]
    public class FloatValue : ScriptableObject
    {
        public float value;
    }
}
