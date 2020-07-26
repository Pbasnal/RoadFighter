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
            imageFill.fillAmount = playerHealth.value / 100;
        }
    }
}
