using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityLogic.BehaviourInterface;

namespace UnityCode.UiEffects
{
    public class PointMultiplierEffects : FloatValueListener
    {
        public FloatValue playerPointsMultiplier;
        public AnimationPlayer animationPlayer;

        private float prevValue = 1;

        private void Update()
        {
            if (prevValue >= playerPointsMultiplier.value)
            {
                prevValue = playerPointsMultiplier.value;
                return;
            }

            if (animationPlayer != null)
            {
                prevValue = playerPointsMultiplier.value;
                animationPlayer.PlayFromBegining("MultiplierIncreased");
            }
        }
    }
}
