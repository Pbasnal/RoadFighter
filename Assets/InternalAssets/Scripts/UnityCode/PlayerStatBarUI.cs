using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine.UI;
using UnityLogic.BehaviourInterface;

namespace Assets.Scripts.UnityCode
{
    public class PlayerStatBarUI : FloatValueListener
    {
        public Image imageFill;
        public FloatValue playerHealth;

        private void Update()
        {
            if (playerHealth.value > 0)
            {
                imageFill.fillAmount = playerHealth.value / 100;
            }
            else
            {
                imageFill.fillAmount = 0;
            }
        }
    }
}
