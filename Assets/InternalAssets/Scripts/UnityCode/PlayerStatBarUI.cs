using Assets.Scripts.UnityLogic.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UnityCode
{
    public class PlayerStatBarUI : MonoBehaviour
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
