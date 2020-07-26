using Assets.Scripts.UnityLogic.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UnityCode
{
    public class PlayerStatTextUI : MonoBehaviour
    {
        public TextMeshProUGUI textField;
        public FloatValue fieldValue;

        private void Update()
        {
            textField.text = fieldValue.value.ToString();
        }
    }
}
