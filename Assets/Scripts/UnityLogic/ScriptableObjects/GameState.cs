using UnityEngine;

namespace Assets.Scripts.UnityLogic.ScriptableObjects
{
    [CreateAssetMenu(menuName = "GameState", fileName = "GameState", order = 51)]
    public class GameState : ScriptableObject
    {
        public States State { get; set; }
    }
}
