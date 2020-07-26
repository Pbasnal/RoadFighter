using UnityEngine;

namespace UnityCode
{
    public class CustomTime : ScriptableObject
    {
        public float scale;
        public float deltaTime => Time.deltaTime * scale;
    }
}
